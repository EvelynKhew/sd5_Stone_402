using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class Notebook : MonoBehaviour
{
    //Objects for the Final Protein Concentration Table
    public Button proteinSubmit;
    public GameObject FPCSubmitTable;
    public InputField proteinConcentrationTube1;
    public InputField proteinConcentrationTube2;
    public InputField proteinConcentrationTube3;
    public InputField proteinConcentrationTube4;
    public InputField proteinConcentrationTube5;
    public InputField proteinConcentrationTube6;
    private bool FPCCorrect;

    //Objects for the Absorbance Reading Table
    public Button absorbanceSubmit;
    public GameObject absorbanceTable;
    public InputField absorbanceTube1;
    public InputField absorbanceTube2;
    public InputField absorbanceTube3;
    public InputField absorbanceTube4;
    public InputField absorbanceTube5;
    public InputField absorbanceTube6;
    private bool absorbanceCorrect;

    //Objects for the LastAbsorbanceTable
    public Button LastAbsorbanceSubmit;
    public GameObject LastAbsorbanceTable;
    public InputField LATTube1;
    public InputField LATTube2;
    private bool lastAbsorbanceCorrect;

    //Objects for the LastAbsorbanceTable2
    public Button LastAbsorbanceSubmit2;
    public GameObject LastAbsorbanceTable2;
    public InputField proteinConcentration;
    public Text absorbance1; //needs to be the correct input for LATTube1
    public Text absorbance2; //needs to be the correct input for LATTube2
    private bool lastAbsorbanceCorrect2;

    //Objects for the LastAbsorbanceTable3
    public Button LastAbsorbanceSubmit3;
    public GameObject LastAbsorbanceTable3;
    public InputField MPSC;
    public Text proteinConcentrationHolder; //needs to be the correct input for proteinConcentration
    public Text LAT3absorbance1; //needs to be the correct input for LATTube1
    public Text LAT3absorbance2; //needs to be the correct input for LATTube2
    private bool lastAbsorbanceCorrect3;

    public GameObject EventSystem;
    private EventSystem eventSystemScript;

    public GameObject endScreen;
    

    // Start is called before the first frame update
    void Start()
    {
        eventSystemScript = EventSystem.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check the curTask and CurSubTask in EventSystem to show which table will be shown.
        if (eventSystemScript.curTask == 1 && eventSystemScript.curSubTask == 1)
        {
            FPCSubmitTable.SetActive(true);
           // foo = true;
        } 
        else if (eventSystemScript.curTask == 5 && eventSystemScript.curSubTask == 3)
        {
            //DEACTIVATE THE TABLES IN THE TASK RIGHT BEFORE THE NEXT TIME YOU NEED A TABLE. 
            FPCSubmitTable.SetActive(false);
        } 
        else if (eventSystemScript.curTask == 5 && eventSystemScript.curSubTask == 4)
        {
            absorbanceTable.SetActive(true);
        } 
        else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 14)
        {
            absorbanceTable.SetActive(false);
        } 
        else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 16)
        {
            LastAbsorbanceTable.SetActive(true);
        } 
        else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 17)
        {
            LastAbsorbanceTable.SetActive(false);
            absorbance1.text = LATTube1.text;
            absorbance2.text = LATTube2.text;
            LastAbsorbanceTable2.SetActive(true);
        } 
        else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 18)
        {
            LastAbsorbanceTable2.SetActive(false);
            LAT3absorbance1.text = LATTube1.text;
            LAT3absorbance2.text = LATTube2.text;
            proteinConcentrationHolder.text = proteinConcentration.text;
            LastAbsorbanceTable3.SetActive(true);
        }
        else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 19)
        {
            LastAbsorbanceTable3.SetActive(false);
            endScreen.SetActive(true);
        }
    }


    // Checks whether the answers submitted in the FinalProteinConcentrationTable are correct
    public void submitProteinConcentration()
    {
        string tube1 = proteinConcentrationTube1.text;
        string tube2 = proteinConcentrationTube2.text; 
        string tube3 = proteinConcentrationTube3.text;
        string tube4 = proteinConcentrationTube4.text;
        string tube5 = proteinConcentrationTube5.text;
        string tube6 = proteinConcentrationTube6.text;
        bool allCorrect = true;

        if (tube1.Equals("0"))
        {
            proteinConcentrationTube1.GetComponent<Image>().color = Color.green;

        } else
        {
            proteinConcentrationTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals("0.002") || tube2.Equals(".002"))
        {
            proteinConcentrationTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            proteinConcentrationTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube3.Equals("0.004") || tube3.Equals(".004"))
        {
            proteinConcentrationTube3.GetComponent<Image>().color = Color.green;
        }
        else
        {
            proteinConcentrationTube3.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube4.Equals("0.006") || tube4.Equals(".006"))
        {
            proteinConcentrationTube4.GetComponent<Image>().color = Color.green;
        }
        else
        {
            proteinConcentrationTube4.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube5.Equals("0.008") || tube5.Equals(".008"))
        {
            proteinConcentrationTube5.GetComponent<Image>().color = Color.green;
        }
        else
        {
            proteinConcentrationTube5.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube6.Equals("0.01") || tube6.Equals(".01"))
        {
            proteinConcentrationTube6.GetComponent<Image>().color = Color.green;
        }
        else
        {
            proteinConcentrationTube6.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (allCorrect)
        {
            FPCCorrect = true;
        } else
        {
            FPCCorrect = false;
        }

    }

    public bool FPCCheck()
    {
        return FPCCorrect;
    }

    // Checks whether the answers submitted in the AbsorbanceReadingTable are correct
    public void submitAbsorbance()
    {
        float margin = 0.05f;

        string tube1 = absorbanceTube1.text;
        string tube2 = absorbanceTube2.text;
        string tube3 = absorbanceTube3.text;
        string tube4 = absorbanceTube4.text;
        string tube5 = absorbanceTube5.text;
        string tube6 = absorbanceTube6.text;

        /**
        if (tube2.Equals(""))
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
        }
        **/
        string[] tubeValues = { tube1, tube2, tube3, tube4, tube5, tube6 };
        InputField[] inputs = { absorbanceTube1, absorbanceTube2, absorbanceTube3, absorbanceTube4, absorbanceTube5, absorbanceTube6 };

        for (int i = 0; i < 6; i++)
        {
            if (tubeValues[i] == null)
            {
                inputs[i].GetComponentInChildren<Text>().text = "999";
            }
        }


        float tube1Value = float.Parse(tube1);
        float tube2Value = float.Parse(tube2);
        float tube3Value = float.Parse(tube3);
        float tube4Value = float.Parse(tube4);
        float tube5Value = float.Parse(tube5);
        float tube6Value = float.Parse(tube6);

        bool allCorrect = true;

        if (tube1Value >= eventSystemScript.absorbances[0] - margin && tube1Value <= eventSystemScript.absorbances[0] + margin)
        {
            absorbanceTube1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2Value >= eventSystemScript.absorbances[1] - margin && tube2Value <= eventSystemScript.absorbances[1] + margin)
        {
            absorbanceTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube3Value >= eventSystemScript.absorbances[2] - margin && tube3Value <= eventSystemScript.absorbances[2] + margin)
        {
            absorbanceTube3.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube3.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube4Value >= eventSystemScript.absorbances[3] - margin && tube4Value <= eventSystemScript.absorbances[3] + margin)
        {
            absorbanceTube4.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube4.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube5Value >= eventSystemScript.absorbances[4] - margin && tube5Value <= eventSystemScript.absorbances[4] + margin)
        {
            absorbanceTube5.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube5.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube6Value >= eventSystemScript.absorbances[5] - margin && tube6Value <= eventSystemScript.absorbances[5] + margin)
        {
            absorbanceTube6.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube6.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (allCorrect)
        {
            absorbanceCorrect = true;
        } else
        {
            absorbanceCorrect = false;
        }


    }

    public bool absorbanceCheck()
    {
        return absorbanceCorrect;
    }

    //TODO: Check if inputs are correct. Add margin of error?
    public void submitAbsorbance2()
    {
        string tube1 = LATTube1.text;
        string tube2 = LATTube2.text;
 
        bool allCorrect = true;

        if (tube1.Equals(eventSystemScript.absorbances[0].ToString()))
        {
            LATTube1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            LATTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals(eventSystemScript.absorbances[1].ToString()))
        {
            LATTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            LATTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }
       

        if (allCorrect)
        {
            lastAbsorbanceCorrect = true;
        }
        else
        {
            lastAbsorbanceCorrect = false;
        }
    }

    public bool absorbanceCheck2()
    {
        return lastAbsorbanceCorrect;
    }

    //TODO: Fill in placeholder values, and protein concentration
    public void submitUnknownProteinConcentration()
    {
        Debug.Log("In submitUnkownProteinConcentration");
        Debug.Log(eventSystemScript.fpM[1].ToString("#.0000"));
        string tube2 = proteinConcentration.text;
        bool allCorrect = true;

        Debug.Log("input read as: " + tube2);

        if (tube2.Equals(eventSystemScript.fpM[1].ToString("#.0000")))
        {
            proteinConcentration.GetComponent<Image>().color = Color.green;
        }
        else
        {
            Debug.Log("In submitUnkownProteinConcentration else bracket");

            proteinConcentration.GetComponent<Image>().color = Color.red;
            Debug.Log("input read as: " + tube2);
            allCorrect = false;
        }

        if (allCorrect)
        {
            lastAbsorbanceCorrect2 = true;
        }
        else
        {
            lastAbsorbanceCorrect2 = false;
        }
    }

    public bool UPCCheck()
    {
        return lastAbsorbanceCorrect2;
    }

    //TODO: Fill in placeholder values, and MPSC
    public void submitUnknownProteinConcentration2()
    {
        eventSystemScript.absorbances[0].ToString();
        string tube1 = MPSC.text;
        float answer = eventSystemScript.fpM[1] * 10;
        Debug.Log("input read as: " + tube1);


        bool allCorrect = true;


        if (tube1.Equals(answer.ToString("#.000")))
        {
            MPSC.GetComponent<Image>().color = Color.green;
            eventSystemScript.curSubTask++;
        }
        else
        {
            MPSC.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }  

        if (allCorrect)
        {
            lastAbsorbanceCorrect3 = true;
        }
        else
        {
            lastAbsorbanceCorrect3 = false;
        }
    }

    public bool UPCCheck2()
    {
        return lastAbsorbanceCorrect3;
    }


}
