using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;

    private float timer;
    public GameObject particles;
    private GameObject particleInstance;

    public float distance = 0;
    bool checker = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x , direction.y). normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        distance = Vector2.Distance(transform.position, mousePos);
        if(distance < 0.5f && checker)
        {
            checker = false;
            rb.velocity = new Vector2(0, 0);
            foreach (GuardController distract in GameManager.gm.distracts)
            {
                distract.GoToDistraction(transform);
            }
            Destroy(gameObject, 20f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
            particleInstance = Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(particleInstance, 5f);
            timer = 0;
        }
    }
}
