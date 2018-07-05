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

    public int maxHealth;
    private int health;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontalSpeed = 7;
    private float verticalSpeed = 14;
    private float downForce = -100;
    private bool canJump = true;
    private bool canDoubleJump = true;
    private bool jumping = false;
    private bool isAlive = true;

    //Attack variables and references
    private GameObject attackPoint;
    private BoxCollider2D attackPointCollider;
    private Vector2 attackPointOffset;
    private Quaternion attackPointRotation;
    private SpriteRenderer attackPointRenderer;
    private float attackHitboxActiveTime = 0.2f;
    private float attackTimer;
    private bool canAttack;

    //Attack variables and references
    private GameObject attackPoint;
    private BoxCollider2D attackPointCollider;
    private Vector2 attackPointOffset;
    private Quaternion attackPointRotation;
    private SpriteRenderer attackPointRenderer;
    private float attackHitboxActiveTime = 0.2f;
    private float attackTimer;
    private bool canAttack;

    private int collisionCount = 0;

    private bool requestJump = false;

    // Use this for initialization
    void Start () {
        health = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        attackPoint = GameObject.Find("AttackPoint") as GameObject;
        attackPointCollider = attackPoint.GetComponent<BoxCollider2D>();
        attackPointCollider.enabled = false;
        attackPointRenderer = attackPoint.GetComponent<SpriteRenderer>();
        attackPointRenderer.enabled = false;
        attackTimer = 0f;
        canAttack = true;
    }
	
    void Update ()
    {
        if (isAlive)
        {
            bool jumpButtonDownPressed = Input.GetButtonDown("Jump");
            requestJump = (requestJump || jumpButtonDownPressed) ? true : false;

        requestJump = (requestJump || jumpButtonDownPressed) ? true : false;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        //Attack handling
        if (canAttack)
        {
            if (hor > 0)
            {
                attackPointOffset = new Vector2(0.65f, 0f);
                attackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
            }
            else
            {
                attackPointOffset = new Vector2(-0.65f, 0f);
                attackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }

            if (ver > 0)
            {
                attackPointOffset = new Vector2(0f, 0.8f);
                attackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
            }
        }

        attackPoint.transform.localPosition = attackPointOffset;
        attackPoint.transform.localRotation = attackPointRotation;

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            //If the attack hitbox has been active for the specified amount of time...
            if (attackTimer >= attackHitboxActiveTime)
            {
                //...then reset the timer to 0, reset attack point collider and renderer, and enable attacking.
                canAttack = true;
                attackTimer = 0f;

                attackPointCollider.enabled = false;
                attackPointRenderer.enabled = false;
            }
        }

        //Get player attack input
        if (Input.GetMouseButtonDown(0))
        {
            //If we can attack, show weapon and enable the attack hitbox
            if (canAttack)
            {
                attackPointCollider.enabled = true;
                attackPointRenderer.enabled = true;
                canAttack = false;
            }
        }
    }

	// Rigidbody physics should be done in FixedUpdate
	void FixedUpdate () {
        //print("RequestJump: " + requestJump + " CanJump: " + canJump + " CanDoubleJump: " + canDoubleJump);

        // Get input
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        bool jumpButtonDownPressed = Input.GetButtonDown("Jump");
        requestJump = (requestJump || jumpButtonDownPressed) ? true : false;
        bool jumpButtonCurrentlyPressed = Input.GetButton("Jump");

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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        //Nothing else yet. Do game over stuff in here or whatever.
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
