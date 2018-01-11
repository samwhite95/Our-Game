using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    Collider2D col;
    Vector2 defaultScale;

    Rigidbody2D rb2d;
    public float speed;
    public float jumpMod;
    public bool ground;
    public float airSpeed;
    bool canDoubleJump;
    public float jumpDelay;
    Vector2 velocity;
    public float groundFriction;
    public float playv;

    float distToGround;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        col = GetComponent<Collider2D>();
        defaultScale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        playv = rb2d.velocity.x;
        flipOrientation();
        playerMovement();
        if(isGrounded()) { canDoubleJump = true; }
        facingRight();


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
        if(Input.GetButtonDown("Jump") && !isGrounded() && canDoubleJump)
        {
            StartCoroutine(doubleJump());
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
    public bool lastFacing = false;
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


}
