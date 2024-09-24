using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Xml.Serialization;
using UnityEngine.Animations;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.Mathematics;

public class movement : MonoBehaviour
{

    //cam
    public Transform playerCam;
    public Transform orientation;

    //random
    private Rigidbody rb;

    //rotation and looking
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //movement
    public float moveSpeed = 1000;
    public float sprintspeed = 1500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    public float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //crouching
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 PlayerScale;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //input
    public float x,y;
    public bool jumping, sprinting, crouching;

    private Vector3 normalVector = Vector3.up;

    void Awake() 
    {
        //get rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // get players scale and set cursor to invisible
        PlayerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() 
    {
        //call movement script
        Movement();
    }

    private void Update()
    {
        MyInput();
        Look();
    }

    //collecting user inputs 
    private void MyInput()
    {
        //inputs
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        sprinting = Input.GetKey(KeyCode.LeftShift);
        crouching = Input.GetKey(KeyCode.LeftControl);

        //crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
        StopCrouch();
    }

    private void StartCrouch()
    {
        //START CROUCHING
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

    }

    private void StopCrouch()
    {
        //RETURNING TO NON CROUCHED 
        transform.localScale = PlayerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5F, transform.position.z);
    }

    private void Movement()
    {
        // EXTRA GRAVITY
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //FINDING VELOCITRY RELATIVE TO WHERE YOU LOOKING
         Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //COUNTER MOVMENT AND SLOP MOVEMENT
        CounterMovement(x, y, mag);

        //IF HOLD JUMP AND JUMP READY THEN JUMP
        if(readyToJump && jumping) Jump();

        //sprinting
        if (grounded && sprinting)
        {
            moveSpeed = sprintspeed;
        }
        else
            {
                moveSpeed = 1000;
            }

        float maxSpeed = this.maxSpeed;
        // IF MOVEMENT MORE THAN MAXSPEED, CANCEL INPUT SO YOU CANT GO OVER MAXSPEED
       if (x > 0 && xMag > maxSpeed) x = 0;
       if (x < 0 && xMag < -maxSpeed) x = 0;
       if (x > 0 && yMag > maxSpeed) x = 0;
       if (x < 0 && yMag < -maxSpeed) x = 0;

       float multiplier = 1f, multiplierV = 1f;

        // AIR MOVEMENT
       if (!grounded)
       {
        multiplier = 0.5f;
        multiplierV = 0.5f;
       }

       //forces to actually move the player
       {
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier );
       }

    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add the jump force
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * .5f);

            Vector3 vel = rb.linearVelocity;
            if (rb.linearVelocity.y < 0.5f)
             rb.linearVelocity = new Vector3(vel.x,0, vel.z);
            else if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);


        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;

    private void Look() 
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //find look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, make sure no over or under rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f,90f);

        //perform rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if(!grounded || jumping) return;

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.linearVelocity.x, 2) + Mathf.Pow(rb.linearVelocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.linearVelocity.y;
            Vector3 n = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(n.x, fallspeed, n.z);
        }

    }

        public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.linearVelocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;


    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

    float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }




 









    

}
