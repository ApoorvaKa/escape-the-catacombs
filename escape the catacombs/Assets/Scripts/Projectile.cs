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
        if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
            PublicVars.attacksEvaded++;
        }
    }

    void EndGame(){
        //GameManager.gm.Restart();
        Destroy(gameObject);
    }


}
