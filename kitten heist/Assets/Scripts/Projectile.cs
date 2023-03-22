using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float timeToDie = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
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
            GameManager.gm.Restart();
            Destroy(gameObject);
        } else if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }


}
