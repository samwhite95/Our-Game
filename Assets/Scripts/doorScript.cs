using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorScript : MonoBehaviour {

    public string doorName;

	

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (doorName != null && Input.GetAxisRaw("Vertical") > 0)
        {
            switch(doorName)
            {
                case "firstExit":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;

            }
        }
    }
}
