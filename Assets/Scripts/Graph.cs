using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    public Transform player;
    public GameObject camera;
    public GameObject canvas;

    public GameObject eq;

    public GameObject graphTable;
    public Transform pointsOffsetMin;
    public Transform pointsOffsetMax;
    public GameObject EventSystem;
    private EventSystem eventSystemScript;

    private List<Vector2> points = new List<Vector2>();
    private float a = 0f;
    private float b = 0f;
    private float rSquared = 0f;

    private List<Transform> pointsTransforms = new List<Transform>();
    private float minX = -213;
    private float maxX = 237;
    private float minY = -112;
    private float maxY = 152;

    public GameObject line;

    public GameObject nextButtonObj;
    public Button nextButton;

    private bool showingPoints = false;
    private bool showingLine = false;

    public bool pointsPlotted = false;
    public bool linePlotted = false;
    public bool eqGenerated = false;

    private GameObject point7;

    //True if canvas render mode is set to screen space. Screen space needed for line.
    private bool canvasRenderMode = false;

    // Start is called before the first frame update
    void Start()
    {
        eventSystemScript = EventSystem.GetComponent<EventSystem>();
        //pointsParent = graphTable.transform.Find("Points");
        nextButton.onClick.AddListener(graphPoints);
        point7 = graphTable.transform.Find("Point7").gameObject;
        point7.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getInfo(List<Vector2>points, float a, float b, float rSquared)
    {
        this.points = points;
        this.a = a;
        this.b = b;
        this.rSquared = rSquared;
    }

    /*
     * X axis:
     * Begins at X = -213
     * Dist b/w 0.00 & 0.002 (dist b/w 2 points) = 90
     * Dist b/w points = 45
     * 
     * Y axis:
     * Begins at Y = -112
     * Dist b/w 0.0 & 0.2 (dist b/w 2 points) = 66
     * Dist b/w points = 33
     */

    public void open()
    {
        //CHANGE CANVAS TO SCREEN-SPACE CAMERA TO USE LINE RENDERER
        //graphPoints();

        if (canvasRenderMode)
        {
            Canvas m_Canvas = canvas.GetComponent<Canvas>();
            m_Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }

    public void close()
    {
        if (canvasRenderMode)
        {
            Canvas m_Canvas = canvas.GetComponent<Canvas>();
            m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }

    public void graphPoints()
    {
        Debug.Log("GraphPts");
        int i = 1;
        foreach(Vector2 point in points)
        {
            Transform curTransform = graphTable.transform.Find("Point" + i);
            
            float ratio = point.x / 0.01f;
            float posX = Mathf.Lerp(pointsOffsetMin.position.x, pointsOffsetMax.position.x, ratio);
            
            ratio = point.y / 1f;
            float posY = Mathf.Lerp(pointsOffsetMin.position.y, pointsOffsetMax.position.y, ratio);

            Vector3 pos = new Vector3(posX, posY, 0f);
            curTransform.position = pos;
            pointsTransforms.Add(curTransform);
            Debug.Log("Point[" + i + "]: X:" + curTransform.position.x + " Y:" + curTransform.position.y + " Z:"+curTransform.position.z + "Pos.Z:"+pos.z);
            i++;
        }
        graphTable.transform.Find("Point7").gameObject.SetActive(false);
        pointsPlotted = true;

        nextButton.onClick.RemoveListener(graphPoints);
        nextButton.onClick.AddListener(plotLinReg);

        //plotLinReg();
    }

    public void plotLinReg()
    {
        Debug.Log("PlotLinReg");

        //https://forum.unity.com/threads/any-good-way-to-draw-lines-between-ui-elements.317902/
        /*
        rectTransform.localPosition = (object1.localPosition + object2.localPosition) / 2;
        Vector3 dif = object2.localPosition - object1.localPosition;
        rectTransform.sizeDelta = new Vector3(dif.magnitude, 5);
        rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        */
        Vector3 dir = (pointsOffsetMin.localPosition - pointsOffsetMax.localPosition).normalized;
        Vector3 forward = pointsOffsetMin.TransformDirection(dir) * 10;
        //Debug.DrawRay(pointsOffsetMin.position, forward, Color.green);
        Debug.DrawLine(pointsOffsetMin.localPosition, pointsOffsetMax.localPosition, Color.red, Mathf.Infinity);

        
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        var linePoints = new Vector3[] { pointsTransforms.ToArray()[0].localPosition, pointsTransforms.ToArray()[pointsTransforms.Count-1].localPosition };
        lineRenderer.SetPositions(linePoints);

        Vector3 linePos = line.transform.localPosition;
        //Position line object at midpoint b/w point 0 and point 1. Very hacky.
        linePos = Vector3.Lerp(pointsTransforms.ToArray()[0].localPosition, pointsTransforms.ToArray()[pointsTransforms.Count - 1].localPosition, 0.5f);
        line.transform.parent.localPosition = linePos;

        lineRenderer.sortingOrder = 1;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = Color.red;
        //lineRenderer.useWorldSpace = true;
        Canvas m_Canvas = canvas.GetComponent<Canvas>();
        m_Canvas.renderMode = RenderMode.ScreenSpaceCamera;

        nextButton.onClick.RemoveListener(plotLinReg);
        nextButton.onClick.AddListener(genEq);
    }

    public void genEq()
    {
        Debug.Log("GenEq");
        string str = "y=" + a.ToString("#.0000") + "+" + b.ToString("#.0000") + "*x\nR^2="+ rSquared.ToString("#.0000");
        Text text = eq.GetComponent<Text>();
        text.text = str;
        nextButton.onClick.RemoveListener(genEq);
        nextButtonObj.SetActive(false);
        eqGenerated = true;
    }

    public void graphMysteryPoint(Vector2 point)
    {
        Debug.Log("GraphMystPt");
        points.Add(point);

        point7.gameObject.SetActive(true);
        Transform curTransform = point7.transform;
        
        float ratio = point.x / 0.01f;
        float posX = Mathf.Lerp(pointsOffsetMin.position.x, pointsOffsetMax.position.x, ratio);

        ratio = point.y / 1f;
        float posY = Mathf.Lerp(pointsOffsetMin.position.y, pointsOffsetMax.position.y, ratio);

        Vector3 pos = new Vector3(posX, posY, curTransform.position.z);
        curTransform.position = pos;
        pointsTransforms.Add(curTransform);

        Debug.Log("Point[7]: X:" + curTransform.position.x + " Y:" + curTransform.position.y + " Z:" + curTransform.position.z + "Pos.Z:" + pos.z);
        
        pointsPlotted = true;

    }

    public void clickPoint(int num)
    {
        GameObject point = pointsTransforms.ToArray()[num].gameObject;
        string coords = "(" + points.ToArray()[num].x + ", " + points.ToArray()[num].y + ")";
        GameObject child = point.transform.Find("Text").gameObject;
        Text text = child.GetComponent<Text>();
        text.text = coords;

        Debug.Log("Point " + num + " clicked: "+coords);
    }

    public void showPoints()
    {
        if (pointsPlotted)
        {
            for (int num = 0; num < points.Count; num++)
            {
                GameObject point = pointsTransforms.ToArray()[num].gameObject;
                string coords = "(" + points.ToArray()[num].x + ", " + points.ToArray()[num].y + ")";
                GameObject child = point.transform.Find("Text").gameObject;
                Text text = child.GetComponent<Text>();

                if (!showingPoints)
                {
                    text.text = coords;
                }
                else
                {
                    text.text = "";
                }

                Debug.Log("Point " + num + " clicked: " + coords);
            }

            if (!showingPoints) showingPoints = true;
            else showingPoints = false;
        }
    }

    public void showLine()
    {
        if (linePlotted)
        {
            if (!showingLine)
            {
                line.SetActive(true);
                showingLine = true;
            }
            else
            {
                line.SetActive(false);
                showingLine = false;
            }
        }
    }
}
