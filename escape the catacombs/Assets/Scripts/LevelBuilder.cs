using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public List<GameObject> chestPoints = new List<GameObject>();
    public List<GameObject> hazardPoints = new List<GameObject>();
    //public List<GameObject> enemyPoints = new List<GameObject>();
    public GameObject keyChest;
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> hazards = new List<GameObject>();
    public List<GameObject> zones = new List<GameObject> ();

    //public GameObject stairkeyWitch;
    bool regKey = false;
    bool stairKey = false;
    //public int enemyCount = 5;
    public int chestCount = 4;
    public int hazardCount = 4;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject zone in zones) // fills each zone with 1 chest
        {
            int randItem = Random.Range(0, items.Count);
            int randChest = 0;
            if (zone.name == "ZoneC")
            {
                randChest = Random.Range(0, 2);
                Transform chestLoc = zone.transform.GetChild(2).GetChild(randChest).gameObject.transform;
                GameObject newChest = Instantiate(items[randItem], chestLoc.position, Quaternion.identity);
                newChest.transform.SetParent(zone.transform.GetChild(2));
            }
            else
            {
                randChest = Random.Range(0, 3);
                Transform chestLoc = zone.transform.GetChild(2).GetChild(randChest).gameObject.transform;
                GameObject newChest = Instantiate(items[randItem], chestLoc.position, Quaternion.identity);
                newChest.transform.SetParent(zone.transform.GetChild(2));
            }
            addConditionalHazards(zone, randChest);
            items.RemoveAt(randItem);
        }
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");

        int randKeyChest = 0;
        int zoneSelection;
        Transform keyPos;
        GameObject kzone = zones[0];
        int checker = 0;
        while(regKey == false)
        {
            randKeyChest = 0;
            checker = 0;
            zoneSelection = Random.Range(0, 3);
            kzone = zones[zoneSelection];
            if (kzone.name == "ZoneC")
            {
                randKeyChest = Random.Range(0, 2);
                keyPos = kzone.transform.GetChild(2).GetChild(randKeyChest).gameObject.transform;
                for(int i = 0; i < chests.Length; i++)
                {
                    if(chests[i].transform.position == keyPos.position)
                    {
                        break;
                    }
                    else
                    {
                        checker++;
                    }
                    if(checker == chests.Length)
                    {
                        GameObject newChest = Instantiate(keyChest, keyPos.position, Quaternion.identity);
                        newChest.transform.SetParent(kzone.transform.GetChild(2));
                        regKey = true;
                    }
                }
            }
            else
            {
                randKeyChest = Random.Range(0, 3);
                keyPos = kzone.transform.GetChild(2).GetChild(randKeyChest).gameObject.transform;
                for (int i = 0; i < chests.Length; i++)
                {
                    if (chests[i].transform.position == keyPos.position)
                    {
                        break;
                    }
                    else
                    {
                        checker++;
                    }
                    if (checker == chests.Length)
                    {
                        GameObject newChest = Instantiate(keyChest, keyPos.position, Quaternion.identity);
                        newChest.transform.SetParent(kzone.transform.GetChild(2));
                        regKey = true;
                    }
                }
            }
        }
        addConditionalHazards(kzone, randKeyChest);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void addConditionalHazards(GameObject zone, int chestLocation)
    {
        if (zone.name == "ZoneA" && chestLocation == 2)
        {
            zone.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        } 
        else if(zone.name == "ZoneA" && chestLocation == 1)
        {
            zone.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        else if (zone.name == "ZoneB" && chestLocation == 0)
        {
            zone.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else if (zone.name == "ZoneB" && chestLocation == 1)
        {
            zone.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
        else if (zone.name == "ZoneC" && chestLocation == 0)
        {
            zone.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            zone.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else if (zone.name == "ZoneC" && chestLocation == 1)
        {
            zone.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
    }
}
