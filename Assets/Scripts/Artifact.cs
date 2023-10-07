using UnityEngine;
using System.Collections;

public class Artifact : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light shrineLight;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 2f;
    [SerializeField] private float intensityChangeRate = 0.5f; // Rate at which light intensity changes.

    [Header("Healing Settings")]
    [SerializeField] private float timeToHeal = 5.0f;
    [SerializeField] private int healAmount = 20;

    private bool isPlayerNearby = false;
    private bool shrineConsumed = false; // Flag to check if shrine is already consumed

    private void Start()
    {
        shrineLight.intensity = minIntensity;
    }

    private void Update()
    {
        if (!shrineConsumed)
        {
            if (isPlayerNearby)
                IntensifyLight();
            else
                DimLight();
        }
    }

    private void IntensifyLight()
    {
        shrineLight.intensity = Mathf.MoveTowards(shrineLight.intensity, maxIntensity, intensityChangeRate * Time.deltaTime);
    }

    private void DimLight()
    {
        shrineLight.intensity = Mathf.MoveTowards(shrineLight.intensity, minIntensity, intensityChangeRate * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!shrineConsumed && other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            StartCoroutine(CheckHealPlayer(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            StopCoroutine(CheckHealPlayer(other.gameObject));
        }
    }

    private IEnumerator CheckHealPlayer(GameObject playerObj)
    {
        yield return new WaitForSeconds(timeToHeal);

        if (isPlayerNearby && !shrineConsumed) // If player is still near the shrine after the duration and the shrine is not consumed
        {
            PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                playerHealth.AddHealth(healAmount);
            }
            isPlayerNearby = false; 
            shrineConsumed = true; // Mark the shrine as consumed
            shrineLight.intensity = 0; // Turn off the light
        }
    }
}
