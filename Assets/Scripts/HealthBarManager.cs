using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Slider healthSlider; 
    public PlayerHealth playerHealth; // Reference to the player's health script

    private void Start()
    {
        if (healthSlider == null)
        {
            Debug.LogError("Slider component is not set!");
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
    }

    private void LateUpdate()
    {
        if (playerHealth != null)
        {
            // Update the slider's value to match the player's current health
            healthSlider.value = playerHealth.CurrentHealth;
        }
    }
}
