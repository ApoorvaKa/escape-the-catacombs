using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunChange : MonoBehaviour
{
    public GameObject run2;
    public GameObject run3;
    public GameObject run4;
    public GameObject run5;
    // Start is called before the first frame update
    void Start()
    {
        if(PublicVars.runNumber == 2)
        {
            run2.SetActive(true);
        }
        else if(PublicVars.runNumber == 3)
        {
            run2.SetActive(true);
            run3.SetActive(true);
        }
        else if (PublicVars.runNumber == 4)
        {
            run2.SetActive(true);
            run3.SetActive(true);
            run4.SetActive(true);
        }
        else if (PublicVars.runNumber >= 5)
        {
            run2.SetActive(true);
            run3.SetActive(true);
            run4.SetActive(true);
            run5.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
