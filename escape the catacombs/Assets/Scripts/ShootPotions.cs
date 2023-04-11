using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//adapted from https://www.youtube.com/watch?v=-bkmPm_Besk
public class ShootPotions : MonoBehaviour
{

    public Camera mainCam;
    private Vector3 mousePos;
    public GameObject potion;
    public Transform batTransformation;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    public Animator batAnimator; 
    private int potionsThrown; 
    public Image[] potions;
    

    
    // Start is called before the first frame update
    void Start()
    {
        potionsThrown = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        if (!canFire){
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring){
                canFire = true;
                timer = 0;
            }
        }
        if(Input.GetMouseButton(1) && canFire && potionsThrown < 3 ){
            canFire = false;
            Instantiate(potion, batTransformation.position, Quaternion.identity);
            batAnimator.SetTrigger("Throwing");
            Destroy(potions[potionsThrown]);
            potionsThrown++;

        }
    }
}
