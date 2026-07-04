using UnityEngine;
using UnityEngine.SceneManagement;

// Per-run coordinator. Tracks run stats and decides when a run ends: victory when
// dawn is reached, defeat when Faith hits zero. Lives only within GameScene; a
// fresh instance is (re)created each time GameScene loads, so stats and timing
// reset automatically on restart. Gameplay scripts report to it via the singleton
// (RunManager.Instance.RegisterKill(), etc.) — no Inspector wiring needed.
public class RunManager : MonoBehaviour
{
    private static RunManager _instance;
    public static RunManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("RunManager");
                _instance = go.AddComponent<RunManager>();
            }
            return _instance;
        }
    }

    public int Kills { get; private set; }
    public int ArtifactsClaimed { get; private set; }
    public int AbilitiesUsed { get; private set; }
    public float TimeSurvived => Time.unscaledTime - startTime;
    public bool RunOver { get; private set; }

    private float startTime;

    // Registered once at app startup; fires on every scene load thereafter, so it
    // covers both the initial GameScene load and restarts.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != GameManager.GameSceneName) return;

        // Clear any pause left over from a game-over screen and start a clean run.
        Time.timeScale = 1f;
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
        _ = Instance; // force-create so startTime == run start
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        startTime = Time.unscaledTime;
    }

    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }

    public void RegisterKill() => Kills++;
    public void RegisterArtifact() => ArtifactsClaimed++;
    public void RegisterAbilityUsed() => AbilitiesUsed++;

    public void TriggerVictory() => EndRun(true);
    public void TriggerDefeat() => EndRun(false);

    private void EndRun(bool victory)
    {
        if (RunOver) return;
        RunOver = true;

        // Freeze gameplay. This also stalls the legacy coroutine-based scene
        // transitions (they use scaled WaitForSeconds), so the outcome screen owns
        // the end-of-run flow.
        Time.timeScale = 0f;
        GameOverUI.Show(victory, this);
    }
}
