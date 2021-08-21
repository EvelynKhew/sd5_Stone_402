using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public static float speed = 12f;
    public static float gravity = -9.81f;
    //public static float jumpStrength = 7f;
    public static float groundDistance = 0.2f;

    public Transform groundCheck;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(!isGrounded) {
            velocity.y += gravity * Time.deltaTime;
        }

        //if(Input.GetButtonDown("Jump") && isGrounded) {
        //    velocity.y = jumpStrength;
        //}

        controller.Move(velocity * Time.deltaTime);
    }
}
