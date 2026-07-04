# Before Dawn: the Land in Obscurity

A 2.5D medieval **roguelike survival** game built in **Unity 2022.3.8f1**. You play a lone crusader of The Divine Order, sent into the dark to hold your ground until dawn. But the health bar isn't health — it's **Faith**, and the darkness is always whispering.

> Survive the last five minutes before dawn. Do nothing, and the darkness takes you anyway.

<!-- TODO: drop a gameplay GIF or screenshot here once the WebGL build is live -->
<!-- ![Gameplay](docs/media/gameplay.gif) -->

## The hook: Faith, not Health

Your Faith meter **drains on its own** — roughly a point every few seconds as the Dark Lord's influence eats at you. Getting hit drains it faster. Hiding doesn't work: stand still and you'll be assimilated *before* the sun rises. To survive you have to stay aggressive — **kill enemies and touch divine artifacts to restore Faith** — while the clock ticks toward dawn.

Thematically it's a Sisyphus loop: every death rewinds the same five minutes, memories wiped, only the player aware of past defeats.

## How it plays

- **Third-person 2.5D** perspective (à la *Hades* / *V Rising*) with a camera that tracks the player, transitions in at the start, shakes on impact, and can be rotated around the character.
- **5-minute survival timer** with **three escalating enemy waves** at 90s / 180s / 240s, each announced with an in-game warning.
- **Three random abilities** granted over time from distinct groups — recovery (heal), crowd control (knockback), and a buff (attack + move-speed). Which you get, and in what order, varies each run.
- Custom **shaders & particle effects**: a sin-driven "shiny" unlit shader for heal/artifact VFX, and a Blinn-Phong "energized" glow on the weapon head during attacks.

## Controls

| Action | Key |
|---|---|
| Move | `W` `A` `S` `D` (camera-relative) |
| Attack (3-hit combo) | `J` or Left Mouse |
| Shield bash | `K` or Right Mouse |
| Abilities 1 / 2 / 3 | `1` `2` `3` (once unlocked) |
| Rotate camera | `Q` / `E` |

## Running it

**In the Unity Editor**
1. Install **Unity 2022.3.8f1** (via Unity Hub).
2. Clone this repo and open the project folder in Unity Hub.
3. Open `Assets/StartScene.unity` and press **Play**.

**Play in the browser** — see [WebGL deployment](#playing-in-the-browser) below.

## Project structure

```
Assets/
├── Scripts/
│   ├── CharacterControl.cs      # movement, combo attacks, abilities
│   ├── PlayerHealth.cs          # the Faith meter (drain / damage / heal)
│   ├── CameraController.cs      # follow cam, rotation, screen shake
│   ├── Monster*.cs              # enemy AI, health, spawning
│   ├── GameTimer.cs             # 5-min cycle + wave triggers
│   ├── Artifact.cs              # healing shrines
│   ├── Abilities/               # ability framework + skill UI
│   ├── Game Manager/            # scene persistence & transitions
│   └── UI Control/              # menus, in-game HUD
├── Shaders/                     # shinyParticle + HammerHead shaders
├── Prefabs/                     # player, monsters, VFX, obstacles
├── *.unity                      # StartScene, GameScene
└── (art / audio / terrain asset packs)
```

## Playing in the browser

Unity can build this to **WebGL**, which is just static files (`index.html` + a `Build/` folder) that GitHub Pages can host for free — that's how the browser-playable version works. See **[docs/WEBGL_DEPLOYMENT.md](docs/WEBGL_DEPLOYMENT.md)** for two ways to do it:
1. **Manual** — build in the Unity editor and push to a `gh-pages` branch (simplest, no setup).
2. **Automated** — a GitHub Actions workflow ([`.github/workflows/`](.github/workflows/)) that builds and deploys on every push (needs a free Unity license configured as a secret).

## Credits & origin

Originally built for **COMP30019 (Graphics & Interaction), University of Melbourne**, by a three-person team — **Yuecheng Wang, Junyan Lai, Jingxuan Zhang**. The original academic write-up (evaluation plan, playtest report, shader notes) is preserved at **[docs/PROJECT_REPORT.md](docs/PROJECT_REPORT.md)**, and the design doc lives in **[GDD.md](GDD.md)**.

Third-party assets (Unity Asset Store / Mixkit) are credited in [docs/PROJECT_REPORT.md](docs/PROJECT_REPORT.md#references-and-external-resources).

## Roadmap

Ongoing solo enhancement toward a fuller game:
- [ ] Tighter core loop and clearer Faith-economy feedback
- [ ] More enemy variety and richer interactions
- [ ] Live WebGL build playable from this README
