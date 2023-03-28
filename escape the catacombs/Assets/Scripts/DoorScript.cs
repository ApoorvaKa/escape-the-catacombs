using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public Item acceptedItem;

    public bool hasPlayer = false;
    public LayerMask layer;

    private AudioSource audioSource;
    public AudioClip unlockSound;
    public AudioClip errorSound;
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && hasPlayer && Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0, layer);
            
            if (hit)
            {
                
                TakeItem();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasPlayer = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void TakeItem()
    {
        print("taking Item");
        if (Inventory.i.UseItem(acceptedItem))
        {

            audioSource.PlayOneShot(unlockSound);
            Destroy(gameObject, .3f);
        } else
        {
            audioSource.PlayOneShot(errorSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasPlayer = false;
        }
    }
}
