using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enviroDamageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && collision.transform.GetComponent<playerController>().canTakeDamage)
        {
            collision.transform.position = collision.transform.GetComponent<playerController>().safePos;
        }
        if (collision.transform.tag == "Player" && !collision.transform.GetComponent<playerController>().canTakeDamage)
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
