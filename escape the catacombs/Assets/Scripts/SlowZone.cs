using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    void  Start() {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .7f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Player"))
        {
            Player.slowZone = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player.slowZone = false; 
    }
}
