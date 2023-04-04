using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player p;

    public float speed;
    public bool isBoosted = false;
    public static bool canMove = true;
    public bool isHiding = false;

    public float hunger = 10;

    public Animator animator;

    private Rigidbody2D rb;
    float horizontal, vertical;

    void Start() {
        p = this;
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("LowerFood", 5.0f, 5.0f);
    }
    
    void Update() {
        if (isBoosted) {
            speed = 10;
        } else {
            speed = 5;
        }
        if (canMove && !isHiding) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            GetComponent<BoxCollider2D>().enabled = true;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Speed", horizontal * horizontal + vertical * vertical);

            rb.velocity = new Vector2(horizontal, vertical).normalized * speed;

        } else if (!canMove && !isHiding) {
            rb.velocity = new Vector2(0, 0);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Speed", 0);
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
            StartCoroutine(waiter());
        } else {
            rb.velocity = new Vector2(0, 0);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Speed", 0);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        }

    }
    // lower hunger every 5 seconds
    public void LowerFood(){
        hunger -= 1;
        Debug.Log("hunger: " + hunger);
        if (hunger <= 0) {
            // game over
            GameManager.gm.Restart();
        }
    }

    IEnumerator waiter()
    {
        //Wait for 5 seconds
        float counter = 0;
        float waitTime = 5;
        while (counter < waitTime)
        {
            //Increment Timer until counter >= waitTime
            counter += Time.deltaTime;
            Debug.Log("We have waited for: " + counter + " seconds");
 
            yield return null;
        }

        canMove = true; 
    }
}
