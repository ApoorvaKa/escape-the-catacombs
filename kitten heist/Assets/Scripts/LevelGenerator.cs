using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    /*
     * 
     * 
     */
    public GameObject levelGrid;
    public List<GameObject> roomType1 = new List<GameObject>();
    public List<GameObject> roomType2 = new List<GameObject>();
    public List<GameObject> roomType3 = new List<GameObject>();
    public List<GameObject> roomType4 = new List<GameObject>();
    public List<GameObject> roomType5 = new List<GameObject>();
    public int gridLength = 5;
    public int gridHeight = 5;
    int roomSize = 10;
    int gridSize;
    int[] floorplan;
    int startIndex;
    int endIndex;
    int crawlDistance = 0;
    bool hitBottom = false;
    // Start is called before the first frame update
    void Start()
    {
        gridSize = gridLength * gridHeight;
        floorplan = new int[gridSize];
        for(int i = 0; i < floorplan.Length; i++) //set all room indices to 0, or "blank space"
        {
            floorplan[i] = 0;
        }
        startIndex = Random.Range(0, gridLength); //picks a starting index from our top row
        floorplan[startIndex] = 1; //makes our starting room type 1
        int currIndex = startIndex;
        bool firstRoom = true;
        int initialPath;
        while (firstRoom)
        {
            initialPath = Random.Range(0, 3); //goes left, right, or down
            if(initialPath == 0 && floorplanCheckLeft(currIndex))
            {
                currIndex = currIndex - 1;
                floorplan[currIndex] = Random.Range(2, 4); //gives it value of 2 or 3 for now
                firstRoom = false;
                Debug.Log("going left");
            }
            if (initialPath == 1 && floorplanCheckRight(currIndex))
            {
                currIndex = currIndex + 1;
                floorplan[currIndex] = Random.Range(2, 4); //gives it value of 2 or 3 for now
                firstRoom = false;
                Debug.Log("going right");
            }
            if (initialPath == 2 && floorplanCheckDown(currIndex))
            {
                currIndex = currIndex + gridLength;
                floorplan[currIndex] = Random.Range(2, 4); //gives it value of 2 or 3 for now
                firstRoom = false;
                Debug.Log("going down");
            }
        }

        while(hitBottom == false)
        {
            int path = Random.Range(0, 3);
            crawlDistance = 0;
            if (path == 0 && floorplanCheckLeft(currIndex))
            {
                if (floorplan[currIndex - 1] == 0)
                {
                    floorplan[currIndex - 1] = Random.Range(2, 4); //gives it value of 2 or 3 for now
                    currIndex--;
                }
            }
            if (path == 1 && floorplanCheckRight(currIndex))
            {
                if (floorplan[currIndex + 1] == 0)
                {
                    floorplan[currIndex + 1] = Random.Range(2, 4); //gives it value of 2 or 3 for now
                    currIndex++;
                }
            }
            if (path == 2 && floorplanCheckDown(currIndex))
            {
                if (floorplan[currIndex + gridLength] == 0)
                {
                    floorplan[currIndex + gridLength] = Random.Range(2, 4); //gives it value of 2 or 3 for now
                    currIndex += gridLength;
                }
            }
            if(path == 2 && !floorplanCheckDown(currIndex))
            {
                floorplan[currIndex] = 4;
                hitBottom = true;
            }
        }

        for(int i = 0; i < floorplan.Length; i++)
        {
            placeRoom(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void placeRoom(int index)
    {
        int roomX, roomY, roomZ;
        roomX = roomSize * (index % gridLength);
        roomY = (-1 * roomSize * (index / gridHeight));
        Debug.Log(roomY);
        Debug.Log(index);
        roomZ = 0;
        Vector3 roomPos = new Vector3(roomX, roomY, roomZ);
        if (floorplan[index] == 1)
        {
            GameObject room = Instantiate(roomType1[0], roomPos, Quaternion.identity);
            room.transform.parent = levelGrid.transform;
        }
        else if (floorplan[index] == 2)
        {
            GameObject room = Instantiate(roomType2[0], roomPos, Quaternion.identity);
            room.transform.parent = levelGrid.transform;
        }
        else if (floorplan[index] == 3)
        {
            GameObject room = Instantiate(roomType3[0], roomPos, Quaternion.identity);
            room.transform.parent = levelGrid.transform;
        }
        else if (floorplan[index] == 4)
        {
            GameObject room = Instantiate(roomType4[0], roomPos, Quaternion.identity);
            room.transform.parent = levelGrid.transform;
        }
        else if (floorplan[index] == 0)
        {
            GameObject room = Instantiate(roomType5[0], roomPos, Quaternion.identity);
            room.transform.parent = levelGrid.transform;
        }
    }

    bool floorplanCheckUp(int index)
    {
        if(index - gridLength < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    bool floorplanCheckDown(int index)
    {
        if(index + gridLength > gridSize)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    bool floorplanCheckLeft(int index)
    {
        if(index % gridLength == 0)
        {
            return false;
        }
        else if ((index - 1) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    bool floorplanCheckRight(int index) //checks for wall on right side of room
    {
        if(((index + 1) % gridLength) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


}
