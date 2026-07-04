# Roadmap

Enhancing **Before Dawn** from an archived student project into an actively developed
game. Direction: **complete the core cycle first**, then deepen combat/interactions,
then grow into a **meta-progression roguelike** leaning on the Sisyphus "memory across
loops" narrative hook.

Each phase is a set of feature branches (see [CONTRIBUTING.md](CONTRIBUTING.md)).
Check items off as they land.

---

## Phase 0 — Foundation & automation
*Small, enables everything else.*

- [ ] `CLAUDE.md`, `CONTRIBUTING.md`, this roadmap.
- [ ] PR compile-gate workflow (`.github/workflows/pr-build.yml`).
- [ ] Hygiene: delete dead, fully-commented [MenuController.cs](../Assets/Scripts/UI%20Control/MenuController.cs); confirm both scenes are in the build list.

## Phase 1 — Complete the game cycle *(current focus)*

The loop today is essentially menu → game → menu. Make it a *complete* run.

- [ ] **`RunManager` / `GameState`** (new, under [Assets/Scripts/Game Manager/](../Assets/Scripts/Game%20Manager/)) tracking run outcome and stats: time survived, enemies slain, artifacts claimed, abilities used. This is also the data seam meta-progression builds on in Phase 3.
- [ ] **Victory screen** (survived to dawn) and **Defeat screen** (Faith depleted), showing the run summary. Replace the empty [CountdownTimer.TimerFinished()](../Assets/Scripts/CountdownTimer.cs#L37) stub and route [GameTimer.endofGame](../Assets/Scripts/GameTimer.cs#L56) / [PlayerHealth.onDeath](../Assets/Scripts/PlayerHealth.cs#L91) into a proper `GameOverController`.
- [ ] **Restart flow** — "Try again" reloads `GameScene` cleanly. Verify the `DontDestroyOnLoad` [GameManager](../Assets/Scripts/Game%20Manager/GameManager.cs#L17) resets state correctly on replay (no stale singletons/coroutines).
- [ ] **Faith economy tuning** — extract the hardcoded drain (1 per 3s ≈ death at exactly 5:00) from [PlayerHealth.DecreaseHealthOverTime](../Assets/Scripts/PlayerHealth.cs#L36) into serialized/config values; add margin and a **difficulty setting** (already implied by the GDD menu).

## Phase 2 — Combat & interaction depth

- [ ] **Unify the two ability systems** — fold the inline abilities in [CharacterControl](../Assets/Scripts/CharacterControl.cs#L211-L308) and the unused [AbilityHolder](../Assets/Scripts/Abilities/AbilityHolder.cs)/[AbilityClient](../Assets/Scripts/Abilities/AbilityClient.cs) into one **ScriptableObject-driven** ability framework; expand toward the GDD's original 5 skills.
- [ ] **Enemy variety** — give the three prefabs (troll / wolf / goblin) distinct stats and behaviors; add a ranged or elite type.
- [ ] **Artifact variety** — more boons on [Artifact.cs](../Assets/Scripts/Artifact.cs) (temporary buffs, ability rerolls) beyond a flat heal.
- [ ] **Combat feel + perf** — hit-stop, knockback, floating damage numbers; replace the per-attack `FindGameObjectsWithTag("Monster")` in [PerformAttack](../Assets/Scripts/CharacterControl.cs#L98) with cached lookups / physics overlap queries.

## Phase 3 — Meta-progression (the Sisyphus loop) *(end vision)*

- [ ] **Persistent save** (JSON / PlayerPrefs) for currency earned per run — built on Phase 1's `RunManager` stats.
- [ ] **Between-run upgrade screen** — permanent stat unlocks, new starting abilities.
- [ ] **Narrative hook** — carry small "memories" / hints forward across loops, so each death informs the next run.
- [ ] **Unlockables** — skills and enemy types gated behind progression.

## Phase 4 — Content, juice & release

- [ ] More modular maps / terrain layouts.
- [ ] Audio pass; VFX and shader polish.
- [ ] **WebGL optimization** — crunch the 64MB TGA textures, set a load-time budget.
- [ ] Settings, control remapping, gamepad support (GDD-scoped).

---

*Phases are ordered but not rigid — pull a later item forward when it unblocks or
sharpens the current focus.*
