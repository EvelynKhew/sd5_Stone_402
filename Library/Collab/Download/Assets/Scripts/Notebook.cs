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
        } else if (eventSystemScript.curTask == 5 && eventSystemScript.curSubTask == 3)
        {
            //DEACTIVATE THE TABLES IN THE TASK RIGHT BEFORE THE NEXT TIME YOU NEED A TABLE. 
            //TODO Put a sleep timer here and delete all the debug.logs from the notebook testing
            FPCSubmitTable.SetActive(false);
        } else if (eventSystemScript.curTask == 5 && eventSystemScript.curSubTask == 4)
        {
            absorbanceTable.SetActive(true);
        } else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 14)
        {
            absorbanceTable.SetActive(false);
        } else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 15)
        {
            LastAbsorbanceTable.SetActive(true);
        } else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 16)
        {
            LastAbsorbanceTable.SetActive(false);
            LastAbsorbanceTable2.SetActive(true);
        } else if (eventSystemScript.curTask == 6 && eventSystemScript.curSubTask == 17)
        {
            LastAbsorbanceTable2.SetActive(false);
            LastAbsorbanceTable3.SetActive(true);
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
        eventSystemScript.absorbances[0].ToString();
        string tube1 = absorbanceTube1.text;
        string tube2 = absorbanceTube2.text;
        string tube3 = absorbanceTube3.text;
        string tube4 = absorbanceTube4.text;
        string tube5 = absorbanceTube5.text;
        string tube6 = absorbanceTube6.text;
        bool allCorrect = true;
        
        if (tube1.Equals(eventSystemScript.absorbances[0].ToString()))
        {
            absorbanceTube1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals(eventSystemScript.absorbances[1].ToString()))
        {
            absorbanceTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube3.Equals(eventSystemScript.absorbances[2].ToString()))
        {
            absorbanceTube3.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube3.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube4.Equals(eventSystemScript.absorbances[3].ToString()))
        {
            absorbanceTube4.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube4.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube5.Equals(eventSystemScript.absorbances[4].ToString()))
        {
            absorbanceTube5.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube5.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube6.Equals(eventSystemScript.absorbances[5].ToString()))
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

    //TODO: Check if inputs are correct
    public void submitAbsorbance2()
    {
        eventSystemScript.absorbances[0].ToString();
        string tube1 = absorbanceTube1.text;
        string tube2 = absorbanceTube2.text;
 
        bool allCorrect = true;

        /**

        if (tube1.Equals(eventSystemScript.absorbances[0].ToString()))
        {
            absorbanceTube1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals(eventSystemScript.absorbances[1].ToString()))
        {
            absorbanceTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }
        **/

        if (allCorrect)
        {
            absorbanceCorrect = true;
        }
        else
        {
            absorbanceCorrect = false;
        }
    }

    public bool absorbanceCheck2()
    {
        return lastAbsorbanceCorrect;
    }

    //TODO: Fill in placeholder values, and protein concentration
    public void submitUnknownProteinConcentration()
    {
        string tube2 = proteinConcentration.text;
        bool allCorrect = true;

        /**

        if (tube1.Equals(eventSystemScript.absorbances[0].ToString()))
        {
            absorbanceTube1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals(eventSystemScript.absorbances[1].ToString()))
        {
            absorbanceTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }
        **/

        if (allCorrect)
        {
            absorbanceCorrect = true;
        }
        else
        {
            absorbanceCorrect = false;
        }
    }

    public bool UPCCheck()
    {
        return false;
    }

    //TODO: Fill in placeholder values, and MPSC
    public void submitUnknownProteinConcentration2()
    {
        eventSystemScript.absorbances[0].ToString();
        string tube1 = absorbanceTube1.text;
        string tube2 = absorbanceTube2.text;

        bool allCorrect = true;

        /**

        if (tube1.Equals(eventSystemScript.absorbances[0].ToString()))
        {
            absorbanceTube1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals(eventSystemScript.absorbances[1].ToString()))
        {
            absorbanceTube2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }
        **/

        if (allCorrect)
        {
            absorbanceCorrect = true;
        }
        else
        {
            absorbanceCorrect = false;
        }
    }

    public bool UPCCheck2()
    {
        return false;
    }


}
