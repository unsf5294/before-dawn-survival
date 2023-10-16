using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Canvas combatUI;
    public TMP_Text infoText;

    private void Start()
    {
        combatUI.enabled = true;
        infoText.enabled = true;
        StartCoroutine(ShowAndHideInfoText());
    }

    IEnumerator ShowAndHideInfoText()
    {
        string[] messages = {
            "press 'W' 'A' 'S' 'D' to move",
            "press 'J' 'K' to attack",
            "kill MONSTERS to heal",
            "use 'Q' 'E' to switch views"
        };

        foreach (string message in messages)
        {
            infoText.text = message;
            infoText.enabled = true;

            // Wait for 5 seconds
            yield return new WaitForSeconds(3f);

            infoText.enabled = false;

            // gap between hiding one message and showing the next
            yield return new WaitForSeconds(1f);
        }
    }
}