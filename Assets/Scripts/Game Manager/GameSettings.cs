// Global, scene-independent game settings. Kept as a lightweight static so menus
// and gameplay can share values without an Inspector-wired singleton. A menu can
// set Difficulty before loading GameScene; gameplay reads the derived multipliers.

public enum Difficulty { Easy, Normal, Hard }

public static class GameSettings
{
    public static Difficulty Difficulty = Difficulty.Normal;

    // How fast Faith drains relative to the base rate. Higher = harder (drains sooner).
    public static float FaithDrainMultiplier
    {
        get
        {
            switch (Difficulty)
            {
                case Difficulty.Easy: return 0.75f;
                case Difficulty.Hard: return 1.5f;
                default: return 1f;
            }
        }
    }
}
