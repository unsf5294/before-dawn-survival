using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Slider healthSlider;  // 
    public float decreaseAmount = 10f;  // 
    public float decreaseDuration = 5f; // 

    private void Start()
    {
        if (healthSlider == null)
        {
            Debug.LogError("Slider component is not set!");
            return;
        }

        // 开始协程
        StartCoroutine(DecreaseHealthOverTime());
    }

    private IEnumerator DecreaseHealthOverTime()
    {
        while (healthSlider.value > 0)
        {
            float startValue = healthSlider.value;
            float endValue = Mathf.Clamp(healthSlider.value - decreaseAmount, 0, healthSlider.maxValue);

            float elapsed = 0f;
            while (elapsed < decreaseDuration)
            {
                float newValue = Mathf.Lerp(startValue, endValue, elapsed / decreaseDuration);
                healthSlider.value = newValue;

                elapsed += Time.deltaTime;
                yield return null;
            }

            healthSlider.value = endValue;

            yield return new WaitForSeconds(decreaseDuration);
        }
    }
}
