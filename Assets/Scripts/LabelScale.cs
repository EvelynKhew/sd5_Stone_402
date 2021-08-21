using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelScale : MonoBehaviour
{
    private Transform player;
    private float angleOffset = 10;
    // Start is called before the first frame update
    void Start()
    {
        //Traverse up hierarchy to gameManager
        Transform gameManager = transform.parent;
        while (gameManager.parent != null)
        {
            gameManager = gameManager.parent;
        }

        player = gameManager.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float dist = Vector3.Distance(player.position, transform.position);
        float angle = Vector3.Angle(player.transform.forward, transform.position - player.transform.position);
        if (angle < angleOffset)
        {
            Vector3 scale = transform.localScale;
            float ratio = dist / 100;
            if (ratio > 10f) ratio = 10f;
            else if (ratio < 0) ratio = 1;
            transform.localScale = scale * ratio;

            Debug.Log("Tube label: "+gameObject.name+" Dist:" + dist + " scale:" + scale+" new scale:"+scale*ratio);
        }
        */
    }
}
