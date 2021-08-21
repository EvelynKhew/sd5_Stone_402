using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipetteScript : MonoBehaviour
{
    private GameObject pipette;
    private GameObject pipCanvas;
    private Text txtTop;
    private Text txtMid;
    private Text txtMid2;
    private Text txtBottom;
    private Button buttonNeg;
    private Button buttonPos;

    public int volume;

    // Start is called before the first frame update
    void Start()
    {
        pipette = gameObject;
        volume = 0;

        //Get the canvas for pipette display
        pipCanvas = pipette.transform.GetChild(1).gameObject;
        Text[] texts = pipCanvas.transform.GetChild(0).GetComponentsInChildren<Text>();
        txtTop = texts[0];
        txtMid = texts[1];
        txtMid2 = texts[2];
        txtBottom = texts[3];

        Button[] buttons = pipCanvas.transform.GetChild(0).GetComponentsInChildren<Button>();
        buttonNeg = buttons[0].GetComponent<Button>();
        buttonPos = buttons[1].GetComponent<Button>();

        buttonNeg.onClick.AddListener(NegOnClick);
        buttonPos.onClick.AddListener(PosOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        //If player is holding this
        if (transform.parent.parent.name == "Player")
        {
            //Pipette display logic
            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                volume += 10;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.RightArrow))
            {
                volume -= 10;
            }
            //Volume maxes out at 300
            if (volume > 1000) volume = 0;
            if (volume < 0) volume = 1000;

            string str = volume.ToString();

            string bottom = "";
            string mid2 = "";
            string mid = "";
            string top = "";

            bottom = str[0] + "";
            if (volume > 9)
            {
                mid2 = str[0] + "";
                bottom = str[1] + "";
            }
            else mid2 = 0 + "";
            if (volume > 99)
            {
                mid = str[0] + "";
                mid2 = str[1] + "";
                bottom = str[2] + "";
            }
            else mid = 0 + "";
            if (volume > 999)
            {
                top = str[0] + "";
                mid = str[1] + "";
                mid2 = str[2] + "";
                bottom = str[3] + "";
            }
            else top = 0 + "";

            txtTop.text = top;
            txtMid.text = mid;
            txtMid2.text = mid2;
            txtBottom.text = bottom;
        }

        //Pipette display rotation logic//
        //Idea: Pipette display always faces the camera
        //Dummy code to rotate pipette
        /*
        if (Input.GetKey(KeyCode.E))
        {
            pipette.transform.eulerAngles = new Vector3(
                pipette.transform.eulerAngles.x,
                pipette.transform.eulerAngles.y + 1,
                pipette.transform.eulerAngles.z
            );

        }
        else if (Input.GetKey(KeyCode.Q))
        {
            pipette.transform.eulerAngles = new Vector3(
                pipette.transform.eulerAngles.x,
                pipette.transform.eulerAngles.y - 1,
                pipette.transform.eulerAngles.z
            );
        }
        else if (Input.GetKey(KeyCode.R))
        {
            pipette.transform.eulerAngles = new Vector3(
                pipette.transform.eulerAngles.x + 1,
                pipette.transform.eulerAngles.y,
                pipette.transform.eulerAngles.z
            );
        }
        else if (Input.GetKey(KeyCode.F))
        {
            pipette.transform.eulerAngles = new Vector3(
                pipette.transform.eulerAngles.x - 1,
                pipette.transform.eulerAngles.y,
                pipette.transform.eulerAngles.z
            );
        }
        */
        /*
        //Have pipette display always facing camera
        pipCanvas.transform.eulerAngles = new Vector3(
            GetComponent<Camera>().transform.eulerAngles.x * -1.0f,
            GetComponent<Camera>().transform.eulerAngles.y * -1.0f,
            GetComponent<Camera>().transform.eulerAngles.z * -1.0f
        );
        */

    }

    void NegOnClick()
    {
        volume--;
    }
    void PosOnClick()
    {
        volume++;
    }

}
