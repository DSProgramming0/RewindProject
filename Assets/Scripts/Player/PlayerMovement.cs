using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;

    public float speed;
    public float normalSpeed = 12f;
    public float sprintSpeed = 17f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;
    public float extraJumpHeight = 4.5f;
    public int extraJumps = 1;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        speed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            extraJumps = 1;
        }

        //Movement Standard
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Movement Sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed =  sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

        //If x is negative, transform.right will become negative so it will move left, etc.
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //Jumping
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //DoubleJump
        if(!isGrounded && extraJumps == 1)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpHeight * extraJumpHeight;
                extraJumps--;
            }
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
             body.velocity += hit.controller.velocity;
    }

}
