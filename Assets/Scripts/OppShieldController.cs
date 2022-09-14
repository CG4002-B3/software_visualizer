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
    private bool isNextShieldReady;
    private bool isShowingShield;

    // Start is called before the first frame update
    void Start()
    {
        oppShieldPrefab.SetActive(false);
        shouldShowShield = false;
        isShieldResetHalfway = false;
        shieldTimeRemaining = SHIELD_DELAY;
        shieldHp = MAX_SHIELD_HP;
        isNextShieldReady = true;
        isShowingShield = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Opponent Shield HP: " + shieldHp);
        oppShieldPrefab.SetActive(shouldShowShield);

        if (isShowingShield)
        {
            if (shieldTimeRemaining >= 0)
            {
                shieldTimeRemaining -= Time.deltaTime;
            }
            return;
        }
        if (shouldShowShield)
        {
            StartCoroutine(ShowShield());
            return;
        }
    }

    public void ActivateShield()
    {
        if (!isShowingShield & isNextShieldReady)
        {
            shieldTimeRemaining = SHIELD_DELAY;
            shouldShowShield = true;
            isShieldResetHalfway = false;
        }
    }

    IEnumerator ShowShield()
    {
        isNextShieldReady = false;
        isShowingShield = true;
        yield return new WaitForSeconds(SHIELD_DELAY);
        isShieldResetHalfway = false;
        shouldShowShield = false;
        isShowingShield = false;
        isNextShieldReady = true;
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
                isShowingShield = false;
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

    public void ResetIsNextShieldReady()
    {
        isNextShieldReady = true;
    }

    public bool GetIsNextShieldReady()
    {
        return isNextShieldReady;
    }
}
