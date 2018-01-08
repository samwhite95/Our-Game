using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

    GameObject player;
    Vector3 newPos;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        newPos = player.transform.position;
        newPos[2] = -10;
        if(newPos[1] < -1) { newPos[1] = -1; }
        transform.position = newPos;
        


	}
}
