using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    private int MAX_NUM_OF_BULLETS = 6;
    private float RELOAD_TIME = 0.5f;

    private Text bullets;
    private int bulletsRemaining;

    public Animator animator;

    public AudioSource shootingSound;
    public AudioSource reloadingSound;

    public ParticleSystem bulletSmoke;

    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletsRemaining = MAX_NUM_OF_BULLETS;
        bullets = GetComponent<Text>();

        shootingSound.Stop();
        reloadingSound.Stop();

        bulletSmoke.Stop();
        bulletSmoke.Clear();
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
        if (bulletsRemaining > 0)
        {
            shootingSound.Play();
            bulletSmoke.Play();
        }
        bulletsRemaining = Math.Max(bulletsRemaining - 1, -1);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reloading", true);
        reloadingSound.Play();
        yield return new WaitForSeconds(RELOAD_TIME);
        animator.SetBool("Reloading", false);
        bulletsRemaining = MAX_NUM_OF_BULLETS;
        isReloading = false;
    }
}
