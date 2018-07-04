using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private enum JumpState 
    {
        Grounded,
        Jumping,
        JumpingKeyReleased,
        DoubleJumping,
    }

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontalSpeed = 7;
    private float verticalSpeed = 14;
    private float downForce = -100;
    private bool canJump = true;
    private bool canDoubleJump = true;
    private bool jumping = false;

    private int collisionCount = 0;

    private bool requestJump = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
	
    void Update ()
    {
        bool jumpButtonDownPressed = Input.GetKeyDown(KeyCode.Space);

        requestJump = (requestJump || jumpButtonDownPressed) ? true : false;
    }

	// Rigidbody physics should be done in FixedUpdate
	void FixedUpdate () {
        print("RequestJump: " + requestJump + " CanJump: " + canJump + " CanDoubleJump: " + canDoubleJump);

        // Get input
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        bool jumpButtonDownPressed = Input.GetKeyDown(KeyCode.Space);
        requestJump = (requestJump || jumpButtonDownPressed) ? true : false;
        bool jumpButtonCurrentlyPressed = Input.GetKey(KeyCode.Space);

        // Horizontal
        Vector2 velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        velocity.x = horizontalSpeed * hor;
        if(hor > 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        // Jumping
        if (canJump && requestJump)
        {
            velocity.y = verticalSpeed;
            jumping = true;
            canJump = false;
            requestJump = false;
        }
        else if (!canJump && canDoubleJump && requestJump)
        {
            velocity.y = verticalSpeed;
            canDoubleJump = false;
            requestJump = false;
        }
        else
        {
            requestJump = false;
        }

        // Set velocity back to rigidbody
        rb.velocity = velocity;


        if (jumping && !jumpButtonCurrentlyPressed && rb.velocity.y > 0)
        {
            rb.AddForce(new Vector2(0, downForce));
        }

        // Reset jump capability
        canJump = false;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCount++;
        if(collision.contacts[0].normal.y > .7)
        {
            canJump = true;
            canDoubleJump = true;
            jumping = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > .7)
        {
            canJump = true;
            canDoubleJump = true;
        }
        else
        {
            if (!canJump)
            {
                canJump = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collisionCount--;
    }
}
