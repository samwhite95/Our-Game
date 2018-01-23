using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundPound : MonoBehaviour {

    Collider2D col;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        col.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponentInParent<playerController>().inGroundPound)
        {
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "breakable")
        {
            Destroy(collision.gameObject);
        }
    }
}
