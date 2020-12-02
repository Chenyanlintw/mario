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
    private int airJumpNum = 0;
    private int airJumpMaxNum = 1;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 在地上
            if (isGrounded)
            {
                rb.velocity = Vector3.up * jumpForce;

                // 重設多段跳躍次數
                if (airJumpNum != airJumpMaxNum)
                    airJumpNum = airJumpMaxNum;
            }

            // 在空中
            else
            {
                // 多段跳躍
                if (airJumpNum > 0)
                {
                    rb.velocity = Vector3.up * jumpForce;
                    airJumpNum--;
                }
            }
        }
    }


    void FixedUpdate()
    {
        // 是否在地上
        isGrounded = CheckGrounded();

        // 是否貼著牆壁
        isKissingWall = CheckKissingWall();

        // 取得方向鍵數值
        float mx = Input.GetAxis("Horizontal");

        // 水平移動
        rb.velocity = new Vector3(mx * speed, rb.velocity.y, rb.velocity.z);

        // 調整角色面對方向
        UpdateFacing(mx);
    }


    // 調整角色面對方向
    private void UpdateFacing(float inputH)
    {
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

    // 判斷是否在地上
    private bool CheckGrounded()
    {
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hitInfo, groundDetectLength))
        {
            if (hitInfo.collider.tag == "Ground")
                return true;
        }

        return false;
    }

    // 判斷是否貼著牆
    private bool CheckKissingWall()
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitInfo, kissingDetectLength))
        {
            if (hitInfo.collider.tag == "Ground")
                return true;
        }

        return false;
    }

    // 畫出測試輔助線
    private void OnDrawGizmos()
    {
        // Kissing wall
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * kissingDetectLength);

        // Ground
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDetectLength);
    }
}
