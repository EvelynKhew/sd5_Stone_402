using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    private Transform player;
    private float angleOffset = 10;
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        if (gameObject.name == "LabelCanvas")
        {
            float dist = Vector3.Distance(player.position, transform.position);
            float angle = Vector3.Angle(player.transform.forward, transform.position - player.transform.position);
            if (angle < angleOffset)
            {
                Vector3 scale = transform.localScale;
                transform.localScale = scale * dist;
            }
        }
    }
}
