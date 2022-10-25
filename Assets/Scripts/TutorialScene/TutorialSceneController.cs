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

    private float NARRATOR_ANIMATOR_DURATION = 3f;
    private float NARRATOR_TRANSITION_OFFSET = 0.5f;

    private bool showingMsg = false;
    private bool welcomeMsg = true;
    private bool showTutorial1 = false;

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

        showTutorial1 = true;
    }

    IEnumerator playTutorial1()
    {
        videoTexture.enabled = true;
        showingMsg = true;
        shieldTutorial.Play();

        yield return new WaitForSeconds((float)shieldTutorial.length);

        videoTexture.enabled = false;
        showTutorial1 = false;
        showingMsg = false;
    }
}
