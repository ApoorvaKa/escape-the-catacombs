using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string currScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currScene != "Tutorial")
        {
            //SceneManager.LoadScene(nextScene);
            GetComponent<AudioSource>().Play();
            GameManager.gm.EndScreen();
            
        } else if (collision.gameObject.CompareTag("Player") && currScene == "Tutorial") {
            SceneManager.LoadScene("Level1");
        }
    }
}
