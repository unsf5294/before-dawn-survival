using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth; // Reference to the player's health script
    public RectTransform sliderRectTransform; // The RectTransform of the health slider
    public Image sliderFillImage; 

    private Color originalColor; 
    private Color darkenedColor; 
    private float lastHealth; 
    private Vector3 initialPosition; 

    private void Start()
    {
        if (healthSlider == null || sliderRectTransform == null)
        {
            Debug.LogError("Slider component or RectTransform is not set!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth reference is not set!");
            return;
        }

        // Set the max value of the slider to match the player's max health
        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.CurrentHealth;
        lastHealth = playerHealth.CurrentHealth; // Initialize lastHealth at the start

        initialPosition = sliderRectTransform.localPosition; // Record the initial position at the start

        if (sliderFillImage != null)
        {
            originalColor = sliderFillImage.color;
            darkenedColor = new Color(
                originalColor.r * 0.9f, 
                originalColor.g * 0.9f, 
                originalColor.b * 0.9f, 
                originalColor.a * 0.7f); 
        }
    }

    private void Update() // Consider using Update instead of LateUpdate for UI
    {
        if (playerHealth != null)
        {
            // Update the slider's value to match the player's current health
            healthSlider.value = playerHealth.CurrentHealth;

            if (playerHealth.CurrentHealth < lastHealth)
            {
                StartCoroutine(ShakeSlider(0.25f)); // Shake for 0.25 seconds
            }

            // Update lastHealth for the next frame
            lastHealth = playerHealth.CurrentHealth;
        }
    }

    IEnumerator ShakeSlider(float duration)
    {
        float elapsed = 0.0f;

        if (sliderFillImage != null)
        {
            sliderFillImage.color = darkenedColor; 
        }

        while (elapsed < duration)
        {
            float x = initialPosition.x + Random.Range(-1f, 1f) * 1f; // Varying the x position slightly
            float y = initialPosition.y + Random.Range(-1f, 1f) * 1f; // Varying the y position slightly

            sliderRectTransform.localPosition = new Vector3(x, y, initialPosition.z);

            elapsed += Time.deltaTime;

            yield return null; // wait until next frame
        }

        if (sliderFillImage != null)
        {
            sliderFillImage.color = originalColor; 
        }

        sliderRectTransform.localPosition = initialPosition; 
    }
}
