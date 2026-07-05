# CLAUDE.md

Guidance for Claude Code (and other agents) working in this repository.

## Project snapshot

**Before Dawn: the Land in Obscurity** — a 2.5D medieval **roguelike survival**
game in **Unity 2022.3.8f1**. The player is a crusader who must survive 5 minutes
until dawn. The health bar is **"Faith"**: it drains passively and from hits, and is
restored by killing enemies and touching artifacts. Build target is **WebGL**,
auto-deployed to GitHub Pages from `main`.

Long-term direction: a **Steam-quality push-your-luck roguelike** — Faith as
health/timer/ammo/currency, four night phases, procedural maps, meta-progression
(the Sisyphus "memory across loops" hook), ending in a Steam release with WebGL as
the free demo. The authoritative design is [GDD.md](GDD.md); the milestone plan is
[docs/ROADMAP.md](docs/ROADMAP.md).

## Architecture map

| System | Key files |
|---|---|
| Player / combat | [CharacterControl.cs](Assets/Scripts/CharacterControl.cs), [PlayerHealth.cs](Assets/Scripts/PlayerHealth.cs) (the Faith meter) |
| Camera | [CameraController.cs](Assets/Scripts/CameraController.cs) (follow, Q/E rotate, screen shake) |
| Enemies | [MonsterMovement.cs](Assets/Scripts/MonsterMovement.cs), [MonsterHealth.cs](Assets/Scripts/MonsterHealth.cs), [MonsterSpawner.cs](Assets/Scripts/MonsterSpawner.cs) |
| Game cycle | [GameTimer.cs](Assets/Scripts/GameTimer.cs) (5-min timer + waves), [CountdownTimer.cs](Assets/Scripts/CountdownTimer.cs), [Game Manager/](Assets/Scripts/Game%20Manager/) |
| Abilities | [Assets/Scripts/Abilities/](Assets/Scripts/Abilities/) + inline logic in [CharacterControl.cs](Assets/Scripts/CharacterControl.cs#L211-L308) |
| World / interactions | [Artifact.cs](Assets/Scripts/Artifact.cs) (healing shrines) |
| UI / HUD | [Assets/Scripts/UI Control/](Assets/Scripts/UI%20Control/), [HealthBarManager.cs](Assets/Scripts/HealthBarManager.cs), [SkillAvailabilityUI.cs](Assets/Scripts/Abilities/SkillAvailabilityUI.cs) |
| Shaders / VFX | [Assets/Shaders/](Assets/Shaders/), particle prefabs in [Assets/Prefabs/](Assets/Prefabs/) |

Scenes: `Assets/StartScene.unity` (menu, entry) → `Assets/GameScene.unity` (gameplay).
Both are in the build list.

## How to run and verify

- **Run:** open the project in Unity Hub (2022.3.8f1), open `Assets/StartScene.unity`,
  press **Play**.
- **There are no unit tests.** "Verified" means: the code compiles, and the affected
  flow plays correctly in the editor.
- The **automated compile gate** is the WebGL build. A PR to `main` runs
  [.github/workflows/pr-build.yml](.github/workflows/pr-build.yml) (build only);
  merging to `main` runs [deploy-webgl.yml](.github/workflows/deploy-webgl.yml)
  (build + deploy). A green build is the minimum bar before merge.

## Workflow rules (important for agents)

1. **Never commit directly to `main`.** `main` is always deployable and auto-deploys.
   Create a branch first: `<type>/<short-kebab-desc>` (e.g. `feat/victory-screen`).
2. **Conventional Commits**, with the co-author trailer on agent commits:
   ```
   feat(cycle): add victory screen with run summary

   Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>
   ```
   Full spec (types, scopes) in [CONTRIBUTING.md](CONTRIBUTING.md).
3. **One logical change per branch → PR → squash-merge** once the build is green.
   Delete the branch after merge.
4. **GDD.md is a living document.** If a gameplay PR changes the design (mechanics,
   numbers, content), update [GDD.md](GDD.md) in the same branch.

## Unity gotchas for agents

- **Always commit the `.meta` file** beside any asset/script you add, move, or delete.
  A missing or orphaned `.meta` corrupts references in the editor.
- **Do not hand-edit `.unity` / `.prefab` / `.asset` YAML** unless strictly necessary —
  GUIDs and fileIDs are fragile. Prefer changing C# and letting a human wire it.
- **Much behavior is wired via `UnityEvent`s in the Inspector**, not in code — e.g.
  [GameTimer.endofGame](Assets/Scripts/GameTimer.cs#L11) and
  [PlayerHealth.onDeath](Assets/Scripts/PlayerHealth.cs#L8). If a change needs new
  Inspector wiring you can't do headlessly, **implement the C# and clearly flag the
  manual wiring step** for the user.
- **Large binary assets** (some 64MB TGA textures) already exist in history; don't add
  more large binaries without reason. WebGL optimization is a roadmap item.

## Where things live

- [README.md](README.md) — player-facing overview, controls, how to run.
- [GDD.md](GDD.md) — **the living design document** (vision, mechanics, numbers).
- [docs/ROADMAP.md](docs/ROADMAP.md) — 7-milestone plan to Steam, with checklists.
- [CONTRIBUTING.md](CONTRIBUTING.md) — commit + branch conventions.
- [docs/WEBGL_DEPLOYMENT.md](docs/WEBGL_DEPLOYMENT.md) — WebGL build & Pages setup.
- [docs/GDD_ORIGINAL.md](docs/GDD_ORIGINAL.md) — archived university GDD.
- [docs/PROJECT_REPORT.md](docs/PROJECT_REPORT.md) — archived academic write-up.
