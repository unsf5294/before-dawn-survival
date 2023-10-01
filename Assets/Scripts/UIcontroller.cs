using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Canvas menuUI;
    public Canvas combatUI;

    private void Start()
    {
        menuUI.enabled = true;
        combatUI.enabled = false;
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
        //SceneManager.LoadScene("StartScene");
    }

    public void OpenSettings()
    {
        // SceneManager.LoadScene("SettingScene");
    }
}
