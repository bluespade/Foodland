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

    //Player parameters
    public int maxHealth;
    private int health;

    //Component references
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //Horizontal movement variables
    private float horizontalSpeed = 7;

    //Vertical movement and jumping variables
    private float verticalSpeed = 14;
    private float downForce = -100;
    private bool canJump = true;
    private bool canDoubleJump = true;
    private bool jumping = false;
    private bool requestJump = false;

    //Player state flags
    private bool isAlive = true;

    //Attack variables and references
    private GameObject attackPoint;
    private BoxCollider2D attackPointCollider;
    private Vector2 attackPointOffset;
    private Quaternion attackPointRotation;
    private Vector3 lastHorizontalAttackPointOffset;
    private Quaternion lastHorizontalAttackPointRotation;
    private SpriteRenderer attackPointRenderer;
    private float attackHitboxActiveTime = 0.2f;
    private float attackTimer;
    private bool canAttack;

    private int collisionCount = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        health = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();

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

            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");

            //Attack handling
            if (canAttack)
            {
                if (ver > 0)
                {
                    attackPointOffset = new Vector2(0f, 1.2f);
                    attackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
                } else
                {
                    if (hor > 0)
                    {
                        attackPointOffset = new Vector2(0.65f, 0.4f);
                        attackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
                        lastHorizontalAttackPointOffset = new Vector2(0.65f, 0.4f);
                        lastHorizontalAttackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
                    }
                    else if (hor < 0)
                    {
                        attackPointOffset = new Vector2(-0.65f, 0.4f);
                        attackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        lastHorizontalAttackPointOffset = new Vector2(-0.65f, 0.4f);
                        lastHorizontalAttackPointRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    } else
                    {
                        attackPointOffset = lastHorizontalAttackPointOffset;
                        attackPointRotation = lastHorizontalAttackPointRotation;
                    }
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
            if (Input.GetButtonDown("Fire1"))
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
        else if (hor < 0)
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

    public void ApplyHealing(int healing)
    {
        health += healing;
        if (health > maxHealth)
        {
            health = maxHealth;
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
