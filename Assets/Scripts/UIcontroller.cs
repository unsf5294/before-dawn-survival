using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Canvas menuUI;
    public Canvas combatUI;
    public TMP_Text infoText;

    private bool hasShownInfoText = false;  

    private void Start()
    {
        menuUI.enabled = true;
        combatUI.enabled = false;
        infoText.enabled = false;  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenSettings();
        }
    }

    public void StartGame()
    {
        menuUI.enabled = false;
        combatUI.enabled = true;

        if (!hasShownInfoText)
        {
            StartCoroutine(ShowAndHideInfoText());
            hasShownInfoText = true;  
        }

        //SceneManager.LoadScene("StartScene");
    }

    public void OpenSettings()
    {
        // SceneManager.LoadScene("SettingScene");
    }

    IEnumerator ShowAndHideInfoText()
    {
        string[] messages = {
            "press 'W' 'A' 'S' 'D' to move",
            "press 'J' 'K' to attack",
            "kill MONSTERS to heal",
            "find ARTIFACTS to regain faith"
        };

        foreach (string message in messages)
        {
            infoText.text = message;
            infoText.enabled = true;

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            infoText.enabled = false;

            // gap between hiding one message and showing the next
            yield return new WaitForSeconds(1f);
        }
    }
}