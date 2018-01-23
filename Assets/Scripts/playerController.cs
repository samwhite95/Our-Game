using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    Collider2D col;
    Vector2 defaultScale;

    Rigidbody2D rb2d;
    public float speed;
    public float jumpMod;
    public float wallJumpMod;
    public bool ground;
    public float airSpeed;
    bool canDoubleJump;
    public float jumpDelay;
    Vector2 velocity;
    public float groundFriction;
    public float playv;
    public float groundPoundSpeed;

    float distToGround;
    float distToSides;
    public int wallTouch; // 0 no wall, 1 left, 2 right
    public bool inGroundPound;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        distToSides = GetComponent<Collider2D>().bounds.extents.x;
        col = GetComponent<Collider2D>();
        defaultScale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        wallTouch = isTouchingWall();
        playv = rb2d.velocity.x;
        groundPound();
        if (!inGroundPound)
        {
            flipOrientation();
            playerMovement();
        }
        if(isGrounded()) { canDoubleJump = true; inGroundPound = false; }
        //facingRight();


	}

    void groundPound()
    {
        if(!isGrounded() && Input.GetAxisRaw("Vertical") < 0 && Input.GetButtonDown("Fire1"))
        {
            inGroundPound = true;
            Vector2 resetVelocity = new Vector2(0, 0);
            rb2d.velocity = resetVelocity;
            rb2d.AddForce(Vector2.down * groundPoundSpeed, ForceMode2D.Impulse);
        }
    }

    void playerMovement()
    {
        groundDrag();
        float moveHoriz = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(moveHoriz * speed, 0);
        if(!isGrounded())
        {
            movement.x = airSpeed * moveHoriz;
        }
        velocity = new Vector2(0, 0);
        velocity.y = rb2d.velocity.y;
        if ((rb2d.velocity.x > 0 && moveHoriz < 0 && isGrounded()) || (rb2d.velocity.x < 0 && moveHoriz > 0 && isGrounded())) { rb2d.velocity = velocity; }
        

        rb2d.AddForce(movement, ForceMode2D.Impulse);

        velocityCap();
        
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb2d.AddForce(Vector2.up * jumpMod, ForceMode2D.Impulse);
        }
        if (Input.GetButtonDown("Jump") && !isGrounded() && canDoubleJump && isTouchingWall() == 0)
        {
            StartCoroutine(doubleJump());
        }
        if(Input.GetButtonDown("Jump") && !isGrounded() && wallTouch != 0)
        {
            Vector2 resetVelocity = new Vector2(0, 0);
            rb2d.velocity = resetVelocity;
            if(wallTouch == 1)
            {
                Vector2 upAndRight = new Vector2(Vector2.right.x, Vector2.up.y * 1.5f);
                rb2d.AddForce(upAndRight * wallJumpMod, ForceMode2D.Impulse);
            }
            if (wallTouch == 2)
            {
                Vector2 upAndLeft = new Vector2(Vector2.left.x, Vector2.up.y * 1.5f);
                rb2d.AddForce(upAndLeft * wallJumpMod, ForceMode2D.Impulse);
            }
        }
    }

    void velocityCap()
    {
        
            if (rb2d.velocity.x > speed && facingRight())
            {
                velocity.x = speed;
                velocity.y = rb2d.velocity.y;
                rb2d.velocity = velocity;
            }

            if (rb2d.velocity.x < -speed && !facingRight())
            {
                velocity.x = -speed;
                velocity.y = rb2d.velocity.y;
                rb2d.velocity = velocity;
            }
        

        
    }

    IEnumerator doubleJump()
    {
        canDoubleJump = false;
        yield return new WaitForSeconds(jumpDelay);
        velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.velocity = velocity;
        rb2d.AddForce(Vector2.up * jumpMod * 1.5f, ForceMode2D.Impulse);
        
    }

    bool isGrounded()
    {
        Vector2 bottomLeft = new Vector2(col.bounds.center.x - col.bounds.extents.x, transform.position.y);
        Vector2 bottomRight = new Vector2(col.bounds.center.x + col.bounds.extents.x, transform.position.y);
        bool ray1 = Physics2D.Raycast(bottomLeft, Vector2.down, distToGround + 0.1f);
        bool ray2 = Physics2D.Raycast(bottomRight, Vector2.down, distToGround + 0.1f);
        if(ray1 || ray2) { return true; }
        else { return false; }
    }

    void groundDrag()
    {
        if(isGrounded())
        {
            if(Input.GetAxisRaw("Horizontal") == 0 && groundFriction != 0)
            {
                rb2d.velocity = rb2d.velocity / groundFriction;
            }
        }
    }
    bool lastFacing = false; //false left true right
    public bool facingRight()
    {
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            lastFacing = true;
            return true;
        }
        if(Input.GetAxisRaw("Horizontal") < 0 )
        {
            lastFacing = false;
            return false;
        }
        else
        {
            return lastFacing;
        }
    }

    void flipOrientation()
    {
        bool right = facingRight();
        if (right)
        {
            transform.localScale = defaultScale;
        }
        if (!right)
        {
            Vector2 newScale = defaultScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    int isTouchingWall()
    {
        Vector2 bottomBound = new Vector2(transform.position.x, col.bounds.center.y - col.bounds.extents.y);
        Vector2 topBound = new Vector2(transform.position.x, col.bounds.center.y + col.bounds.extents.y);

        //left wall check
        bool leftRay1 = Physics2D.Raycast(bottomBound, Vector2.left, distToSides + 0.2f);
        bool leftRay2 = Physics2D.Raycast(topBound, Vector2.left, distToSides + 0.2f);

        //right wall check
        bool rightRay1 = Physics2D.Raycast(bottomBound, Vector2.right, distToSides + 0.2f);
        bool rightRay2 = Physics2D.Raycast(topBound, Vector2.right, distToSides + 0.2f);

        if(leftRay1 || leftRay2)
        {
            return 1;
        }
        if(rightRay1 || rightRay2)
        {
            return 2;
        }
        else
        {
            return 0;
        }

    }


}
