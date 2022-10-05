using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfShieldController : MonoBehaviour
{
    private int MAX_NUM_OF_SHIELDS = 3;

    public int shieldsRemaining;
    public Image[] shields;
    public SelfShieldOverlayController selfShieldOverlayController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i < shieldsRemaining)
            {
                shields[i].enabled = true;
            }
            else
            {
                shields[i].enabled = false;
            }
        }
    }

    public void ReduceShield()
    {
        if (!selfShieldOverlayController.GetIsShowingShield()
                && selfShieldOverlayController.GetIsNextShieldReady())
        {
            shieldsRemaining = Math.Max(shieldsRemaining - 1, 0);
        }
    }

    public void ResetShieldsRemaining()
    {
        shieldsRemaining = MAX_NUM_OF_SHIELDS;
        selfShieldOverlayController.ResetIsNextShieldReady();
    }

    public void SetShieldRemaining(int shields)
    {
        shieldsRemaining = shields;
    }
}
