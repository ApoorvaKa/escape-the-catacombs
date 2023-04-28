using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingObject : MonoBehaviour
{

    public GameObject exitPosition;
    public LayerMask layer;
    public bool isHiding;
    public ParticleSystem boxSparkles;
    public float pDistance;
    public bool colliding = false;
    public bool canEscape = true;

    // Start is called before the first frame update
    void Start()
    {
        isHiding = false;
    }

    void Update(){
        //// input e for interact
        var emission = boxSparkles.emission;
        pDistance = Vector2.Distance(Player.p.transform.position, transform.position);
        if(pDistance < 5 && pDistance > 4)
        {
            emission.rateOverTime = 1;
        }
        else if(pDistance < 4 && pDistance > 3)
        {
            emission.rateOverTime = 3;
        }
        else if (pDistance < 4 && pDistance > 2)
        {
            emission.rateOverTime = 5;
        }
        else if (pDistance < 4 && pDistance > 1)
        {
            emission.rateOverTime = 10;
        }
        else if (pDistance < 1)
        {
            emission.rateOverTime = 13;
        }
        else
        {
            emission.rateOverTime = 0;
        }
        if (Time.timeScale != 0 && Input.GetMouseButtonDown(0))
        {
            HidePlayer();
        }
        if(Time.timeScale != 0 && Input.GetKeyDown(KeyCode.E))
        {
            HidePlayerKey();
        }
        if(Time.timeScale != 0 && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            EscapeWithMovement();
        }
    }

    private void HidePlayer() {
        float distance = Vector2.Distance(Player.p.transform.position, transform.position);
        if (distance < 2.5) {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0, layer);
            if (hit)
            {
                if (isHiding)
                {
                    Player.p.transform.position = exitPosition.transform.position;
                    GetComponent<SpriteRenderer>().color = Color.white;
                    isHiding = false;
                    print("Player not hiding");
                    Player.p.isHiding = false;
                    Player.p.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    Player.p.transform.position = transform.position;
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    isHiding = true;
                    print("Player hiding");
                    Player.p.isHiding = true;
                    Player.p.GetComponent<BoxCollider2D>().enabled = true;
                    StartCoroutine(hideWait());
                }
            }
        }
    }
    private void HidePlayerKey()
    {
        if (colliding)
        {
            if (isHiding)
            {
                Player.p.transform.position = exitPosition.transform.position;
                GetComponent<SpriteRenderer>().color = Color.white;
                isHiding = false;
                print("Player not hiding");
                Player.p.isHiding = false;
                Player.p.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                Player.p.transform.position = transform.position;
                GetComponent<SpriteRenderer>().color = Color.gray;
                isHiding = true;
                print("Player hiding");
                Player.p.isHiding = true;
                Player.p.GetComponent<BoxCollider2D>().enabled = true;
                StartCoroutine(hideWait());
            }
        }
    }

    private void EscapeWithMovement()
    {
        if (isHiding && canEscape)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            isHiding = false;
            print("Player not hiding");
            Player.p.isHiding = false;
            Player.p.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        colliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
    }

    IEnumerator hideWait()
    {
        canEscape = false;
        yield return new WaitForSeconds(1.0f);
        canEscape = true;
    }
}
