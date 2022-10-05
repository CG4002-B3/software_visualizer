using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfScoreController : MonoBehaviour
{
    public Text killsText;
    public Text deathsText;

    private int numOfKills;
    private int numOfDeaths;

    // Start is called before the first frame update
    void Start()
    {
        numOfKills = 0;
        numOfDeaths = 0;
    }

    // Update is called once per frame
    void Update()
    {
        killsText.text = "Kills: " + numOfKills;
        deathsText.text = "Deaths: " + numOfDeaths;
    }

    public void IncrementNumOfKills()
    {
        numOfKills++;
    }

    public void IncrementNumOfDeaths()
    {
        numOfDeaths++;
    }

    public void SetNumKills(int kills)
    {
        numOfKills = kills;
    }
}
