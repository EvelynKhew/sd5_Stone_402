using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EventSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public GameObject labObjs;
    public GameObject gloveBox;
    public GameObject pipette;
    public GameObject pipette1;
    public GameObject pipette2;
    public GameObject beakerNaCl;
    public GameObject beakerDye;
    public GameObject beakerProtein;
    public GameObject parafilm;
    public GameObject spectrometer;
    private GameObject spectrometerLeftKnob;
    private GameObject spectrometerRightKnob;

    private Notebook notebookScript;
    private PickUp pickUpScript;
    private ImmobileClick immobileClickScript;
    private Spectrometer spectrometerScript;
    private Canvas item;
    private float mysteryConcentrate;
    private string lastBeaker;
    //ALEJANDRO How much the height of fluid in the tube should increase when clicked on while holding the pipette
    private const float tubeFillHeight = 0.1f;

    //The naclM, spM, and fpM concentrations of the tubes. Ex: tube 0 has nacl 1, sp 1, and fp 0
    //See Frontend/virtualbiochemlab/src/Pages/Modules/Module1/Module1.tsx
    float[] naclM = new float[] { 1f, 0.8f, 0.6f, 0.4f, 0.2f, 0f };
    float[] spM = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
    float[] fpM = new float[] { 0f, 0.002f, 0.004f, 0.006f, 0.008f, 0.01f };
    //Absorbance & transmission
    //See Frontend/virtualbiochemlab/src/Pages/Modules/Module1/steps1/Module1Step6.tsx
    //console.log("Absorb Trans "+(i+1)+": "+(b*concentration).toFixed(3)+" \t\t "+(10**(-(b*concentration))*100))
    //abs[i] = fpM[i]*b
    float[] absorbances = new float[6];
    //See Frontend/virtualbiochemlab/src/Pages/Modules/Module1/steps1/Module1Step6.tsx
    //trans[i] = (10**(-(b*concentration))*100),
    float[] transmissions = new float[6];

    //ALEJANDRO The constant RGB colors of the nacl, dye, and protein beakers
    //See Frontend/virtualbiochemlab/src/Pages/Modules/Module1/Flask.common.tsx
    static string[] hexBeakers = new string[] { "#dedddd", "#604110", "#cccecd" };
    //See Frontend/virtualbiochemlab/src/Pages/Modules/Module1/steps1/Module1Step3.tsx
    //static string[] hexBeakers = new string[] { "#dedddd", "#A0522D", "#787878" };
    //The hexBeakers converted into Unity colors
    Color[] colorBeakers = new Color[3];
    

    //The final protein concentrations of each tube.
    //See Frontend/virtualbiochemlab/src/Pages/Modules/Module1/Module1.tsx
    float[] fpC = new float[6];
    //Y intercept of the graph. Calculated in calculateMysteryConcentrate()
    static float b = -1;// = Random.Range(0, 30)+70;
    //The coords of the tube's points on the linear regression graph. Stored as [y][x]. y=Absorbance, x=Protein Concentration Ex: knownPoints[0][0] is tube 0's absorbance and protein concentration
    //float[][] knownPoints = new float[6][6];

    public List<GameObject> tubes = new List<GameObject>();

    private enum canvases
    {
        Tube0,
        Tube1,
        Tube2,
        Tube3,
        Tube4,
        Tube5,
        Pipette1000,
        BeakerNaCl,
        BeakerDye,
        BeakerProtein,
        Spectrometer,
        SpectrometerLeftKnob,
        SpectrometerRightKnob,
        Glovebox,
        Notebook,
    };

    //Record list of references to objective canvases so we can still reactivate them when inactive
    /*
     * Indices:
     * 0-5: Tubes
     * 6: Pipette1000
     * 7-9: Beakers
     * 10: Spectrometer
     * 11: Spectrometer left knob
     * 12: Spectrometer right knob
     * 13: Glovebox
     */
    private List<Canvas> objectives = new List<Canvas>();

    public int curTask;
    public int curSubTask;
    public int tasks;

    // Start is called before the first frame update
    void Start()
    {
        pickUpScript = player.GetComponent<PickUp>();
        spectrometerScript = spectrometer.GetComponent<Spectrometer>();
        spectrometerLeftKnob = spectrometer.transform.Find("KnobLeft").gameObject;
        spectrometerRightKnob = spectrometer.transform.Find("KnobRight").gameObject;
        notebookScript = player.GetComponent<Notebook>();

        Transform[] gameObjs = labObjs.GetComponentsInChildren<Transform>();
        foreach (Transform g in gameObjs)
        {
            if (g.name.Contains("Flask")) tubes.Add(g.gameObject);
        }

        //Calculate answers
        //b = Random.Range(0, 30) + 70;
        b = 98;
        for (int i = 0; i < fpM.Length; i++)
        {
            //((parseFloat(fpM[i])*100 * .05) / 5).toFixed(3)
            fpC[i] = (float)((fpM[i] * 100f * 0.05f) / 5f);
            //Debug.Log("fpC[" + i + "]: " + fpC[i]);


            absorbances[i] = (float)b * fpC[i];

            transmissions[i] = (float)(Mathf.Pow(10f, (-(b * fpC[i]))) * 100f);// (10 * *(-(b * concentration)) * 100),

            Debug.Log("tubes[" + i + "]: abs:" + absorbances[i] + " trans:" + transmissions[i]);
        }

        /*
         * Objective markers start off activated. 
         * Call deactivateObjCanvas() to deactive them and get references to them for latter. 
         * Needed since inactive objects cannot be gotten.
         */
        int j = 0;
        foreach (GameObject g in tubes)
        {
            objectives.Add(deactivateObjCanvas(g));
            Tube tubeScript = g.GetComponent<Tube>();
            tubeScript.setTransmission(transmissions[j]);
            j++;
        }
        objectives.Add(deactivateObjCanvas(pipette));
        objectives.Add(deactivateObjCanvas(beakerNaCl));
        objectives.Add(deactivateObjCanvas(beakerDye));
        objectives.Add(deactivateObjCanvas(beakerProtein));
        objectives.Add(deactivateObjCanvas(spectrometer));
        objectives.Add(deactivateObjCanvas(spectrometerLeftKnob));
        objectives.Add(deactivateObjCanvas(spectrometerRightKnob));
        objectives.Add(deactivateObjCanvas(gloveBox));

        activateObjCanvas(objectives.ToArray()[(int)canvases.Glovebox]);

        //Set beaker colors
        for (int i = 0; i < colorBeakers.Length; i++)
        {
            colorBeakers[i] = new Color();
            Color newCol;
            if (ColorUtility.TryParseHtmlString(hexBeakers[i], out newCol)) colorBeakers[i] = newCol;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //See TaskSystem.txt for more info on tasks
        switch (curTask)
        {
            //Put gloves on
            case 0:
                //ImmobileClick script only used to store a boolean for if it was clicked or not
                immobileClickScript = gloveBox.GetComponent<ImmobileClick>();
                if (immobileClickScript == null) break;

                //Advance to next task
                if (immobileClickScript.clicked)
                {
                    deactivateObjCanvas(gloveBox);

                    activateObjCanvas(objectives.ToArray()[(int)canvases.Tube0]);
                    curTask++;
                }
                break;

            case 1:
                switch (curSubTask)
                {
                    //Label tube 0
                    case 0:
                        if (pickUpScript.isHolding("Flask_0"))
                        {
                            GameObject tube = tubes.ToArray()[0];

                            //Get tube display canvas
                            Transform tubeCanvas = tube.transform.GetChild(1);
                            Text[] texts = tubeCanvas.GetChild(0).GetComponentsInChildren<Text>();
                            Text tubeCanvasTxt = texts[0];

                            //If label has been submitted, move to next task
                            if (tubeCanvasTxt.text.Equals("1"))//!tubeCanvasTxt.text.Equals("New Text") && !tubeCanvasTxt.text.Equals(""))
                            {
                                deactivateObjCanvas(tube);
                                int i = 1;
                                foreach (GameObject g in tubes)
                                {
                                    if (i > 1)
                                    {
                                        //Get tube display canvas
                                        tubeCanvas = g.transform.GetChild(1);
                                        texts = tubeCanvas.GetChild(0).GetComponentsInChildren<Text>();
                                        texts[0].text = i + "";

                                        GameObject canvas = g.transform.Find("Canvas").gameObject;
                                        if (canvas != null) canvas.SetActive(true);
                                    }
                                    i++;
                                }

                                curSubTask++;
                            }
                            
                        }
                        break;

                    case 1:
                        //Check that final protein concentrations have been entered in book
                        curTask++;
                        curSubTask = 0;
                        activateObjCanvas(objectives.ToArray()[(int)canvases.Pipette1000]);
                        break;
                }
                break;

            case 2:
                switch (curSubTask)
                {
                    //Pick up pipette 1000
                    case 0:
                        if (pickUpScript.isHolding("Pipette1000"))
                        {
                            deactivateObjCanvas(pipette);
                            //Advance to next task
                            curSubTask++;
                        }
                        break;
                    //Scroll to set vol to 1000
                    case 1:
                        if (pickUpScript.isHolding("Pipette1000"))
                        {
                            PipetteScript pipetteScript = pipette.transform.GetComponent<PipetteScript>();
                            int vol = pipetteScript.volume;

                            //Advance to next task
                            if (vol == 1000)
                            {
                                curTask++;
                                curSubTask = 0;
                                activateObjCanvas(objectives.ToArray()[(int)canvases.BeakerNaCl]);

                                //Override pickUp.holding
                                pickUpScript.holdingOverride = true;
                            }
                        }
                        break;
                }
                break;

            case 3:
                switch (curSubTask)
                {
                    case 0:
                        if(doImmobileTask(beakerNaCl, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            tubes[1].tag = "Immobile";

                            Debug.Log("NaCl clicked");
                            curSubTask++;
                        }
                        break;

                    case 1:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;
                            
                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 2:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 3:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 4:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 5:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 6:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 7:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 8:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 9:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerProtein))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 10:
                        if (doImmobileTask(beakerProtein, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Protein clicked");
                            curSubTask++;
                        }
                        break;

                    case 11:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.SpectrometerRightKnob))
                        {
                            //Unoverride pickUp.holding
                            pickUpScript.holdingOverride = false;

                            tubes[1].tag = "Interactable";

                            Debug.Log("Tube 1 clicked");
                            curSubTask = 0;
                            curTask++;
                        }
                        break;


                }
                break;

            case 4:
                switch (curSubTask)
                {
                    //Shake tubes
                    case 0:
                        activateObjCanvas(objectives.ToArray()[12]);

                        //Calculate mysteryConcentrate

                        Debug.Log("Tubes shaken");
                        curSubTask = 0;
                        curTask++;
                        break;
                }
                break;

            case 5:
                switch (curSubTask)
                {
                    //Set wavelength to 595 nm
                    case 0:
                        if(doImmobileTask(spectrometerRightKnob, "", (int)canvases.Tube0))
                        {
                            Debug.Log("Spectrometer wavelength set to 595");
                            curSubTask++;
                        }
                        break;
                    case 1:
                        if(doInteractableTask(tubes[0], "", (int)canvases.Spectrometer))
                        {
                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;
                    case 2:
                        if (doImmobileTask(spectrometer, "Flask_0", (int)canvases.SpectrometerLeftKnob))
                        {
                            Debug.Log("Spectrometer clicked");
                            curSubTask++;
                        }
                        break;
                    case 3:
                        if (doImmobileTask(spectrometerLeftKnob, "", (int)canvases.Tube1))
                        {
                            Debug.Log("Spectrometer left knob clicked");
                            curSubTask++;
                        }
                        break;
                    case 4:
                        //TODO: Check notebook script to see if notebook absorbance values are all submitted and correct
                        if (doNotebookTask("absorbance"))
                        {
                            Debug.Log("Absorbances correct");
                            curSubTask++;

                        }
                        break;
                    case 5:
                        //TODO: Click next button in notebook, which plots points on graph
                        if (doNotebookTask("graph"))
                        {
                            Debug.Log("Graph plotted");
                            curSubTask++;

                        }
                        break;
                    case 6:
                        //TODO: Click next button in notebook, which plots linear regression line on points on graph
                        if (doNotebookTask("regression"))
                        {
                            Debug.Log("Linear regression plotted");
                            curSubTask++;

                        }
                        break;
                    case 7:
                        //TODO: Click next button in notebook, generates the line equation for the graph
                        if (doNotebookTask("eq"))
                        {
                            Debug.Log("Line eq generated");
                            curSubTask = 0;
                            curTask++;

                            //Reset tubes
                            for (int i = 2; i < tubes.Count; i++)
                            {
                                Destroy(tubes[i]);
                            }
                            tubes.RemoveRange(1, tubes.Count - 2);
                            //TODO: Reset tubes 0 and 1
                        }
                        break;
                }
                break;

            case 6:
                switch (curSubTask)
                {
                    case 0:
                        if (pickUpScript.isHolding("Flask_0"))
                        {
                            GameObject tube = tubes.ToArray()[0];

                            //Get tube display canvas
                            Transform tubeCanvas = tube.transform.GetChild(1);
                            Text[] texts = tubeCanvas.GetChild(0).GetComponentsInChildren<Text>();
                            Text tubeCanvasTxt = texts[0];

                            //If label has been submitted, move to next task
                            if (tubeCanvasTxt.text.Equals("1"))//!tubeCanvasTxt.text.Equals("New Text") && !tubeCanvasTxt.text.Equals(""))
                            {
                                deactivateObjCanvas(tube);
                                int i = 1;
                                foreach (GameObject g in tubes)
                                {
                                    if (i > 1)
                                    {
                                        //Get tube display canvas
                                        tubeCanvas = g.transform.GetChild(1);
                                        texts = tubeCanvas.GetChild(0).GetComponentsInChildren<Text>();
                                        texts[0].text = i + "";

                                        GameObject canvas = g.transform.Find("Canvas").gameObject;
                                        if (canvas != null) canvas.SetActive(true);
                                    }
                                    i++;
                                }

                                activateObjCanvas(objectives.ToArray()[(int)canvases.Pipette1000]);
                                curSubTask++;
                            }

                        }
                        break;
                    case 1:
                        if (doImmobileTask(beakerNaCl, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            tubes[1].tag = "Immobile";

                            Debug.Log("NaCl clicked");
                            curSubTask++;
                        }
                        break;

                    case 2:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 3:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 4:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 5:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 6:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 7:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 8:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerDye))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 9:
                        if (doImmobileTask(beakerDye, "Pipette1000", (int)canvases.Tube1))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Dye clicked");
                            curSubTask++;
                        }
                        break;

                    case 10:
                        if (doImmobileTask(tubes[1], "Pipette1000", (int)canvases.BeakerProtein))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = true;

                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;

                    case 11:
                        if (doImmobileTask(beakerProtein, "Pipette1000", (int)canvases.Tube0))
                        {
                            //Override pickUp.holding
                            pickUpScript.holdingOverride = false;

                            Debug.Log("Protein clicked");
                            curSubTask++;
                        }
                        break;

                    case 12:
                        if (doInteractableTask(tubes[0], "", (int)canvases.Spectrometer))
                        {
                            Debug.Log("Tube 1 clicked");
                            curSubTask++;
                        }
                        break;
                    case 13:
                        if (doImmobileTask(spectrometer, "Flask_0", (int)canvases.SpectrometerLeftKnob))
                        {
                            Debug.Log("Spectrometer clicked");
                            curSubTask++;
                        }
                        break;
                    case 14:
                        if (doImmobileTask(spectrometerLeftKnob, "", (int)canvases.Tube1))
                        {
                            Debug.Log("Spectrometer right knob clicked");
                            curSubTask++;
                        }
                        break;
                    case 15:
                        //TODO: Check notebook script to see if notebook absorbance values are correct
                        if (doNotebookTask("absorbance"))
                        {
                            Debug.Log("Absorbances correct");
                            curSubTask++;
                        }
                        break;
                    case 16:
                        //TODO: Check that correct unknown protein concentrations have been entered into book
                        if (doNotebookTask("unknown protein concentrations"))
                        {
                            Debug.Log("Unknown protein concentrations correct");
                            curSubTask++;
                        }
                        break;
                    case 17:
                        //TODO: Check that correct unknown protein concentration for beaker has been entered into book
                        if (doNotebookTask("unknown protien concentration beaker"))
                        {
                            Debug.Log("Unknown protein beaker concentrations correct");
                            curSubTask++;
                        }
                        break;
                    case 18:
                        //TODO: Display text that lab is complete
                        break;
                }
                break;
        }
    }

    /*
     * ALEJANDRO
     * Called when clicking on tube with pipette after clicking on substance with pipette.
     * May need to rewrite this
     */
    public void fillTube(GameObject tube, string substance)
    {
        Tube tubeScript = tube.GetComponent<Tube>();
        if (tubeScript == null) return;
        switch (substance)
        {
            case "BeakerNaCl":
                tubeScript.fill(colorBeakers[0], tubeFillHeight);
                break;
            case "BeakerDye":
                tubeScript.fill(colorBeakers[1], tubeFillHeight);
                break;
            case "BeakerProtein":
                tubeScript.fill(colorBeakers[2], tubeFillHeight);
                break;
        }
        Debug.Log("fillTube:" + tube.name + " " + substance);
    }

    /*
     * ALEJANDRO
     * Fills tubes 0 and 2 through 5. (Called after player manually fills tube 1).
     * Each tube has different amounts of each, except the dye. So its going to change the color somehow.
     * Use naclM, spM, and fpM to calculate the color somehow.
     */
    public void fillTubes()
    {
        
    }

    /*
     * Calculates mystery concentrate and updates naclM, spM, and fpM
     */
    public void calculateMysteryConcentrate()
    {
        //Random rand = new Random();
        //float mysteryConcentrate = Math.random() * (.008 - .002) + .002;
        this.mysteryConcentrate = Random.Range(0.002f, 0.008f);//Random.Next() * (.008 - .002) + .002;
        Debug.Log("mysteryConcentrate calculated:" + this.mysteryConcentrate);

        naclM = new float[] { 1f, 0.5f };
        spM = new float[] { 1f, 0.5f };
        fpM = new float[] { 1f, mysteryConcentrate };

        //TODO: Reset tubes (Locations and colors)
    }

    /*
    * Gets object held by player
    */
    GameObject[] getItemsHeld()
    {
        return pickUpScript.items;
    }

    /*
    * Finds the objective marker for given GameObject
    * Return: Canvas that was deactivated
    */
    Canvas getObjCanvas(GameObject g)
    {
        Canvas[] objs = g.GetComponentsInChildren<Canvas>();
        foreach (Canvas c in objs)
        {
            if (c.name.Equals("ObjCanvas"))
            {
                return c;
            }
        }
        return null;
    }

    /*
    * 
    * Finds and deactivates the objective marker for given GameObject
    * Return: Canvas that was deactivated
    */
    Canvas deactivateObjCanvas(GameObject g)
    {
        Canvas[] objs = g.GetComponentsInChildren<Canvas>();
        foreach (Canvas c in objs)
        {
            if (c.name.Equals("ObjCanvas"))
            {
                c.gameObject.SetActive(false);
                return c;
            }
        }
        return null;
    }

    /*
     * 
     * Activates the given canvas
     */
    void activateObjCanvas(Canvas c)
    {
        Debug.Log("ActivateObjCanvas");
        c.gameObject.SetActive(true);
        item = c;
    }

    /*
     * 
     * Checks if obj was clicked, and if otherHeld is being held, then resets its value for clicked, deactivates its obj marker, and activates the next obj marker.
     * 
     * obj: Immobile object to be checked
     * otherHeld: Name of object that should be in other hand. "" if no such requirement.
     * nextObj: Index of next obj marker
     */
    public bool doImmobileTask(GameObject obj, string otherHeld, int nextObj)
    {
        immobileClickScript = obj.GetComponent<ImmobileClick>();
        if (immobileClickScript == null) return false;
        if (!otherHeld.Equals(""))
        {
            if (immobileClickScript.clicked && pickUpScript.isHolding(otherHeld))
            {
                immobileClickScript.clicked = false;

                if (obj.name.Contains("Flask_"))
                {
                    fillTube(obj, lastBeaker);
                }
                else if (obj.name.Contains("Beaker"))
                {
                    lastBeaker = obj.name;
                }

                deactivateObjCanvas(obj);
                activateObjCanvas(objectives.ToArray()[nextObj]);
                return true;
            }

            else if (!pickUpScript.isHolding(otherHeld))
            {
                Debug.Log("Immoble task: " + obj.name + "!holding:" + otherHeld);
            }
        } 
        else if (immobileClickScript.clicked)
        {
            Debug.Log("HERE");
            immobileClickScript.clicked = false;
            if (obj.name == "KnobLeft") spectrometerScript.zeroValue();
            else if (obj.name == "KnobRight") spectrometerScript.wavelength = 595;//spectrometerScript.scrollKnob(spectrometerRightKnob);

            deactivateObjCanvas(obj);
            activateObjCanvas(objectives.ToArray()[nextObj]);
            return true;
        }
        
        return false;
    }

    /*
     * 
     * Checks if obj is being held, and if otherHeld is being held, then resets its value for clicked, deactivates its obj marker, and activates the next obj marker.
     * 
     * obj: Immobile object to be checked
     * otherHeld: Name of object that should be in other hand. "" if no such requirement.
     * nextObj: Index of next obj marker
     */
    public bool doInteractableTask(GameObject obj, string otherHeld, int nextObj)
    {
        if (!otherHeld.Equals(""))
        {
            if (pickUpScript.isHolding(otherHeld) && pickUpScript.isHolding(obj.name))
            {
                Debug.Log(obj.name);

                deactivateObjCanvas(obj);
                activateObjCanvas(objectives.ToArray()[nextObj]);
                return true;
            }
        }
        else if (pickUpScript.isHolding(obj.name))
        {
            Debug.Log(obj.name);

            deactivateObjCanvas(obj);
            activateObjCanvas(objectives.ToArray()[nextObj]);
            return true;
        }
        return false;
    }

    /*
     * NOAH
     * Checks if the notebook button with the given name has been clicked.
     */
    public bool doNotebookTask(string button)
    {
        //if( check notebook script to see if button has been successfully clicked ) return true, else return false
        if (button.Equals("Final Protein Concentration"))
        {
            //TODO: Call submitProteinConcentration() from Notebook.cs
            return notebookScript.submitProteinConcentration();
        } 
        else if (button.Equals("absorbance"))
        {
            return notebookScript.submitAbsorbance();
        }

        return true;
    }

    /*
     * Sets up the given task by deactivating the current obj marker and activating the proper obj marker
     */
    public void setUpTask(int task)
    {
        Debug.Log("setUpTask:" + task);
        curTask = task;
        curSubTask = 0;
        item.gameObject.SetActive(false);

        switch (curTask)
        {
            case 0:
                activateObjCanvas(objectives.ToArray()[(int)canvases.Glovebox]);
                break;
            case 1:
                activateObjCanvas(objectives.ToArray()[(int)canvases.Tube0]);
                break;
            case 2:
                activateObjCanvas(objectives.ToArray()[(int)canvases.Pipette1000]);
                break;
            case 3:
                activateObjCanvas(objectives.ToArray()[(int)canvases.BeakerNaCl]);
                break;
            case 4:
                //
                break;
            case 5:
                calculateMysteryConcentrate();
                activateObjCanvas(objectives.ToArray()[(int)canvases.SpectrometerRightKnob]);
                break;
            case 6:
                activateObjCanvas(objectives.ToArray()[(int)canvases.Tube0]);
                break;
        }
    }
}