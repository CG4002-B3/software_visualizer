using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldOverlayController : MonoBehaviour
{
    private float SHIELD_DELAY = 10f;

    public SpriteRenderer shieldOverlay;
    public Text shieldCountdown;

    private bool shouldShowShield;
    private bool isShowingShield;
    private float shieldTimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        shouldShowShield = false;
        isShowingShield = false;
        shieldTimeRemaining = SHIELD_DELAY;

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
        if (!isShowingShield)
        {
            shieldTimeRemaining = SHIELD_DELAY;
            shouldShowShield = true;
        }
    }

    IEnumerator ShowShield()
    {
        isShowingShield = true;
        yield return new WaitForSeconds(SHIELD_DELAY);
        shouldShowShield = false;
        isShowingShield = false;
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
}
