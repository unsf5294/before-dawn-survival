using UnityEngine;
using UnityEngine.InputSystem;

// Central input facade. All gameplay input is meant to flow through here so the rest
// of the game never touches UnityEngine.Input or the Input System directly.
//
// M0 scope: only Move is wired (the movement consumer). Attack/abilities/camera/menu
// still read legacy Input under the project's "Both" input handler; they migrate here
// as those systems are rewritten. A .inputactions asset + rebinding UI replace these
// code-defined bindings in M3.
public static class InputReader
{
    private static InputAction moveAction;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (moveAction != null) return;

        moveAction = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        moveAction.AddBinding("<Gamepad>/leftStick");
        moveAction.Enable();
    }

    /// <summary>Horizontal movement input; x = strafe, y = forward, each in [-1, 1].</summary>
    public static Vector2 Move => moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
}
