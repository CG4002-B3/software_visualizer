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

    void Start ()
    {
        isGrenadeThrown = false;
        isWaitingForDelay = false;
        explosionParticles.Stop();
        explosionParticles.Clear();
        grenadeExplosionSound.Stop();
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
        yield return new WaitForSeconds(GRENADE_DELAY_TIME);
        explosionParticles.Play();
        grenadeExplosionSound.Play();
        isGrenadeThrown = false;
        isWaitingForDelay = false;
    }
}
