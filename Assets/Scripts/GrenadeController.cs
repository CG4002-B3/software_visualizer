using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeController : MonoBehaviour
{
    private int MAX_NUM_OF_GRENADES = 2;
    private int GRENADE_DAMAGE = 30;

    public int grenadesRemaining;
    public Image[] grenades;
    public ExplosionController explosionController;

    public OppShieldController oppshieldController;

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
        if (!explosionController.GetIsGrenadeThrown())
        {
            grenadesRemaining = Math.Max(grenadesRemaining - 1, 0);
            oppshieldController.ReduceShieldHp(GRENADE_DAMAGE);
        }
    }

    public void ResetGrenadesRemaining()
    {
        grenadesRemaining = MAX_NUM_OF_GRENADES;
    }
}
