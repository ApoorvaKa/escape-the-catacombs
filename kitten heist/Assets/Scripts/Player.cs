using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player p;

    public float speed;
    public bool canMove = true;
    public bool isHiding = false;
    public Animator animator;

    private Rigidbody2D rb;
    float horizontal, vertical;

    void Start() {
        p = this;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        if (canMove && !isHiding) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            GetComponent<BoxCollider2D>().enabled = true;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Speed", horizontal * horizontal + vertical * vertical);

            rb.velocity = new Vector2(horizontal, vertical).normalized * speed;

        } else {
            rb.velocity = new Vector2(0, 0);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Speed", 0);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        }

    }
}
