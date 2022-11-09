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
    public VideoPlayer exitTutorial;
    public RawImage videoTexture;
    public Image backgroundImage;
    public MqttTutorialScene mqttTutorialScene;
    public InvalidActionFeedbackController invalidActionFeedbackController;
    public SceneChanger sceneChanger;

    private string selfAction;
    private string selfIdString;

    private float NARRATOR_ANIMATOR_DURATION = 3f;
    private float NARRATOR_TRANSITION_OFFSET = 0.5f;

    private bool showingMsg = false;
    private bool welcomeMsg = true;
    private bool showShootTutorial = false;
    private bool showReloadTutorial = false;
    private bool showGrenadeTutorial = false;
    private bool showShieldTutorial = false;
    private bool showExitTutorial = false;

    void Start()
    {
        videoTexture.enabled = false;
    }

    void Update()
    {
        selfAction = mqttTutorialScene.getSelfAction();
        selfIdString = mqttTutorialScene.getSelfIdString();
        bool justDecodedData = mqttTutorialScene.getJustDecodedData();

        if (showingMsg)
        {
            Debug.Log("[ACTION] " + selfAction + " " + selfIdString);
            if (showReloadTutorial)
            {
                if (selfAction == "reload")
                {
                    showReloadTutorial = false;
                    showingMsg = false;
                    showGrenadeTutorial = true;
                }
                else if (justDecodedData)
                {
                    invalidActionFeedbackController.SetFeedback("Invalid reload action.\nWhat a disappointment. Do it again!");
                }
            }
            else if (showGrenadeTutorial)
            {
                if (selfAction == "grenade")
                {
                    showGrenadeTutorial = false;
                    showingMsg = false;
                    showShieldTutorial = true;
                }
                else if (justDecodedData)
                {
                    invalidActionFeedbackController.SetFeedback("Invalid grenade action.\nWhat a disappointment. Do it again!");
                }
            }
            else if (showShieldTutorial)
            {
                if (selfAction == "shield")
                {
                    showShieldTutorial = false;
                    showingMsg = false;
                    showExitTutorial = true;
                }
                else if (justDecodedData)
                {
                    invalidActionFeedbackController.SetFeedback("Invalid shield action.\nWhat a disappointment. Do it again!");
                }
            }
            else if (showExitTutorial)
            {
                if (selfAction == "logout")
                {
                    mqttTutorialScene.PublishFinishTutorial();
                    showExitTutorial = false;
                    showingMsg = false;
                    sceneChanger.ChangeScene("GameScene");
                }
                else if (justDecodedData)
                {
                    invalidActionFeedbackController.SetFeedback("Invalid logout action.\nWhat a disappointment. Do it again!");
                }
            }
            mqttTutorialScene.setJustDecodedData(false);
            return;
        }

        if (welcomeMsg)
        {
            StartCoroutine(showWelcomeMessage());
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

        if (showExitTutorial)
        {
            StartCoroutine(playExitTutorial());
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

        // showGrenadeTutorial = false;
        // showingMsg = false;

        // showShieldTutorial = true;
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
    }

    IEnumerator playExitTutorial()
    {
        showingMsg = true;

        LightenBackground();
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET * 1.5f);

        narratorWords.text = "What to do when you see no hope?";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        narratorWords.text = "You quit";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION);
        wordFadingEffect.SetBool("showNarrator", false);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        DarkenBackground();
        videoTexture.enabled = true;
        exitTutorial.Play();
        yield return new WaitForSeconds((float)exitTutorial.length);
        yield return new WaitForSeconds(NARRATOR_TRANSITION_OFFSET);

        TransparentBackground();

        videoTexture.enabled = false;

        narratorWords.text = "Try quitting";
        wordFadingEffect.SetBool("showNarrator", true);
        yield return new WaitForSeconds(NARRATOR_ANIMATOR_DURATION - 0.2f);
        wordFadingEffect.SetBool("showNarrator", false);
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
