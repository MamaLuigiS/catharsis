using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float walkSpeed = 3f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 200f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public LayerMask groundLayer = 1;

    private Rigidbody rb;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSpeed = walkSpeed;

        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMovment();
        HandleGravity();
    }
    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    void HandleMovment()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 targetVelocity = moveDirection * currentSpeed;

            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void HandleGravity()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        if (!isGrounded)
        {
            rb.velocity += new Vector3(0, gravity * Time.deltaTime, 0);
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                Invoke("ResetGrounded", 0.1f);
                break;
            }
        }
    }

    void ResetGrounded()
    {
        isGrounded = false;
    }
}
