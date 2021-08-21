using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tasks : MonoBehaviour
{
    public GameObject taskTable;

    public GameObject EventSystem;
    private EventSystem eventSystemScript;

    private GameObject instructionText;
    private List<GameObject> descriptionTexts = new List<GameObject>();
    private Transform rowLabel;

    private static string[] T0goal = new string[] { "First, put your gloves on to stay safe.",
        "Left click to interact with an object",
        "Right click to pick up object"
        };
    private static string[] T1goal = new string[] { "Next, prepare the six test tubes. Begin by labeling the first tube '1', then fill out the final protein concentration for the tubes in the notebook.",
        "Right click the tube to pick it up. While holding the tube, press Enter to fill out the label and press Enter again to submit.",
        "To fill out the final protein concentrations, press TAB or press Esc to open the pause menu, press the 'Open Notes' button."
        };
    private static string[] T2goal = new string[] { "Set up your pipette by picking it up and setting the volume to 1000",
        "Right click the pipette with 1000 micro liters to pick it up.",
        "To set the volume, scroll the mouse wheel. Scrolling downward from volume 0 loops to volume 1000."
        };
    private static string[] T3goal = new string[] { "Transfer fluids from the NaCl, Dye, and Protein beakers to tube 1.",
        "To transfer fluids from a beaker to a test tube, left click the beaker while holding the pipette, then left click the tube while holding the pipette.",
        "The order of fluids is NaCl->tube1, Dye->tube1, Dye->tube1, Dye->tube1, Dye->tube1, Protein->tube1."
        };

    private static string[] T4goal = new string[] { "Shake tubes." };

    private static string[] T5goal = new string[] { "Prepare the Spectrometer, record the tube absorbances in the notebook, graph the points, plot the linear regression, then finally generate the line equation.",
        "Set the Spectrometer wavelength 595 by clicking the right knob and scrolling the mouse wheel.",
        "Load tube 0 into the Spectrometer by picking it up and left clicking the Spectrometer. Then click the Spectrometer's left knob to zero it out.",
        "To view a tube's absorbance, load it into the Spectrometer and left click the Spectrometer display.",
        "Once absorbances are recorded, click the 'Next' button in the notebook to graph the tube points.",
        "Once the points are graphed, click the 'Next' button in the notebook to plot the linear regression.",
        "Once the linear regression is plotted, click the 'Next' button in the notebook to generate the line equation.",
        };

    private static string[] T6goal = new string[] { "Prepare tube 1 again, same as task 3, then prepare the Spectrometer, record the tube absorbances, record the tube unknown protein concentrations, and finally record the beaker unknown protein concentration.",
        "The order of fluids is NaCl->tube1, Dye->tube1, Dye->tube1, Dye->tube1, Dye->tube1, Protein->tube1.",
        "Load tube 0 into the Spectrometer by picking it up and left clicking the Spectrometer. Then click the Spectrometer's left knob to zero it out.",
        "To view a tube's absorbance, load it into the Spectrometer and left click the Spectrometer display.",
        "Record the correct absorbances of all of the two tubes.",
        "Calculate tube 1's unknown protein concentration using the newly plotted point.",
        "Calculate the protein beaker's unknown protein concentration using tube 1's unknown protein concentration."
        };

    // Start is called before the first frame update
    void Start()
    {
        eventSystemScript = EventSystem.GetComponent<EventSystem>();

        rowLabel = taskTable.transform.Find("RowLabel");
        instructionText = rowLabel.transform.Find("Row1").Find("InstructionText").gameObject;

        for (int i = 2; i <= 7; i++)
        {
            GameObject desc = rowLabel.transform.Find("Row" + i).Find("DescriptionText").gameObject;
            descriptionTexts.Add(desc);
        }

        foreach (GameObject desc in descriptionTexts)
        {
            Debug.Log("Desc text:" + desc.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void open()
    {
        int task = eventSystemScript.curTask;

        switch (task)
        {
            case 0:
                setRowTexts(T0goal);
                break;
            case 1:
                setRowTexts(T1goal);
                break;
            case 2:
                setRowTexts(T2goal);
                break;
            case 3:
                setRowTexts(T3goal);
                break;
            case 4:
                setRowTexts(T4goal);
                break;
            case 5:
                setRowTexts(T5goal);
                break;
            case 6:
                setRowTexts(T6goal);
                break;
        }

        //GameObject taskNum = rowLabel.Find("TaskNum").gameObject;
        Text taskNumText = rowLabel.transform.Find("Row1").GetComponent<Text>();
        taskNumText.text = "Task: " + task;
    }

    void setRowTexts(string[] texts)
    {
        Text text = instructionText.GetComponent<Text>();
        text.text = texts[0];

        for (int i = 0; i < descriptionTexts.Count; i++)
        {
            string str = "";
            GameObject description = descriptionTexts.ToArray()[i];
            text = description.GetComponent<Text>();

            if (i + 1 < texts.Length) str = texts[i + 1];
            text.text = str;
        }
    }
}

/*
 * 400.87
 * 34.4183
 * 
 * 
 * 
 * 
 */