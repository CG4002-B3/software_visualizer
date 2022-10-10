using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvalidActionFeedbackController : MonoBehaviour
{
    private float SHOW_FEEDBACK_DURATION = 3f;
    public Text invalidActionFeedback;
    private string feedback;

    private bool showingFeedback;

    // Start is called before the first frame update
    void Start()
    {
        showingFeedback = false;
        ClearFeedback();
    }

    void Update()
    {
        invalidActionFeedback.text = feedback;

        if (showingFeedback)
        {
            return;
        }

        if (feedback != "")
        {
            StartCoroutine(ShowFeedback());
            return;
        }
    }

    public void ClearFeedback()
    {
        feedback = "";
    }

    public void SetFeedback(string feedbackText)
    {
        feedback = feedbackText;
    }

    IEnumerator ShowFeedback()
    {
        showingFeedback = true;
        yield return new WaitForSeconds(SHOW_FEEDBACK_DURATION);
        ClearFeedback();
        showingFeedback = false;
    }
}
