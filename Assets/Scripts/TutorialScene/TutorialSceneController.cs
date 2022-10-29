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
    public VideoPlayer shieldTutorial;
    public RawImage videoTexture;
    public Image backgroundImage;

    private float NARRATOR_ANIMATOR_DURATION = 3f;
    private float NARRATOR_TRANSITION_OFFSET = 0.5f;

    private bool showingMsg = false;
    private bool welcomeMsg = true;
    private bool showTutorial1 = false;
    private bool showTutorial2 = false;

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

        if (showTutorial1)
        {
            StartCoroutine(playTutorial1());
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

        wordFadingEffect.SetBool("showNarrator", false);
        showTutorial1 = true;
    }

    IEnumerator playTutorial1()
    {
        showingMsg = true;

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
        // yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        showTutorial1 = false;
        showingMsg = false;
    }

    // IEnumerator playTutorial2()
    // {
    //     showingMsg = true;

    //     narratorWords.text = "Here's how to defense yourself out there";
    //     wordFadingEffect.SetBool("showNarrator", true);
    //     yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
    //     wordFadingEffect.SetBool("showNarrator", false);
    //     yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

    //     DarkenBackground();
    //     videoTexture.enabled = true;
    //     shieldTutorial.Play();
    //     yield return new WaitForSeconds((float)shieldTutorial.length);
    //     LightenBackground();

    //     videoTexture.enabled = false;
    //     showTutorial2 = false;
    //     showingMsg = false;
    // }

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
