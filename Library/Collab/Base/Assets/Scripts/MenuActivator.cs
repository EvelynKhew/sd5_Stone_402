using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuActivator : MonoBehaviour
{
    public GameObject menuBackground;
    public Button tasks;
    public Button notebook;
    public bool menuOpen;
    public GameObject notebookPanel;
    //InputField for labels. Used for inputting text for labels
    public InputField inputField;
    public Transform camera;
    public GameObject player;
    public Transform labObjs;

    private Transform tube;
    private Transform tubeCanvas;
    private Text tubeCanvasTxt;
    private bool labelOpen = false;

     // Start is called before the first frame update
     void Start()
     {
        notebookPanel.SetActive(false);
     }

    // Update is called once per frame
    void Update()
    {
        //checks if the esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuOpen == false) {
                camera.GetComponent<MouseLook>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                menuBackground.SetActive(true);
                menuOpen = true;
                //Set timeScale to 0 so game pauses
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            } else {
                camera.GetComponent<MouseLook>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                menuBackground.SetActive(false);
                menuOpen = false;
                //Set timeScale to 1 so game unpauses
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

            }
        }
        //Check if the enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!labelOpen)
            {
                //Disable MouseLook and PlayerMovement scripts
                camera.GetComponent<MouseLook>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;

                
                //See if name contains Flask_02 Variant so Flask_02 Variant(1) still works
                GameObject cameraObj = camera.gameObject;
                Transform[] objs = cameraObj.GetComponentsInChildren<Transform>();
                foreach (Transform g in objs)
                {
                    if (g.name.Contains("Flask")) tube = g.transform;
                }

                if (tube != null)
                {
                    labelOpen = true;

                    //Activate notes
                    inputField.gameObject.SetActive(true);
                    inputField.ActivateInputField();

                    //When user presses enter while writing label, label is submitted
                    inputField.onEndEdit.AddListener(SubmitLabel);
                }
                
            }
            else if (labelOpen)
            {
                labelOpen = false;
            }
        }
    }


    public void OpenNotebook()
    {
        notebookPanel.SetActive(true);
    }

    public void CloseNotebook()
    {
        notebookPanel.SetActive(false);
    }

    public void NextTable()
    {
        //Changes to the next table. Array/list of tables isn't done yet, so currently does nothing
        Debug.Log("In NextTable method");
    }

    public void PreviousTable()
    {
        //Changes to the next table. Array/list of tables isn't done yet, so currently does nothing
        Debug.Log("In PreviousTable method");
    }

    //Saves label for test tube
    private void SubmitLabel(string str)
    {
        //Update tube display canvas
        tubeCanvas = tube.GetChild(1);
        Text[] texts = tubeCanvas.GetChild(0).GetComponentsInChildren<Text>();
        tubeCanvasTxt = texts[0];
        tubeCanvasTxt.text = str;

        inputField.onEndEdit.RemoveListener(SubmitLabel);

        //Deactivate inputField
        inputField.DeactivateInputField();
        
        inputField.gameObject.SetActive(false);

        camera.GetComponent<MouseLook>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
