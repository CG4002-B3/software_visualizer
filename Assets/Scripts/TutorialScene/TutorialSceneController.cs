using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialSceneController : MonoBehaviour
{
    public Animator wordFadingEffect;
    public Text narratorWords;
    public VideoPlayer reloadTutorial;
    public VideoPlayer grenadeTutorial;
    public VideoPlayer shieldTutorial;
    public RawImage videoTexture;
    public Image backgroundImage;

    private float NARRATOR_ANIMATOR_DURATION = 3f;
    private float NARRATOR_TRANSITION_OFFSET = 0.5f;

    private bool showingMsg = false;
    private bool welcomeMsg = true;
    private bool showShootTutorial = false;
    private bool showReloadTutorial = false;
    private bool showGrenadeTutorial = false;
    private bool showShieldTutorial = false;

    void Start()
    {
        videoTexture.enabled = false;
    }

    void Update()
    {
        if (showingMsg)
        {
            return;
        }

        if (welcomeMsg)
        {
            StartCoroutine(showWelcomeMessage());
        }

        if (showShootTutorial)
        {
            StartCoroutine(playShootTutorial());
        }

        if (showReloadTutorial)
        {
            StartCoroutine(playReloadTutorial());
        }

        if (showGrenadeTutorial)
        {
            StartCoroutine(playGrenadeTutorial());
        }

        if (showShieldTutorial)
        {
            StartCoroutine(playShieldTutorial());
        }
    }

    IEnumerator showWelcomeMessage()
    {
        showingMsg = true;

        narratorWords.text = "Welcome Aboard Soldier";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        narratorWords.text = "Introduce Mr. R - The Destroyer";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        welcomeMsg = false;
        showingMsg = false;

        showShootTutorial = true;
    }

    IEnumerator playShieldTutorial()
    {
        showingMsg = true;

        LightenBackground();
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET * 1.5f);

        narratorWords.text = "Here's how to defense yourself out there";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        DarkenBackground();
        videoTexture.enabled = true;
        shieldTutorial.Play();
        yield return new WaitForSeconds((float)shieldTutorial.length);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        TransparentBackground();

        videoTexture.enabled = false;

        narratorWords.text = "Now it's your turn to shield";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION - 0.2f);
        wordFadingEffect.SetBool("showNarrator", false);

        showShieldTutorial = false;
        showingMsg = false;
    }

    IEnumerator playShootTutorial()
    {
        showingMsg = true;

        narratorWords.text = "I'll teach you how to aim properly";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET * 1.5f);

        TransparentBackground();

        narratorWords.text = "Aim at your enemy's chest";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        showShootTutorial = false;
        showingMsg = false;

        showReloadTutorial = true;
    }

    IEnumerator playReloadTutorial()
    {
        showingMsg = true;

        LightenBackground();
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET * 1.5f);

        narratorWords.text = "What to do when the gun cannot shoot?";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        narratorWords.text = "You reload!";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        DarkenBackground();
        videoTexture.enabled = true;
        reloadTutorial.Play();
        yield return new WaitForSeconds((float)reloadTutorial.length);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        TransparentBackground();

        videoTexture.enabled = false;

        narratorWords.text = "Try reloading the gun";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION - 0.2f);
        wordFadingEffect.SetBool("showNarrator", false);

        showReloadTutorial = false;
        showingMsg = false;

        showGrenadeTutorial = true;
    }

    IEnumerator playGrenadeTutorial()
    {
        showingMsg = true;

        LightenBackground();
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET * 1.5f);

        narratorWords.text = "Want a faster way to attack?";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        narratorWords.text = "Use grenade";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        DarkenBackground();
        videoTexture.enabled = true;
        grenadeTutorial.Play();
        yield return new WaitForSeconds((float)grenadeTutorial.length);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        TransparentBackground();

        videoTexture.enabled = false;

        narratorWords.text = "Throw the grenadeeeeeee";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION - 0.2f);
        wordFadingEffect.SetBool("showNarrator", false);

        showGrenadeTutorial = false;
        showingMsg = false;

        showShieldTutorial = true;
    }

    private void DarkenBackground()
    {
        backgroundImage.color = new Color(0f, 0f, 0f, 1f);
    }

    private void LightenBackground()
    {
        backgroundImage.color = new Color(0.76470588235f, 0.06666666666f, 0.61568627451f, 1f);
    }

    private void TransparentBackground()
    {
        backgroundImage.color = new Color(0f, 0f, 0f, 0f);
    }
}
