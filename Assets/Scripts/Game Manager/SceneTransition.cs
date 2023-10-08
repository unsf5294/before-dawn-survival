public class SceneTransition : GameManagerClient
{
    public void GotoGameScene(float delay = 0f)
    {
        StartCoroutine(GameManager.GotoScene(GameManager.GameSceneName, delay));
    }

    public void GotoMenuScene(float delay = 0f)
    {
        StartCoroutine(GameManager.GotoScene(GameManager.MenuSceneName, delay));
    }
}