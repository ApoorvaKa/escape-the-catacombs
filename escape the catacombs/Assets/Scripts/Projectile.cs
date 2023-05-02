using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float timeToDie = 3f;
    private float elapsedTime = 0f;
    private GameObject player;
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        speed += PublicVars.runNumber / 5;
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        Destroy(gameObject, timeToDie);
    }

    public void Move()
    {
        GetComponent<Rigidbody2D>().velocity = speed * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Player"))
        {
            //hurt player
            //GameManager.gm.Restart();
            collision.gameObject.transform.position = PublicVars.spawnCoords;
            foreach(GuardController guard in GameManager.gm.distracts)
            {
                guard.gameObject.transform.position = guard.patrolPoints[0].transform.position;
                guard.AlertTimeLeft = 0;
                guard.dialogue = "";
                //guard.state = GuardController.GuardStates.Stopped;
            }
            Debug.Log("PLAYER HITTTTTTTT");
            //playerAnimator.SetTrigger("Player Dead");
            //Invoke("EndGame", 1);
            
        } else if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    void EndGame(){
        //GameManager.gm.Restart();
        Destroy(gameObject);
    }


}
