using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player p;

    [SerializeField]
    private FloatSO speedSO;
    public float speed;
    public bool isBoosted = false;
    public static bool canMove = true;
    public bool isHiding = false;
    public static bool slowZone = false; 

    public float hunger = 10;

    public Animator playerAnimator;
    public Animator batAnimator; 

    private Rigidbody2D rb;
    float horizontal, vertical;

    float maxHunger;
    float speedHunger;
    public AudioSource ado;
    bool dying = false;
    public ShootPotions sp;

    void Start() {
        GameManager.gm.hunger = (int)hunger;
        maxHunger = hunger;
        p = this;
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("LowerFood", 5.0f, 5.0f);
        canMove = true; 
        isHiding = false; 
        slowZone = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        if (speedSO.Value < 5){
            speedSO.Value = 5;  
        }
        //ado = GetComponent<AudioSource>();
    }
    
    void Update() {
        speedHunger = maxHunger % hunger;
        speedHunger = speedHunger / 10;
        // cut speed in half if in slow zone
        if (slowZone){
            speed = speedSO.Value / 2;
        }else{
            speed = speedSO.Value;
        }
        if(hunger == 1) //lowest value
        {
            speedHunger = 1.5f;
        }
        speed -= speedHunger;

        if (canMove && !isHiding) {
            if(slowZone){
                GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
            } else {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
            GetComponent<BoxCollider2D>().enabled = true;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            playerAnimator.SetFloat("Horizontal", horizontal);
            playerAnimator.SetFloat("Speed", horizontal * horizontal + vertical * vertical);
            batAnimator.SetFloat("Horizontal", horizontal);
            batAnimator.SetFloat("Speed", horizontal * horizontal + vertical * vertical);

            rb.velocity = new Vector2(horizontal, vertical).normalized * speed;

        } else if (!canMove && !isHiding) {
            rb.velocity = new Vector2(0, 0);
            playerAnimator.SetFloat("Horizontal", 0);
            playerAnimator.SetFloat("Speed", 0);
            batAnimator.SetFloat("Horizontal", 0);
            batAnimator.SetFloat("Speed", 0);
            //GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
            //StartCoroutine(waiter());
        } else {
            rb.velocity = new Vector2(0, 0);
            playerAnimator.SetFloat("Horizontal", 0);
            playerAnimator.SetFloat("Speed", 0);
            batAnimator.SetFloat("Horizontal", 0);
            batAnimator.SetFloat("Speed", 0);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        }

    }
    // lower hunger every 5 seconds
    public void LowerFood(){
        if (hunger > 1)
            hunger -= 1;
        GameManager.gm.hunger = (int)hunger;
        GameManager.gm.hungerText.text = hunger.ToString();
        GameManager.gm.hungerBar.transform.localScale = new Vector3( hunger/20, 1 ,  1 );
        if (hunger <= 0) {
            hunger = 1;
            // game over
            //GameManager.gm.Restart();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dying && (collision.gameObject.CompareTag("Arrow") || collision.gameObject.CompareTag("Projectile")))
        {
            dying = true;
            //hurt player
            //GameManager.gm.Restart();
            GameManager.gm.fadeOut();
            canMove = false;

            StartCoroutine(audioFade(2.5f));
            Debug.Log("PLAYER HITTTTTTTT");
            //playerAnimator.SetTrigger("Player Dead");
            //Invoke("EndGame", 1);

        }
        else if (!GameManager.gm.hasEnded && collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    public void cleanScene()
    {
        gameObject.transform.position = PublicVars.spawnCoords;
        foreach (GuardController guard in GameManager.gm.distracts)
        {
            guard.gameObject.transform.position = guard.patrolPoints[0].transform.position;
            Debug.Log("Moved");
            guard.gameObject.transform.rotation = guard.originalRotation;
            guard.AlertTimeLeft = 0;
            guard.dialogue = "";
            //guard.state = GuardController.GuardStates.Stopped;
        }
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
        sp.Reload();
        hunger = maxHunger;
        GameManager.gm.hunger = (int)hunger;
        GameManager.gm.hungerText.text = hunger.ToString();
        GameManager.gm.hungerBar.transform.localScale = new Vector3(hunger / 20, 1, 1);
        GameManager.gm.fadeIn();
        canMove = true;
        ado.Play();
        dying = false;
    }

    IEnumerator audioFade(float fadeTime)
    {
        float startVolume = ado.volume;
        while(ado.volume > 0)
        {
            ado.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        ado.Stop();
        ado.volume = startVolume;
    }
}
