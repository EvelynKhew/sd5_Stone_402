using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform player;
    public bool[] holding;
    public GameObject[] items;
    public static float reachDistance = 4f;
    //Objects are in Lab Objects gameobject when not being held
    public Transform labObjs;

    public bool holdingOverride = false;
    public bool isWatching = false;

    enum Hand
    {
        Left,
        Right
    };

    // Start is called before the first frame update
    void Start()
    {
        holding = new bool[2];
        items = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        //Left click to pick up/drop
        if (Input.GetButtonDown("Fire2"))
        {
            if (holding[(int)Hand.Left] && !holdingOverride)
            {
                DropItem(Hand.Left);
            }
            else
            {
                AttemptPickUp(Hand.Left);
            }
        }
        //Right click to interact
        else if (Input.GetButtonDown("Fire1"))
        {
            AttemptInteract(Hand.Left);
        }
    }

    void AttemptPickUp(Hand hand)
    {
        RaycastHit hit;
        Ray direction = new Ray(player.position, player.forward);
        if (!Physics.Raycast(direction, out hit, reachDistance))
        {
            return;
        }
        // Debug.DrawRay(player.position, player.forward * reachDistance, Color.red, 2, true);
        string tag = hit.collider.tag;
        if (tag == "Interactable")
        {
            GameObject item = hit.collider.gameObject;
            item.transform.SetParent(player);

            item.gameObject.transform.position = player.position + Vector3.Normalize(player.forward + player.up * -0.3f) * 0.7f;
            item.gameObject.transform.rotation = player.rotation;
            if (item.name.Contains("Pipette")) item.gameObject.transform.rotation *= Quaternion.Euler(0, -90, 0);

            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<Rigidbody>().useGravity = false;

            //Set HUD of held object active
            GameObject canvas = item.transform.Find("Canvas").gameObject;
            if (canvas != null) canvas.SetActive(true);


            holding[(int)hand] = true;
            items[(int)hand] = item;
        }
    }

    void AttemptInteract(Hand hand)
    {
        RaycastHit hit;
        Ray direction = new Ray(player.position, player.forward);
        if (!Physics.Raycast(direction, out hit, reachDistance))
        {
            return;
        }
        // Debug.DrawRay(player.position, player.forward * reachDistance, Color.red, 2, true);
        string tag = hit.collider.tag;
        //Debug.Log("AttemptInteract: " + tag);
        if (tag == "Immobile")
        {
            GameObject immobile = hit.collider.gameObject;
            Debug.Log("Immobile clicked:" + immobile.name);
            ImmobileClick script = immobile.transform.GetComponent<ImmobileClick>();
            if (script != null)
            {
                script.clicked = true;
                if (immobile.name == "Spectrometer")
                {
                    //Debug.Log("Spectrometer clicked");
                    Spectrometer spectrometerScript = immobile.transform.GetComponent<Spectrometer>();
                    if (holding[(int)Hand.Left])
                    {
                        //Prevent dropping held obj since we just loaded spectrometer (tube is still held by player, but invisible)
                        Debug.Log("holding null");
                        GameObject item = items[(int)Hand.Left];
                        DropItem(Hand.Left);
                        spectrometerScript.interact(item);
                    }
                    else
                    {
                        //Allow dropping again since we just unloaded spectrometer (tube held by player is visible again)
                        GameObject item = spectrometerScript.interact(null);
                        if (item != null)
                        {
                            Debug.Log("Pick up item " + item.name);
                            item.transform.SetParent(player);

                            item.gameObject.transform.position = player.position + Vector3.Normalize(player.forward + player.up * -0.3f) * 0.7f;
                            item.gameObject.transform.rotation = player.rotation;
                            if(item.name.Contains("Pipette")) item.gameObject.transform.rotation *= Quaternion.Euler(0, -90, 0);

                            item.GetComponent<Rigidbody>().isKinematic = true;
                            item.GetComponent<Rigidbody>().useGravity = false;

                            //Set HUD of held object active
                            GameObject canvas = item.transform.Find("Canvas").gameObject;
                            if (canvas != null) canvas.SetActive(true);


                            holding[(int)hand] = true;
                            items[(int)hand] = item;
                        }
                    }
                    //script.clicked = false;
                }
                else if (immobile.name == "KnobRight")
                {
                    //Debug.Log("Spectrometer clicked");
                    Spectrometer spectrometerScript = immobile.transform.parent.GetComponent<Spectrometer>();
                    spectrometerScript.scrollKnob();
                    //script.clicked = false;
                }
                else if (immobile.name == "KnobLeft")
                {
                    //Debug.Log("Spectrometer clicked");
                    Spectrometer spectrometerScript = immobile.transform.parent.GetComponent<Spectrometer>();
                    spectrometerScript.zeroValue();
                    //script.clicked = false;
                }
                else if (immobile.name == "SpectrometerDisplayCanvas")
                {
                    //Debug.Log("Spectrometer display clicked");
                    Spectrometer spectrometerScript = immobile.transform.parent.GetComponent<Spectrometer>();
                    spectrometerScript.watchDisplay();
                    //script.clicked = false;
                }
            }
        }
    }

    void DropItem(Hand hand)
    {
        GameObject item = items[(int)hand];
        //Set HUD of held object inactive
        //Has to be done before removing parent
        //GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");
        GameObject canvas = item.transform.Find("Canvas").gameObject;

        //Test tube labels should stay active
        if (canvas != null && !item.name.Contains("Flask")) canvas.SetActive(false);

        //Had to change to remove specific child since it was removing the UI canvas
        item.transform.SetParent(labObjs);
        item.gameObject.transform.rotation = Quaternion.identity;

        item.GetComponent<Rigidbody>().isKinematic = false;
        item.GetComponent<Rigidbody>().useGravity = true;


        holding[(int)hand] = false;

        //Set HUD of held object inactive
        GameObject[] UIObjects;
        UIObjects = GameObject.FindGameObjectsWithTag("UI");
    }

    public int amountHeld()
    {
        if (holding[0] && holding[1]) return 2;
        if (holding[0] || holding[1]) return 1;
        return 0;
    }

    public bool isHolding(string name)
    {
        if (amountHeld() == 0) return false;
        for (int i = 0; i < 2; i++)
        {
            if (holding[i]) if (items[i].name.Equals(name)) return true;
        }
        return false;
    }

}
