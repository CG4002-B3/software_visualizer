using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private int GRENADE_DELAY_TIME = 2;

    public ParticleSystem explosionParticles;
    public AudioSource grenadeExplosionSound;

    private bool isGrenadeThrown;
    private bool shouldIndicateExplosion;
    private bool isWaitingForDelay;

    public Animator animator;

    void Start ()
    {
        isGrenadeThrown = false;
        isWaitingForDelay = false;
        explosionParticles.Stop();
        explosionParticles.Clear();
        grenadeExplosionSound.Stop();
    }

    void OnEnable()
    {
        animator.SetBool("IsSelfGrenadeThrown", false);
    }

    void Update()
    {
        if (isWaitingForDelay)
        {
            return;
        }

        if (isGrenadeThrown)
        {
            StartCoroutine(ExplodeGrenade());
        }
    }

    public void ExplosionButtonPress()
    {
        isGrenadeThrown = true;
    }

    IEnumerator ExplodeGrenade()
    {
        isWaitingForDelay = true;
        animator.SetBool("IsSelfGrenadeThrown", true);
        yield return new WaitForSeconds(GRENADE_DELAY_TIME);
        animator.SetBool("IsSelfGrenadeThrown", false);
        explosionParticles.Play();
        grenadeExplosionSound.Play();
        isGrenadeThrown = false;
        isWaitingForDelay = false;
    }

    public bool GetIsGrenadeThrown()
    {
        return isGrenadeThrown;
    }
}
