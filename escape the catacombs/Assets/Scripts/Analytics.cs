using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Analytics : MonoBehaviour
{
    public TextMeshProUGUI itemsFound;
    public TextMeshProUGUI missedItems;
    public TextMeshProUGUI deathCount;
    public TextMeshProUGUI levelsCompleted;
    public TextMeshProUGUI timeCompletion;
    public TextMeshProUGUI rank;

    // Start is called before the first frame update
    void Start()
    {
        itemsFound.text = PublicVars.totalItemsFound.ToString();
        missedItems.text = PublicVars.missedItems.ToString();
        int totalDeaths = PublicVars.witchDeathCount + PublicVars.arrowDeathCount;
        deathCount.text = totalDeaths.ToString();
        levelsCompleted.text = PublicVars.levelsCompleted.ToString();
        string elapsedTimeFormatted = FormatElapsedTime(PublicVars.elapsedTime);
        timeCompletion.text = elapsedTimeFormatted;

        string rankStr = CalculateRank();
        rank.text = rankStr;

        GameObject inv = GameObject.FindGameObjectWithTag("inventory");
        Destroy(inv);
        PublicVars.totalItemsFound = 0;
        PublicVars.totalItems = 0;
        PublicVars.witchDeathCount = 0;
        PublicVars.levelsCompleted = 0;
        PublicVars.arrowDeathCount = 0;
        PublicVars.missedItems = 0;
        PublicVars.elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string CalculateRank()
    {
        float score = 0;
        float itemsPerLevel = PublicVars.totalItemsFound / PublicVars.levelsCompleted;
        score += itemsPerLevel;
        score -= PublicVars.missedItems / PublicVars.levelsCompleted;
        score += PublicVars.levelsCompleted;
        score -= (PublicVars.arrowDeathCount + PublicVars.witchDeathCount) / 2;
        string scoreLetter = "";
        if(score < 5)
        {
            scoreLetter = "E";
        }
        else if(score < 8 && score >= 5)
        {
            scoreLetter = "D";
        }
        else if (score < 11 && score >= 8)
        {
            scoreLetter = "C";
        }
        else if (score < 15 && score >= 11)
        {
            scoreLetter = "B";
        }
        else if (score < 25 && score >= 15)
        {
            scoreLetter = "A";
        }
        else if (score < 50 && score >= 25)
        {
            scoreLetter = "S";
        }
        else if (score >= 50)
        {
            scoreLetter = "SSS";
        }
        return scoreLetter;
    }
    public string FormatElapsedTime(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        return string.Format("{0:D2} : {1:D2} : {2:D2}", hours, minutes, seconds);
    }
}
