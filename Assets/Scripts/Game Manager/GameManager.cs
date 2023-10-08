// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneManager))]
public class GameManager : MonoBehaviour
{
    public const string Tag = "GameManager";

    public const string MenuSceneName = "StartScene";
    public const string GameSceneName = "GameScene";

    private void Awake()
    {
        // Should not be created if there's already a manager present (at least
        // two total, including ourselves). This allows us to place a game
        // manager in every scene, in case we want to open scenes direct.
        if (GameObject.FindGameObjectsWithTag(Tag).Length > 1)
            Destroy(gameObject);

        // Make this game object persistent even between scene changes.
        DontDestroyOnLoad(gameObject);

        // Hook into scene loaded events.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public IEnumerator GotoScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        var asyncLoadOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoadOp.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
}

