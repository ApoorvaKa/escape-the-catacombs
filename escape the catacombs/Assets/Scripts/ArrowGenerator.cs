using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public GameObject arrowPrefab;
    public string direction;
    public float speed = 5f;
    public float timeBetweenArrows = 1f;
    public float timeToDie = 3f;
    
    private float elapsedTime = 0f;

    private Quaternion arrowRotation;

    void Start() {
        if (direction == "R") {
            Vector3 newRotation = new Vector3(0, 0, 45);
            transform.eulerAngles = newRotation;
        } else if (direction == "L") {
            Vector3 newRotation = new Vector3(0, 0, -135);
            transform.eulerAngles = newRotation;
        } else if (direction == "U") {
            Vector3 newRotation = new Vector3(0, 0, 135);
            transform.eulerAngles = newRotation;
        } else if (direction == "D") {
            Vector3 newRotation = new Vector3(0, 0, -45);
            transform.eulerAngles = newRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeBetweenArrows)
        {
            elapsedTime = 0f;
            GenerateArrow();
        }
    }

    public void GenerateArrow()
    {
        if (direction == "R") {
            arrowRotation = Quaternion.Euler(0, 0, 270);
        } else if (direction == "L") {
            arrowRotation = Quaternion.Euler(0, 0, 90);
        } else if (direction == "U") {
            arrowRotation = Quaternion.Euler(0, 0, 0);
        } else if (direction == "D") {
            arrowRotation = Quaternion.Euler(0, 0, 180);
        }

        GameObject arrow = Instantiate(arrowPrefab, transform.position, arrowRotation);
        arrow.GetComponent<Arrow>().speed = speed;
        arrow.GetComponent<Arrow>().direction = direction;
        Destroy(arrow, timeToDie);
    }
}
