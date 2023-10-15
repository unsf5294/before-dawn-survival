using UnityEngine;

public class LightBlink : MonoBehaviour
{
    [SerializeField] private float lowIntensity = 0.0f;
    [SerializeField] private float highIntensity = 1.5f;
    [SerializeField] private float blinkDuration = 1.0f;
    [SerializeField] private float breathingSpeed = 0.3f;

    [SerializeField] private Light pointLight;

    private void Start()
    {
        pointLight = GetComponent<Light>();
        StartCoroutine(BreathingCycle());
    }

    private System.Collections.IEnumerator BreathingCycle()
    {
        while (true)
        {
            // Blink twice with high intensity
            
            for (int i = 0; i < 2; i++)
            {
                pointLight.intensity = highIntensity;
                yield return new WaitForSeconds(blinkDuration);
                pointLight.intensity = lowIntensity; 
                yield return new WaitForSeconds(blinkDuration);
            }

            float startTime = Time.time;
            while ((Time.time - startTime) < (Mathf.PI / (2 * breathingSpeed)))
            {
                float t = (Time.time - startTime) * breathingSpeed;
                pointLight.intensity = Mathf.Lerp(highIntensity, lowIntensity, Mathf.Sin(t));
                yield return null;
            }
        }
    }
}
