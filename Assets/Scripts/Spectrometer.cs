using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spectrometer : MonoBehaviour
{
    public GameObject player;
    public GameObject dialPivot;
    public GameObject dialSprite;
    public Transform cameraPos;
    public Camera camera;
    private ImmobileClick immobileClickScript;
    private MouseLook mouseLookScript;
    private PlayerMovement playerMovementScript;
    private Transform playerCam;
    private GameObject knobLeft;
    private ImmobileClick immobileClickScriptKnobLeft;
    private GameObject knobRight;
    private ImmobileClick immobileClickScriptKnobRight;
    private GameObject wavelengthDisplay;
    private GameObject placeholder;
    private GameObject tubeHeld;
    private Vector3 camOgPos;
    private Vector3 camEndPos;

    private int clicks;
    private bool isMoving = false;
    public bool isWatching = false;
    public bool isLoaded = false;
    public bool isScrolling = false;
    private float endAngle = 0f;
    private float lastAngle = 100f;
    private float rotSpeed = 0.05f;

    public bool zeroed;
    public int wavelength = 498;
    private float value = 50;
    private float transmission;

    private Vector3 placeholderPos;
    private Vector3 loadedTubePos;
    private Transform loadedTube;

    private Vector3 loadTubeLoc = new Vector3((float)-0.9679987, (float)1.228001, (float)2.6924);

    //Dial rotates on Z axis from 51 (abs 0, trans 100) to -51 (abs inf, trans 0)
    private const float maxZ = -51;
    private const float minZ = 51;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = player.transform.Find("Camera");
        mouseLookScript = playerCam.GetComponent<MouseLook>();
        playerMovementScript = player.GetComponent<PlayerMovement>();
        knobLeft = transform.Find("KnobLeft").gameObject;
        immobileClickScriptKnobLeft = knobLeft.GetComponent<ImmobileClick>();
        knobRight = transform.Find("KnobRight").gameObject;
        immobileClickScriptKnobRight = knobRight.GetComponent<ImmobileClick>();
        //wavelengthDisplay = knobRight.transform.Find("Text").gameObject;
        Transform wavelengthCanvas = knobRight.transform.Find("WavelengthCanvas");
        Transform wavelengthPanel = wavelengthCanvas.Find("Panel");
        wavelengthDisplay = wavelengthPanel.transform.Find("Text").gameObject;
        placeholder = transform.Find("Flask_Placeholder").gameObject;
        placeholderPos = placeholder.transform.position;

        camOgPos = cameraPos.transform.position;
        camEndPos = camOgPos;
        camera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Animate dial movement
        if (isMoving)
        {
            Vector3 rot = dialPivot.transform.rotation.eulerAngles;
            //Debug.Log("Rotating: Z:" + rot.z + " to Z:" + (360 - getMeterAngle(this.value)));

            float nextZ = rot.z - rotSpeed;
            dialPivot.transform.rotation = Quaternion.Euler(rot.x, rot.y, nextZ);
            //Debug.Log("nextZ:" + nextZ);
            if ((int)(nextZ) == (int)endAngle)
            {
                Debug.Log("End angle reached:" + endAngle);
                isMoving = false;
            }
        }
        if (isScrolling)
        {
            Debug.Log("isScrolling");
            if (Input.GetButtonDown("Fire1")) wavelength = 595;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                wavelength += 1;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.RightArrow))
            {
                wavelength -= 1;
            }
            //Volume maxes out at 300
            if (wavelength > 1000) wavelength = 0;
            if (wavelength < 0) wavelength = 1000;

            string str = wavelength.ToString();
            Text wavelengthText = wavelengthDisplay.GetComponent<Text>();
            wavelengthText.text = str;
            //Debug.Log("wavelength:" + wavelength + " txt:" + wavelengthText);
        }
        if (isWatching)
        {
            cameraPos.position = Vector3.MoveTowards(cameraPos.position, camEndPos, 2f * Time.deltaTime);
            //Debug.Log("Camera move: x:" + cameraPos.position.x + " y:" + cameraPos.position.y + " z:" + cameraPos.position.z + " End x:" + camEndPos.x + " y:" + camEndPos.y + " z:" + camEndPos.z);
        }

    }

    /*
     * Either loads or unloads obj as a tube.
     * 
     * Returns true if holding is overriden for PickUp.cs or false otherwise. If true, tube is inside the Spectrometer, so player can't drop held tube.
     */
    public GameObject interact(GameObject obj)
    {
        //Debug.Log("Interact: isLoaded:" + isLoaded + " isMoving:" + isMoving + " isWatching:" + isWatching);
        if (!isLoaded)
        {
            if (obj == null || !obj.name.Contains("Flask_"))
            {
                return null;
            }
            if (!isMoving)
            {
                Debug.Log("Interact: load tube");
                loadTube(obj);
                return null;
            }
        }
        else
        {
            if (!isMoving)
            {
                GameObject tube = unloadTube();
                Debug.Log("Interact: unload tube " + tube.name);
                return tube;
            }
        }
        return null;
    }

    /*
     * Transmissions calculated in ChromatographyTube.tsx using tube's rgb value
     * See Frontend/virtualbiochemlab/src/Components/Chromatography/ChromatographyTube.tsx
     * Takes transmission of tube as input, then rotates dial accordingly if the transmission is different from the last transmission input
     * newTransmission: Transmission of tube, calculated in event system at step 5
     */
    private void render(float newTransmission)
    {
        if (newTransmission != this.value)
        {
            Debug.Log("transmission " + newTransmission + " != value " + this.value);

            updateValue(newTransmission);
            isMoving = true;

        }
        else
        {
            Debug.Log("transmission " + newTransmission + " == value " + this.value);
            isLoaded = true;
            isMoving = false;
        }
    }

    /*
     * Toggles the Spectrometer display cutscene
     */
    public void watchDisplay()
    {
        if (!isWatching)
        {
            Debug.Log("Watch cutscene");
            //Set up player to watch cutscene
            mouseLookScript.GetComponent<MouseLook>().toggleCrosshair();
            mouseLookScript.GetComponent<MouseLook>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            playerCam.GetComponent<Camera>().enabled = false;
            camera.enabled = true;
            isWatching = true;

            camOgPos = cameraPos.transform.position;
            camEndPos = camOgPos;
            cameraPos.position = playerCam.position;
            //Debug.Log("Camera move: Og x:" + camOgPos.x + " y:" + camOgPos.y + " z:" + camOgPos.z + " End x:" + camEndPos.x + " y:" + camEndPos.y + " z:" + camEndPos.z);
        }
        else
        {
            Debug.Log("Quit watching cutscene");
            //Set up player to quit watching cutscene
            mouseLookScript.GetComponent<MouseLook>().toggleCrosshair();
            mouseLookScript.GetComponent<MouseLook>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            playerCam.GetComponent<Camera>().enabled = true;
            camera.enabled = false;
            isWatching = false;

            //camEndPos = camOgPos;
            //cameraPos.position = playerCam.position;
        }
    }

    /*
     * WORKS CORRECTLY
     */
    private void updateValue(float newTransmission)
    {
        float angle = getMeterAngle(newTransmission);
        //Debug.Log("updateValue: transmission:" + newTransmission + " angle:" + angle + " ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        //Dial rotation circles around to 360 at 0 degress, so anything past must be 360 - angle
        if (angle < 0)
        {
            endAngle = 360 + angle;
            rotSpeed = 0.05f;
        }
        else
        {
            endAngle = angle;
            Debug.Log("rotSpeed:" + rotSpeed + " angle:" + getMeterAngle(this.value) + " new rotSpeed:" + rotSpeed * -1 + " new angle:" + angle);
            if (angle < getMeterAngle(this.value)) rotSpeed = 0.05f;
            else rotSpeed = -0.05f;
        }
        //Debug.Log("endAngle calulated:" + endAngle + " og angle:" + angle + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");


        string from = this.getMeterAngle(this.value) + "";
        string to = angle + "";

        //Debug.Log("From " + from + " this.value:" + this.value);
        //Debug.Log("To "+to + " newTransmission:" + newTransmission + " endAngle="+endAngle);
        this.value = newTransmission;
    }

    //51=trans100, -50=trans0
    //!!!!!!!!!
    //Angle starts to be off around trans75 (~27 degrees) and trans25 (~ -27 degrees)
    public float getMeterAngle(float transmission)
    {
        double transmissionFactor = (double)transmission / 100;
        //Debug.Log("TransmissionFactor:" + transmissionFactor + " Lerp: "+ Mathf.Lerp(-51, 51, (float)transmissionFactor));
        return Mathf.Lerp(maxZ, minZ, (float)transmissionFactor);
    }

    //-1: nothing in there, not zeroed || -2: not there, zeroed || -3: something, not zeroed
    public int calculateTransmission(int transmission, int transmissionOverride)
    {
        if (transmissionOverride != -1)
            return transmissionOverride;
        switch (transmission)
        {
            case -1:
            case -2:
                return 100;
            case -3:
                //return 100-Math.random() *10;
                return Random.Range(0, 100);
            case int i when (i >= 0 && i <= 100):
                return transmission;
            default:
                return 50;
        }
    }

    public void zeroValue()
    {
        zeroed = true;
        render(100);
    }

    /*
     * Load tube held by player by activating placeholder in Spectrometer, setting its appearance equal, then inactivating the one held by player
     * 
     */
    public void loadTube(GameObject obj)
    {
        Debug.Log("loadTube: " + obj.name);
        if (obj.name.Contains("Flask_"))
        {
            isLoaded = true;
            tubeHeld = obj;

            loadedTube = obj.transform;
            loadedTubePos = obj.transform.position;
            loadedTube.parent = transform;
            loadedTube.position = placeholderPos;

            Tube tubeScript = obj.GetComponent<Tube>();
            render(tubeScript.getTransmission());
        }
    }

    public GameObject unloadTube()
    {
        Debug.Log("unloadTube: " + tubeHeld.name);
        isLoaded = false;

        Debug.Log("unloadTube end:" + loadedTube.name);
        return loadedTube.gameObject;
    }

    /*
    * 
    * Checks if obj was clicked, and if otherHeld is being held, then resets its value for clicked, deactivates its obj marker, and activates the next obj marker.
    * 
    * obj: Immobile object to be checked
    * otherHeld: Name of object that should be in other hand. "" if no such requirement.
    * nextObj: Index of next obj marker
    */
    public void scrollKnob()
    {
        Debug.Log("ScrollKnob");
        if (!isScrolling)
        {
            mouseLookScript.GetComponent<MouseLook>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            isScrolling = true;
        }
        else
        {
            mouseLookScript.GetComponent<MouseLook>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            isScrolling = false;
        }
    }


}
