using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfHealthBarController : MonoBehaviour
{
    private int MAX_HEALTH = 100;

    public int healthRemaining;
    public Image[] healthSegments;
    public Text healthPointText;

    public BulletController bulletController;
    public GrenadeController grenadeController;
    public SelfShieldController selfShieldController;

    public SelfShieldOverlayController selfShieldOverlayController;
    public OppGrenadeExplosionController oppGrenadeExplosionController;

    public SelfScoreController selfScoreController;

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

    public void ReduceHealth(int hpToReduce)
    {
        if (!oppGrenadeExplosionController.GetIsGrenadeThrown()
                && !selfShieldOverlayController.GetIsShowingShield())
        {
            if (selfShieldOverlayController.GetIsShieldResetHalfway())
            {
                healthRemaining = Math.Max(healthRemaining +
                        selfShieldOverlayController.GetHpToReduceAfterShieldProtection(), 0);
                selfShieldOverlayController.ResetIsShieldResetHalfway();

                if (healthRemaining == 0)
                {
                    healthRemaining = MAX_HEALTH;
                    bulletController.ResetBulletsRemaining();
                    grenadeController.ResetGrenadesRemaining();
                    selfShieldController.ResetShieldsRemaining();
                    selfScoreController.IncrementNumOfDeaths();
                }
                return;
            }

            Debug.Log("Reducingggg Health");
            healthRemaining = Math.Max(healthRemaining - hpToReduce, 0);

            if (healthRemaining == 0)
            {
                healthRemaining = MAX_HEALTH;
                bulletController.ResetBulletsRemaining();
                grenadeController.ResetGrenadesRemaining();
                selfShieldController.ResetShieldsRemaining();
                selfScoreController.IncrementNumOfDeaths();
            }
        }
    }

    public int GetHealthRemaining()
    {
        return healthRemaining;
    }

    public void SetHealthRemaining(int hp)
    {
        healthRemaining = hp;
    }
}
