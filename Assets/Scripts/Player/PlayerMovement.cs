using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Global global;

    public float speed = 5f;
    public float sprintingSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;
    Vector3 velocity;
    bool isSprinting;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        global = GameObject.FindWithTag("Global").GetComponent<Global>();
    }

    void Update()
    {
        if(controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                isSprinting = true;
            else
                isSprinting = false;
        }

        Move();
        MoveDown();

        if(Input.GetButtonDown("Jump") && controller.isGrounded)
            Jump();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed = isSprinting ? sprintingSpeed : this.speed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void MoveDown()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
    }
}
