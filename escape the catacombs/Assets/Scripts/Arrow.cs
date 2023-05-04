using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public string direction;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        // base rotation and direction of flight on direction var
        if (direction == "R")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        } else if (direction == "L")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
        } else if (direction == "U")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        } else if (direction == "D")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Player"))
        {
            //hurt player
            collision.gameObject.transform.position = PublicVars.spawnCoords;
            foreach (GuardController guard in GameManager.gm.distracts)
            {
                guard.gameObject.transform.position = guard.patrolPoints[0].transform.position;
                guard.AlertTimeLeft = 0;
                guard.dialogue = "";
                //guard.state = GuardController.GuardStates.Stopped;
            }
            GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
            foreach (GameObject arrow in arrows)
            {
                Destroy(arrow);
            }
        } else if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }
}
