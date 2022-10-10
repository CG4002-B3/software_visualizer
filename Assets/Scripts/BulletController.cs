using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    private int MAX_NUM_OF_BULLETS = 6;
    private float RELOAD_TIME = 0.5f;
    private int BULLET_DAMAGE = 10;

    private Text bullets;
    private int bulletsRemaining;

    public Animator animator;

    public AudioSource shootingSound;
    public AudioSource reloadingSound;

    public ParticleSystem bulletSmoke;

    public OppHealthBarController oppHealthBarController;
    public OppShieldController oppshieldController;

    private bool isReloading = false;
    private bool startReloading = false;

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
        startReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        bullets.text = bulletsRemaining.ToString() + " / " + MAX_NUM_OF_BULLETS;
        if (isReloading)
        {
            return;
        }

        if (startReloading)
        {
            StartCoroutine(Reload());
            return;
        }
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
            oppshieldController.ReduceShieldHp(BULLET_DAMAGE);
            oppHealthBarController.ReduceHealth(BULLET_DAMAGE);
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
        isReloading = false;
        startReloading = false;
    }

    public int GetBulletsRemaining() {
        return bulletsRemaining;
    }

    public void ResetBulletsRemaining()
    {
        bulletsRemaining = MAX_NUM_OF_BULLETS;
    }

    public void SetBulletsRemaining(int bullets, bool isValidShoot)
    {
        if (isValidShoot)
        {
            shootingSound.Play();
            bulletSmoke.Play();
        }

        bulletsRemaining = bullets;
    }

    public void StartReloading(bool isValidReload)
    {
        if (isValidReload)
        {
            startReloading = true;
        }
    }
}
