using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfShieldOverlayController : MonoBehaviour
{
    private float SHIELD_DELAY = 10f;
    private int MAX_SHIELD_HP = 30;

    public Image shieldOverlay;
    public Text shieldCountdown;

    private int shieldHp;
    private bool isShieldResetHalfway;
    private bool isNextShieldReady;

    private bool shouldShowShield;
    private bool isShowingShield;
    private float shieldTimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        shouldShowShield = false;
        isShowingShield = false;
        shieldTimeRemaining = SHIELD_DELAY;
        shieldHp = MAX_SHIELD_HP;
        isShieldResetHalfway = false;
        isNextShieldReady = true;

        shieldOverlay.enabled = shouldShowShield;
    }

    // Update is called once per frame
    void Update()
    {
        shieldOverlay.enabled = shouldShowShield;
        shieldCountdown.enabled = shouldShowShield;

        if (isShowingShield)
        {
            if (shieldTimeRemaining >= 0)
            {
                shieldTimeRemaining -= Time.deltaTime;
                DisplayCountdown(shieldTimeRemaining);
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
        if (isShowingShield)
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

    void DisplayCountdown(float shieldTimeRemaining)
    {
        shieldTimeRemaining += 1;
        float seconds = Mathf.FloorToInt(shieldTimeRemaining % 60);
        if (seconds > 1)
        {
            shieldCountdown.text = string.Format("Shield Countdown\n{0:0} seconds", seconds);
        }
        else
        {
            shieldCountdown.text = string.Format("Shield Countdown\n{0:0} second", seconds);
        }
    }

    public bool GetIsShowingShield()
    {
        return isShowingShield;
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
