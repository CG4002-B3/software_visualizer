using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeController : MonoBehaviour
{
    private int MAX_NUM_OF_GRENADES = 2;

    public int grenadesRemaining;
    public Image[] grenades;
    public Sprite grenadeVisible;

    void Update()
    {
        for (int i = 0; i < grenades.Length; i++)
        {
            if (i < grenadesRemaining)
            {
                grenades[i].enabled = true;
            }
            else
            {
                grenades[i].enabled = false;
            }
        }
    }

    public void ReduceGrenades()
    {
        grenadesRemaining = Math.Max(grenadesRemaining - 1, 0);
        if (grenadesRemaining == 0)
        {
            grenadesRemaining = MAX_NUM_OF_GRENADES;
        }
    }
}
