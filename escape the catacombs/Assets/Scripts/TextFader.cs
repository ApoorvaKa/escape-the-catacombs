using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFader : MonoBehaviour
{
    public TextMeshProUGUI ins;
    public Color insColor;
    public bool fadingIn = false;
    public bool fadingOut = false;
    public float fadeStrength = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        insColor = ins.color;
        ins.color = new Color(insColor.r, insColor.g, insColor.b, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadingIn && ins.color.a < 1.0f)
        {
            Debug.Log(ins.color.a);
            ins.color = new Color(insColor.r, insColor.g, insColor.b, ins.color.a + (fadeStrength));
        }
        if (fadingOut && ins.color.a > 0.0f)
        {
            ins.color = new Color(insColor.r, insColor.g, insColor.b, ins.color.a - (fadeStrength));
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Entered");
            fadingIn = true;
            fadingOut = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Exited");
            fadingIn = false;
            fadingOut = true;
        }
    }

}
