using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 6f; //change to private when finalised
    public float jumpingPower = 8f; //change to private when finalised
    private bool isFacingRight = true;

    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private bool needStand;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform roofCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else 
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            bc2d.size = new Vector2(bc2d.size.x, 0.25f);
        }
        if (Input.GetButtonUp("Crouch") && !IsRoof())
        {
            bc2d.size = new Vector2(bc2d.size.x, 1.46f);
        } 
        else 
        {
            needStand = true;
        }
        if (needStand == true && !IsRoof() && !Input.GetButton("Crouch"))
        {
            bc2d.size = new Vector2(bc2d.size.x, 1.46f);
        }

        Flip();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsRoof()
    {
        return Physics2D.OverlapCircle(roofCheck.position, 0.2f, groundLayer);
    }

}
