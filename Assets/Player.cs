using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundDetectLength;
    public float kissingDetectLength;
    public bool isGrounded = false;
    public bool isKissingWall = false;

    private Rigidbody rb;
    private bool facingRight = true;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (isGrounded)
            {
                rb.velocity = Vector3.up * jumpForce;
            }
            else
            {
                if (isKissingWall)
                {
                    //rb.AddForce((-transform.forward * jumpForce * 2) + (Vector3.up * jumpForce), ForceMode.VelocityChange);
                }
            }
        }
    }


    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        isKissingWall = CheckKissingWall();

        float mx = Input.GetAxis("Horizontal");
        if (isGrounded)
            rb.velocity = new Vector3(mx * speed, rb.velocity.y, rb.velocity.z);
        else
            rb.velocity = new Vector3((rb.velocity.x + mx * speed) / 2, rb.velocity.y, rb.velocity.z);

        UpdateFacing(mx);
    }

    private void UpdateFacing(float inputH)
    {
        // facing
        if (facingRight == false && inputH > 0)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (facingRight == true && inputH < 0)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

    private bool CheckGrounded()
    {
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hitInfo, groundDetectLength))
        {
            if (hitInfo.collider.tag == "Ground")
                return true;
        }

        return false;
    }

    private bool CheckKissingWall()
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitInfo, kissingDetectLength))
        {
            if (hitInfo.collider.tag == "Ground")
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        // Kissing
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * kissingDetectLength);

        // Ground
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDetectLength);
    }
}
