using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour {

    public bool isAttacking;
    public float moveSpeed;
    float timeSince;
    float timeStart;
    float percent;
    Vector2 startPos;
    Vector2 endPos;
    bool goingIn; //weapon moving back in
    float offset; //2 for right, -2 for left
    Vector2 defaultScale;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        endPos = new Vector2(transform.position.x + 2, transform.position.y);
        defaultScale = gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject.GetComponentInParent<playerController>().facingRight())
        {
            offset = 2;
        }
        else
        {
            offset = -2;
        }
        attackAction();

	}

    void attackAction()
    {
        startPos = transform.parent.localPosition;
        endPos = new Vector2(transform.parent.localPosition.x + offset, transform.parent.localPosition.y);
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            timeStart = Time.time;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if(isAttacking)
        {
            if (!goingIn)
            {
                timeSince = Time.time - timeStart;
                percent = timeSince / moveSpeed;
                transform.position = Vector2.Lerp(startPos, endPos, percent);
                if (percent >= 1)
                {
                    goingIn = true;
                    percent = 0;
                }
            }
            if(goingIn)
            {
                timeSince = Time.time - timeStart;
                percent = timeSince / moveSpeed;
                transform.position = Vector2.Lerp(endPos, startPos, percent);
                if(percent >= 1)
                {
                    goingIn = false;
                    isAttacking = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                    percent = 0;

                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "breakable")
        {
            Destroy(collision.gameObject);
        }
    }

    

}
