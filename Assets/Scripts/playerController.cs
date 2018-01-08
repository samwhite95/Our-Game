using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    Rigidbody2D rb2d;
    public float speed;
    public float jumpMod;
    public bool ground;
    public float airSpeed;

    float distToGround;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {

        playerMovement();
        ground = isGrounded();


	}

    void playerMovement()
    {
        float moveHoriz = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHoriz * speed, 0);
        if(!isGrounded())
        {
            movement.x = airSpeed;
        }
        Vector2 velocity = new Vector2(0, 0);
        velocity.y = rb2d.velocity.y;
        if ((rb2d.velocity.x > 0 && moveHoriz < 0) || (rb2d.velocity.x < 0 && moveHoriz > 0)) { rb2d.velocity = velocity; }

        rb2d.AddForce(movement, ForceMode2D.Impulse);

        
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb2d.AddForce(Vector2.up * jumpMod, ForceMode2D.Impulse);
        }
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.005f);
    }

    
}
