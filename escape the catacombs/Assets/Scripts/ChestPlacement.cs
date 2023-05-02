using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestPlacement : MonoBehaviour
{
    public List<GameObject> commonChests = new List<GameObject>(); //common items
    public List<GameObject> rareChests = new List<GameObject>(); // rare items
    public List<GameObject> spawnPoints = new List<GameObject>();
    public int itemMin = 2;
    public int itemMax = 4;
    public int chestCount;
    public GameObject hidingParent;
    // Start is called before the first frame update
    void Start()
    {
            chestCount = Random.Range(itemMin, itemMax);
            Debug.Log("Chest Count: " + chestCount);
            int counter = 0;
            while (counter < chestCount)
            {
                int randomPoint = Random.Range(0, spawnPoints.Count);
                Debug.Log("Random Point: " + randomPoint);
                float rareDropCheck = Random.Range(0, PublicVars.rareOdds);
                Debug.Log("RareDrop: " + rareDropCheck);
                if (rareDropCheck == 7)
                {
                    int randomRare = Random.Range(0, rareChests.Count);
                    Debug.Log("Random rare: " + randomRare);
                    GameObject newChest = Instantiate(rareChests[randomRare], spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    rareChests.RemoveAt(randomRare);
                    spawnPoints.RemoveAt(randomPoint);
                    newChest.transform.parent = hidingParent.transform;
                }
                else
                {
                    int randomCommon = Random.Range(0, commonChests.Count);
                    Debug.Log("Random common: " + randomCommon);
                    GameObject newChest = Instantiate(commonChests[randomCommon], spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    commonChests.RemoveAt(randomCommon);
                    spawnPoints.RemoveAt(randomPoint);
                    newChest.transform.parent = hidingParent.transform;
                }
                counter++;
                Debug.Log("Counter: " + counter);
            }
        GameManager.gm.itemsInLevel += chestCount;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
