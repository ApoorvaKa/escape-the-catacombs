using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory i;
    public List<Item> itemsHeld;
    public Dictionary<Item, GameObject> itemInInventory;

    // Start is called before the first frame update
    void Start()
    {
        i = this;
        itemInInventory = new Dictionary<Item, GameObject>();
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
