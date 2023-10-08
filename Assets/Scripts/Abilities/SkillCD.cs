using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconFade : MonoBehaviour
{
    public Image skillIcon;  // 
    public KeyCode activationKey = KeyCode.Alpha1;
    public float fadeDuration = 5.0f; // 
    private Color originalColor;

    private void Start()
    {
        if (skillIcon == null)
        {
            Debug.LogError("Skill Icon is not set!");
            return;
        }
        originalColor = skillIcon.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey)) // when a key is pressed
        {
            StartCoroutine(FadeOutAndIn());
        }
    }

    private IEnumerator FadeOutAndIn()
    {
        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration / 2)
        {
            skillIcon.color = Color.Lerp(originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 0.25f), elapsed / (fadeDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }
        skillIcon.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        // Reset elapsed time
        elapsed = 0f;

        // Fade in
        while (elapsed < fadeDuration / 2)
        {
            skillIcon.color = Color.Lerp(new Color(originalColor.r, originalColor.g, originalColor.b, 0.25f), originalColor, elapsed / (fadeDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }
        skillIcon.color = originalColor;
    }
}
