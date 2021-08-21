using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Only purpose is to record if this was clicked or not
 * Bool is changed in PickUp script and used by EventSystem script to see if this was clicked or not
 */
public class ImmobileClick : MonoBehaviour
{
    public bool clicked;
    public int clicks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
