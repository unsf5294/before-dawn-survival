using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputListener : MonoBehaviour
{
    public Animator uiAnimator;

    private bool animationStarted;

    void Start()
    {
        animationStarted = false;

        if (uiAnimator != null)
        {

            uiAnimator.ResetTrigger("StartAnimation");
        }
        else
        {
            Debug.LogError("Animator component is not attached in the inspector.");
        }
    }

    void Update()
    {
        if  (!animationStarted && Input.anyKeyDown)
        {
            animationStarted = true;
            StartUIAnimation();
        }
    }

    void StartUIAnimation()
    {
        StartCoroutine(StartAnimationAfterDelay(0.1f));
    }

    IEnumerator StartAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (uiAnimator != null) 
        {
            uiAnimator.SetTrigger("StartAnimation");
        }
        else
        {
            Debug.LogError("Animator component is not attached in the inspector.");
        }
    }
}
