using UnityEngine;

// Temporary global audio kill-switch. Runs automatically at startup (no scene
// wiring required) and mutes every AudioListener in the game. Flip Muted to false
// (or delete this file) to restore audio.
public static class AudioMute
{
    public const bool Muted = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Apply()
    {
        AudioListener.volume = Muted ? 0f : 1f;
    }
}
