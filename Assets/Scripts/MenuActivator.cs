using System;
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
    public GameObject tasksPanel;//!!
    public GameObject tasksManager;//!!
    public GameObject graphPanel;//!!
    public GameObject graphManager;//!!
    //InputField for labels. Used for inputting text for labels
    public InputField inputField;
    public Transform camera;
    public GameObject player;
    public GameObject eventSystem;
    public Transform labObjs;

    private Transform tube;
    private Transform tubeCanvas;
    private Text tubeCanvasTxt;
    private bool labelOpen = false;
    private bool cmdOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        notebookPanel.SetActive(false);
        tasksPanel.SetActive(false);
        graphPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen == false)
            {
                camera.GetComponent<MouseLook>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                menuBackground.SetActive(true);
                menuOpen = true;
                //Set timeScale to 0 so game pauses
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
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
        //Check if the enter key is pressed. Enter doesn't do anything when pause menu is open
        if (Input.GetKeyDown(KeyCode.Return) && !menuOpen)
        {
            if (!labelOpen && !cmdOpen)
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
            else if (!labelOpen && cmdOpen)
            {
                cmdOpen = false;
            }
            else if (labelOpen)
            {
                labelOpen = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menuOpen)
            {
                camera.GetComponent<MouseLook>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                menuBackground.SetActive(true);
                menuOpen = true;
                //Set timeScale to 0 so game pauses
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                OpenNotebook();
            }
            else
            {
                camera.GetComponent<MouseLook>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                menuBackground.SetActive(false);
                menuOpen = false;
                //Set timeScale to 1 so game unpauses
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                CloseNotebook();
            }
        }
        //Check if console has been opened
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (!cmdOpen)
            {
                //Disable MouseLook and PlayerMovement scripts
                camera.GetComponent<MouseLook>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;

                cmdOpen = true;

                //Activate notes
                inputField.gameObject.SetActive(true);
                inputField.ActivateInputField();

                //When user presses enter while writing label, label is submitted
                inputField.onEndEdit.AddListener(SubmitCommand);
            }
            else
            {
                cmdOpen = false;
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

    //!!
    public void OpenTasks()
    {
        tasksPanel.SetActive(true);
        Tasks tasksScript = tasksManager.GetComponent<Tasks>();
        tasksScript.open();
    }

    public void CloseTasks()
    {
        tasksPanel.SetActive(false);
    }
    //!!

    //!!
    public void OpenGraph()
    {
        graphPanel.SetActive(true);
        Graph graphScript = graphManager.GetComponent<Graph>();
        graphScript.open();
    }

    public void CloseGraph()
    {
        graphPanel.SetActive(false);
        Graph graphScript = graphManager.GetComponent<Graph>();
        graphScript.close();
    }
    //!!

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

    //Submits command for devmode
    private void SubmitCommand(string str)
    {
        if (str.Contains("task_"))
        {
            int index = str.IndexOf("_");
            string sub = str.Substring(index + 1);

            try
            {
                int num = Int32.Parse(sub);
                EventSystem eventSystemScript = eventSystem.GetComponent<EventSystem>();
                eventSystemScript.setUpTask(num);
            }
            catch (FormatException)
            {
                Debug.Log("Improper task command format! Should be 'task_(task number)'. Was " + str + " found int:" + sub);
            }

        }

        inputField.onEndEdit.RemoveListener(SubmitCommand);

        //Deactivate inputField
        inputField.DeactivateInputField();

        inputField.gameObject.SetActive(false);

        camera.GetComponent<MouseLook>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
