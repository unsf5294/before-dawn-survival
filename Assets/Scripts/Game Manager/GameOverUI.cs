using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Runtime-built game-over overlay (victory or defeat) showing a run summary with
// Retry / Menu actions. Built entirely from code so it needs no scene wiring —
// this is a functional first pass intended to be restyled in the editor later.
// Buttons are clickable; R (retry) / M or Esc (menu) also work as fallbacks.
public class GameOverUI : MonoBehaviour
{
    public static void Show(bool victory, RunManager run)
    {
        var go = new GameObject("GameOverUI");
        go.AddComponent<GameOverUI>().Build(victory, run);
    }

    private void Build(bool victory, RunManager run)
    {
        EnsureEventSystem();

        // Root canvas, drawn above the in-game HUD.
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        gameObject.AddComponent<GraphicRaycaster>();

        // Dimmed full-screen backdrop.
        var bg = CreatePanel(transform, new Color(0f, 0f, 0f, 0.72f));
        Stretch(bg);

        string title = victory ? "DAWN BREAKS" : "DEVOURED BY DARKNESS";
        Color titleColor = victory ? new Color(1f, 0.86f, 0.4f) : new Color(0.85f, 0.2f, 0.2f);
        string subtitle = victory ? "You held your faith until sunrise." : "Your faith was consumed before the light.";

        CreateText(bg.transform, title, 92, titleColor, FontStyle.Bold,
            new Vector2(0.5f, 0.80f), new Vector2(1000, 140));
        CreateText(bg.transform, subtitle, 34, Color.white, FontStyle.Italic,
            new Vector2(0.5f, 0.70f), new Vector2(1200, 80));

        int m = Mathf.FloorToInt(run.TimeSurvived / 60f);
        int s = Mathf.FloorToInt(run.TimeSurvived % 60f);
        string summary =
            $"Time survived   {m:00}:{s:00}\n" +
            $"Enemies slain   {run.Kills}\n" +
            $"Artifacts       {run.ArtifactsClaimed}\n" +
            $"Abilities used  {run.AbilitiesUsed}";
        CreateText(bg.transform, summary, 40, new Color(0.9f, 0.9f, 0.9f), FontStyle.Normal,
            new Vector2(0.5f, 0.48f), new Vector2(900, 320));

        CreateButton(bg.transform, "Try Again  (R)", new Vector2(0.5f, 0.22f), Retry);
        CreateButton(bg.transform, "Main Menu  (M)", new Vector2(0.5f, 0.12f), Menu);
    }

    private void Update()
    {
        // Keyboard fallbacks (unaffected by Time.timeScale = 0).
        if (Input.GetKeyDown(KeyCode.R)) Retry();
        else if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape)) Menu();
    }

    private void Retry() => Load(GameManager.GameSceneName);
    private void Menu() => Load(GameManager.MenuSceneName);

    private void Load(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    // --- tiny uGUI builders -------------------------------------------------

    private static void EnsureEventSystem()
    {
        if (EventSystem.current != null) return;
        var es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();
    }

    private static Font UIFont()
    {
        return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")
            ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private static Image CreatePanel(Transform parent, Color color)
    {
        var go = new GameObject("Panel", typeof(Image));
        go.transform.SetParent(parent, false);
        var img = go.GetComponent<Image>();
        img.color = color;
        return img;
    }

    private static void Stretch(Component c)
    {
        var rt = (RectTransform)c.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static Text CreateText(Transform parent, string content, int size, Color color,
        FontStyle style, Vector2 anchor, Vector2 sizeDelta)
    {
        var go = new GameObject("Text", typeof(Text));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<Text>();
        t.text = content;
        t.font = UIFont();
        t.fontSize = size;
        t.fontStyle = style;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        var rt = t.rectTransform;
        rt.anchorMin = rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = Vector2.zero;
        return t;
    }

    private static void CreateButton(Transform parent, string label, Vector2 anchor,
        UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject("Button", typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        go.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.18f, 0.95f);
        var rt = (RectTransform)go.transform;
        rt.anchorMin = rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(420, 90);
        rt.anchoredPosition = Vector2.zero;

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(onClick);

        var text = CreateText(go.transform, label, 36, Color.white, FontStyle.Bold,
            new Vector2(0.5f, 0.5f), new Vector2(400, 80));
        Stretch(text); // fill the button
    }
}
