using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;  // Use TextMeshProUGUI instead of Text
    public float timerDuration = 300.0f;  // 5 minutes in seconds

    private float currentTime;

    private void Start()
    {
        currentTime = timerDuration;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (currentTime > 0)
        {
            // Calculate minutes and seconds from the current time
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
        }

        countdownText.text = "00:00";
        TimerFinished();
    }

    void TimerFinished()
    {
        // This function gets called when the timer hits 0.
        // You can add any game over or timer-related logic here.
        Debug.Log("Timer has finished!");
    }
}