using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject player;
    public static Inventory i;
    public List<Item> itemsHeld;
    public Dictionary<Item, GameObject> itemInInventory;
    [SerializeField]
    private FloatSO speedSO;

    // Start is called before the first frame update
    void Start()
    {
        i = this;
        itemInInventory = new Dictionary<Item, GameObject>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void obtainItem(Item i)
    {
        if (!itemsHeld.Contains(i) && i != null)
        {
            GameManager.gm.itemsObtained += 1;
            itemsHeld.Add(i);
            GameManager.gm.ShowItemObtained(i);
            var button = GameManager.gm.AddToInventory(i);
            itemInInventory.Add(i, button);
            if (i.tag == "speed"){
                Debug.Log("speed");
                if (speedSO.Value < 15) {
                    speedSO.Value += 3;
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
            itemsHeld.Remove(i);
            Destroy(itemInInventory[i]);
            itemInInventory.Remove(i);
            return true;
        } 
        else
        {
            // error
            return false;
        }
    }
}
