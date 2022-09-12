using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OppHealthBarController : MonoBehaviour
{
    private int MAX_HEALTH = 100;

    public int healthRemaining;
    public Image[] healthSegments;
    public Text healthPointText;

    public BulletController bulletController;

    // Start is called before the first frame update
    void Start()
    {
        healthRemaining = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        healthPointText.text = healthRemaining.ToString();

        for (int i = 0; i < healthSegments.Length; i++)
        {
            if (i < (int)(healthRemaining / 10))
            {
                healthSegments[i].enabled = true;
            }
            else
            {
                healthSegments[i].enabled = false;
            }
        }
    }

    public void ReduceHealth()
    {
        if (bulletController.GetBulletsRemaining() >= 0)
        {
            Debug.Log("Reducingggg");
            healthRemaining = Math.Max(healthRemaining - 10, 0);
            if (healthRemaining == 0)
            {
                healthRemaining = MAX_HEALTH;
            }
        }
    }

    public int GetHealthRemaining()
    {
        return healthRemaining;
    }
}
