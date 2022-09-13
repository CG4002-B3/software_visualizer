using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppGrenadeExplosionController : MonoBehaviour
{
    private int GRENADE_DAMAGE = 30;
    private float GRENADE_DELAY_TIME = 2f;
    private float GRENADE_PROJECTION_OFFSET = 1f;

    public ParticleSystem explosionParticles;
    public AudioSource grenadeExplosionSound;

    private bool isGrenadeThrown;
    private bool shouldIndicateExplosion;
    private bool isWaitingForDelay;

    public Animator animator;
    public SelfHealthBarController selfHeathBarController;
    public SelfShieldOverlayController selfShieldOverlayController;

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
        animator.SetBool("IsOppGrenadeThrown", false);
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
            return;
        }
    }

    public void ExplosionButtonPress()
    {
        if (!isGrenadeThrown)
        {
            selfShieldOverlayController.ReduceShieldHp(GRENADE_DAMAGE);
            selfHeathBarController.ReduceHealth(GRENADE_DAMAGE);
            isGrenadeThrown = true;
        }
    }

    IEnumerator ExplodeGrenade()
    {
        isWaitingForDelay = true;
        animator.SetBool("IsOppGrenadeThrown", true);
        yield return new WaitForSeconds(GRENADE_DELAY_TIME - GRENADE_PROJECTION_OFFSET);
        animator.SetBool("IsOppGrenadeThrown", false);
        yield return new WaitForSeconds(GRENADE_PROJECTION_OFFSET);
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
