using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI title;
    Item item;
    public Button b;

    public void init(Item i)
    {
        item = i;
        image.sprite = item.itemIcon;
        title.text = item.title;
        b.onClick.AddListener(delegate { GameManager.gm.ShowItemObtained(item); });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
