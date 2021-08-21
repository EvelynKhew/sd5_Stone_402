using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //Traverse up hierarchy to gameManager
        Transform gameManager = transform.parent;
        while(gameManager.parent != null)
        {
            gameManager = gameManager.parent;
        }

        player = gameManager.Find("Player");

        //EventCamera eventCam = GetComponent<EventCamera>();
        //eventCam = camera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
