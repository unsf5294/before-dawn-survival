using UnityEngine;

public class ShrineBehavior : MonoBehaviour
{
    public Light shrineLight;            // Drag your shrine's light here in the Inspector
    public float blinkInterval = 0.5f;   // Time between light blinks
    public float timeToVanish = 20f;     // Time after which the light disappears

    private bool playerInRange = false;
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            timer = 0f; // Reset timer
            InvokeRepeating("ToggleLight", 0, blinkInterval);
            Invoke("TurnOffLight", timeToVanish);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CancelInvoke("ToggleLight");
            shrineLight.enabled = true; // Ensure the light is turned on if the player exits early
        }
    }

    private void ToggleLight()
    {
        shrineLight.enabled = !shrineLight.enabled;
    }

    private void TurnOffLight()
    {
        CancelInvoke("ToggleLight"); // Stop the blinking
        shrineLight.enabled = false;
    }
}
