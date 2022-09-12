using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OppShieldController : MonoBehaviour
{
    private float SHIELD_DELAY = 10f;
    private int MAX_SHIELD_HP = 30;

    public GameObject oppShieldPrefab;

    private bool shouldShowShield;
    private bool isShieldResetHalfway;
    private float shieldTimeRemaining;
    private int shieldHp;

    // Start is called before the first frame update
    void Start()
    {
        oppShieldPrefab.SetActive(false);
        shouldShowShield = false;
        isShieldResetHalfway = false;
        shieldTimeRemaining = SHIELD_DELAY;
        shieldHp = MAX_SHIELD_HP;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Opponent Shield HP: " + shieldHp);
        oppShieldPrefab.SetActive(shouldShowShield);

        if (shouldShowShield){
            if (shieldTimeRemaining >= 0)
            {
                shieldTimeRemaining -= Time.deltaTime;
            }
            else
            {
                shouldShowShield = false;
                shieldTimeRemaining = SHIELD_DELAY;
                shieldHp = MAX_SHIELD_HP;
                isShieldResetHalfway = false;
            }
        }
    }

    public void ActivateShield()
    {
        if (!shouldShowShield)
        {
            shouldShowShield = true;
            isShieldResetHalfway = false;
        }
    }

    public void ReduceShieldHp(int shieldHpToReduce)
    {
        if (shouldShowShield)
        {
            shieldHp = Math.Max(shieldHp - shieldHpToReduce, 0);
            if (shieldHp == 0)
            {
                isShieldResetHalfway = true;
                shouldShowShield = false;
                shieldHp = MAX_SHIELD_HP;
                shieldTimeRemaining = SHIELD_DELAY;
            }
        }
    }

    public bool GetShouldShowShield()
    {
        return shouldShowShield;
    }

    public bool GetIsShieldResetHalfway()
    {
        return isShieldResetHalfway;
    }

    public void ResetIsShieldResetHalfway()
    {
        isShieldResetHalfway = false;
    }
}
