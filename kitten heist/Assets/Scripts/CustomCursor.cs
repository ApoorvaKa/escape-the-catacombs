using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Vector3 mDisplacement;
    public GameObject pointer, hovered;
    public Color close, far;
    public LayerMask layer;
    private EventSystem eventSystem;
    public bool hasPlayer = true;

    private Animator anim;

    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (hasPlayer)
        {
            float distance = Vector2.Distance(Player.p.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (distance <= 1.3)
            {
                pointer.transform.GetChild(0).GetComponent<Image>().color = close;
                hovered.transform.GetChild(0).GetComponent<Image>().color = close;
            }
            else
            {
                pointer.transform.GetChild(0).GetComponent<Image>().color = far;
                hovered.transform.GetChild(0).GetComponent<Image>().color = far;
            }
        }
        

        if (eventSystem.IsPointerOverGameObject())
        {
            pointer.SetActive(true);
            hovered.SetActive(false);
            pointer.transform.GetChild(0).GetComponent<Image>().color = close;
        }
        else
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0, layer);
            if (hit)
            {
                pointer.SetActive(false);
                hovered.SetActive(true);
            }
            else
            {
                pointer.SetActive(true);
                hovered.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Click");
        }
        //else
        //{
        //    anim.ResetTrigger("Click");
        //}
        

        if (Cursor.visible)
        {
            Cursor.visible = false;
        }

        transform.position = Input.mousePosition + mDisplacement;
        
    }
}