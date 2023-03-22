using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingObject : MonoBehaviour
{

    public GameObject exitPosition;
    public LayerMask layer;
    public bool isHiding;

    // Start is called before the first frame update
    void Start()
    {
        isHiding = false;
    }

    void Update(){
        //// input e for interact
        //if (Input.GetKeyDown(KeyCode.E)){
        //    HidePlayer();
        //}
        if (Time.timeScale != 0 && Input.GetMouseButtonDown(0))
        {
            HidePlayer();
        }
    }

    private void HidePlayer() {
        float distance = Vector2.Distance(Player.p.transform.position, transform.position);
        if (distance < 2) {
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
                    //Player.p.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    Player.p.transform.position = transform.position;
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    isHiding = true;
                    print("Player hiding");
                    Player.p.isHiding = true;
                    //Player.p.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
    }
}
