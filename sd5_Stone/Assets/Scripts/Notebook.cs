using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Notebook : MonoBehaviour
{
    //Objects for the Final Protein Concentration Table
    public Button proteinSubmit;
    public GameObject proteinConcentrationTable;
    public InputField proteinConcentrationTube1;
    public InputField proteinConcentrationTube2;
    public InputField proteinConcentrationTube3;
    public InputField proteinConcentrationTube4;
    public InputField proteinConcentrationTube5;
    public InputField proteinConcentrationTube6;

    //Objects for the Absorbance Reading Table
    public Button absorbanceSubmit;
    public GameObject absorbanceTable;
    public InputField absorbanceTube1;
    public InputField absorbanceTube2;
    public InputField absorbanceTube3;
    public InputField absorbanceTube4;
    public InputField absorbanceTube5;
    public InputField absorbanceTube6;


    public Text correctAnswer;
    public bool test = true;

    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks whether the answers submitted in the FinalProteinConcentrationTable are correct
    public bool submitProteinConcentration()
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
            allCorrect = true;

        } else
        {
            proteinConcentrationTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals("0.002") || tube2.Equals(".002"))
        {
            proteinConcentrationTube2.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            proteinConcentrationTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube3.Equals("0.004") || tube3.Equals(".004"))
        {
            proteinConcentrationTube3.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            proteinConcentrationTube3.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube4.Equals("0.006") || tube4.Equals(".006"))
        {
            proteinConcentrationTube4.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            proteinConcentrationTube4.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube5.Equals("0.008") || tube5.Equals(".008"))
        {
            proteinConcentrationTube5.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            proteinConcentrationTube5.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube6.Equals("0.01") || tube6.Equals(".01"))
        {
            proteinConcentrationTube6.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            proteinConcentrationTube6.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        return allCorrect;

    }

    // Checks whether the answers submitted in the AbsorbanceReadingTable are correct
    public bool submitAbsorbance()
    {

        string tube1 = absorbanceTube1.text;
        string tube2 = absorbanceTube2.text;
        string tube3 = absorbanceTube3.text;
        string tube4 = absorbanceTube4.text;
        string tube5 = absorbanceTube5.text;
        string tube6 = absorbanceTube6.text;
        bool allCorrect = true;

        if (tube1.Equals("0"))
        {
            absorbanceTube1.GetComponent<Image>().color = Color.green;
            allCorrect = true;

        }
        else
        {
            absorbanceTube1.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube2.Equals("0.002") || tube2.Equals(".002"))
        {
            absorbanceTube2.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            absorbanceTube2.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube3.Equals("0.004") || tube3.Equals(".004"))
        {
            absorbanceTube3.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            absorbanceTube3.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube4.Equals("0.006") || tube4.Equals(".006"))
        {
            absorbanceTube4.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            absorbanceTube4.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube5.Equals("0.008") || tube5.Equals(".008"))
        {
            absorbanceTube5.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            absorbanceTube5.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        if (tube6.Equals("0.01") || tube6.Equals(".01"))
        {
            absorbanceTube6.GetComponent<Image>().color = Color.green;
            allCorrect = true;
        }
        else
        {
            absorbanceTube6.GetComponent<Image>().color = Color.red;
            allCorrect = false;
        }

        return allCorrect;
    }

}
