using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    private int MAX_NUM_OF_BULLETS = 6;
    private float RELOAD_TIME = 1f;

    private Text bullets;
    private int bulletsRemaining;

    public Animator animator;

    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletsRemaining = MAX_NUM_OF_BULLETS;
        bullets = GetComponent<Text>();
    }

    void OnEnable()
    {
        animator.SetBool("Reloading", false);
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (bulletsRemaining <= -1)
        {
            StartCoroutine(Reload());
            return;
        }

        bullets.text = bulletsRemaining.ToString() + " / " + MAX_NUM_OF_BULLETS;
    }

    public void ReduceBullets()
    {
        if (isReloading)
        {
            return;
        }
        bulletsRemaining = Math.Max(bulletsRemaining - 1, -1);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(RELOAD_TIME);
        animator.SetBool("Reloading", false);
        bulletsRemaining = MAX_NUM_OF_BULLETS;
        isReloading = false;
    }
}
