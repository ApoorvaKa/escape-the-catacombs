using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public GameObject player;
    public static Inventory i;
    public List<Item> itemsHeld;
    public List<GameObject> itemInInventory;
//    public Dictionary<Item, GameObject> itemInInventory;
    [SerializeField]
    private FloatSO speedSO;

    // Start is called before the first frame update
    void Start()
    {
        i = this;
        itemInInventory = new List<GameObject>();
      //  itemInInventory = new Dictionary<Item, GameObject>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
        PublicVars.elapsedTime += Time.deltaTime;
    }

    public void obtainItem(Item i)
    {
        //        if (!itemsHeld.Contains(i) && i != null)
        if (i != null)
        {
            GameManager.gm.itemsObtained += 1;
            itemsHeld.Add(i);
            GameManager.gm.ShowItemObtained(i);
            var button = GameManager.gm.AddToInventory(i);
            itemInInventory.Add(button);
            if (i.tag == "speed"){
                Debug.Log("speed");
                if (speedSO.Value < 20) {
                    speedSO.Value += 5;
                }
            }
            if (i.tag == "food"){
                Debug.Log("food");
                Player.p.hunger = 20;
                GameManager.gm.hunger = 20;
                GameManager.gm.hungerBar.transform.localScale = new Vector3(20 / 20, 1, 1);
                UseItem(i);
                Debug.Log(player.GetComponent<Player>().hunger);
            }
        }
    }

    public bool UseItem(Item i)
    {
        if(itemsHeld.Contains(i)){
            int tempIndex = itemsHeld.IndexOf(i);
            itemsHeld.Remove(i);
            Debug.Log("///////// Temp Index: " + tempIndex + " /////////");
            Destroy(itemInInventory[tempIndex].gameObject);
            itemInInventory.RemoveAt(tempIndex);
            return true;
        } 
        else
        {
            // error
            return false;
        }
    }
}
