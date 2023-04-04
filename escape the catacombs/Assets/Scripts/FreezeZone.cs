using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Player"))
        {
            Player.canMove = false;
        }
    }
}
