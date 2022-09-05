using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    private float RELOAD_TIME = 1f;

    private Text bullets;
    private int bulletsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        bulletsRemaining = 6;
        bullets = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        bullets.text = bulletsRemaining.ToString() + " / 6";
    }

    public void ReduceBullets()
    {
        bulletsRemaining = Math.Max(bulletsRemaining - 1, -1);
        if (bulletsRemaining == -1)
        {
            bulletsRemaining = 6;
        }
    }
}
