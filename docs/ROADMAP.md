# Roadmap — to Steam

Implementation plan for the design in [GDD.md](../GDD.md). Seven milestones,
each a set of feature branches per [CONTRIBUTING.md](../CONTRIBUTING.md)
(branch → PR → green WebGL build → squash-merge).

**Standing rules**
- Every merge keeps the WebGL demo on `main` green and playable — CI deploys it.
- No unit tests exist: each milestone ships with a **manual verification
  checklist**; "done" = checklist passes in the editor **and** the deployed build.
- **`[H]`** marks work needing human editor/Inspector action — agents implement the
  code and flag these steps. Everything else is agent-ownable (all C#, shaders,
  `manifest.json`, `.inputactions` JSON, ProjectSettings/TagManager/Quality YAML
  scalars, `.meta` importer settings, editor tools).
- **`[RISKY]`** marks one-time migrations, sequenced earliest so content is never
  tuned twice.
- Estimated total: **~4–6 months part-time** (solo + agents).

---

## M0 — Foundations & risk retirement (~1–2 wk)

| Branch | Scope |
|---|---|
| `chore/linear-color-space` [RISKY][H: relight pass] | Gamma → Linear; re-tune ambient/lights/emissives while lighting content is still minimal |
| `feat/input-system-both` [RISKY] | Add Input System 1.7.0 in "Both" mode + `BeforeDawn.inputactions` + static `InputReader` facade; first consumer: movement |
| `feat/physics-layers` [H: scene objects] | Player/Enemy/Obstacle/Ground/Pickup/SensorZone/FX layers + collision matrix |
| `feat/player-motor` | Rigidbody-velocity movement, FreezeRotation (deletes angular-drag hack), interpolation |
| `feat/enemy-motor` | Steering (seek + whisker avoidance + separation + wander); deletes transform-write movement and `pushTo` |

**✔ Checklist:** plays identically to before · both input paths work · no wall
jitter, no monster stacking · WebGL boots and looks correct in Linear.

## M1 — Combat feel & unified abilities (~2–3 wk)

| Branch | Scope |
|---|---|
| `feat/damage-pipeline` | `DamageInfo`/`IDamageable`, overlap+cone queries, hit-stop, knockback, damage numbers |
| `feat/enemy-brain` | FSM with windup telegraphs; deletes `OnCollisionEnter` attacks |
| `feat/animator-rebuild` [H: run tool once] | Editor script wires existing damage/dead/death clips into controllers |
| `feat/player-death-flow` | Death anim → 1.6s → defeat screen; fixes instant-`Destroy` dangling refs |
| `feat/dodge` | 0.18s dash, i-frames, trail VFX, recovery-cancel window |
| `feat/ability-so` + `feat/ability-migrate` [H: audit scene UnityEvents, wire caster] | `AbilityDefinition` SOs + `AbilityCaster` + `PlayerStats`; Communion/Repulsion/Zeal Surge; deletes both legacy systems |
| `feat/object-pools` | Monsters, VFX, damage numbers, one-shot audio |
| `feat/zeal` + `feat/faith-costs` | Overheal→Zeal buff; ability Faith costs (GDD §3) |

**✔ Checklist:** telegraph→dodge→punish loop feels fair · hit-stop/knockback/damage
numbers visible · death anims both sides · 3 abilities behave per GDD from SOs ·
Zeal segment appears on overheal and buffs · pools reuse under a 100-kill soak.

## M2 — Procedural generation (~2–3 wk)

| Branch | Scope |
|---|---|
| `feat/run-seed` | `DeterministicRng`, seed on HUD/game-over, retry-same-seed, `?seed=` URL param |
| `feat/mapgen-layout` | 6×6 cell planner (spawn/shrines/POIs/biomes) + editor `MapPreviewWindow` |
| `feat/mapgen-ground` | Runtime low-res Terrain tiles, fBm heights, clearings, border rim |
| `feat/mapgen-props` | Poisson-disc scatter, BLOCKER/DECOR collider policy, static batching |
| `feat/mapgen-integration` [H: shrine prefab extract, `GameScene_Legacy` duplicate, strip old map] | Procgen becomes the default GameScene path |
| `feat/mapgen-lights` | `LightBudgetManager` + three-tier torch/shrine lighting |
| `feat/night-phases` | GameTimer → phase events; drain curve, spawn tables, phase banner hook |
| `chore/texture-crunch` | Crunch the 64MB TGAs and oversized textures |

**✔ Checklist:** same seed → identical map (spawn + 2 landmarks match) · different
seeds differ · ≥3 shrines reachable · spawn clearing empty · border DoT works ·
no enemy snags in a 2-minute watch · WebGL load ≤5s · 60fps mid-tier desktop
Chrome. **Exit:** legacy scene + 8 TerrainData assets deleted in a cleanup PR.

## M3 — UI/UX overhaul (~2 wk)

| Branch | Scope |
|---|---|
| `feat/ui-tween` | ~200-line tweener (unscaled-time, easings, CanvasGroup/Rect extensions) |
| `feat/ui-router` | `UIScreen` base + router stack, Esc handling, single EventSystem |
| `feat/hud-v2` [H: canvas rewire] | Faith bar (forecast/ghost/Zeal/flash), radial ability bar, phase banner, seed label |
| `feat/pause-service` | Ref-counted timeScale; pause screen; RunManager/hit-stop route through it |
| `feat/settings` [H: create AudioMixer] | Audio sliders, difficulty binding, quality preset; PlayerPrefs persistence |
| `feat/main-menu-v2` [H: diorama dressing] | 3D campfire backdrop, menu, seed field |
| `feat/gameover-v2` | TMP screen: stats, Embers, Retry-same-seed/New/Menu |
| `feat/input-cutover` [RISKY — last PR] | Flip to Input System-only, `InputSystemUIInputModule`, rebind screen |

**✔ Checklist:** all screens tween at `timeScale 0` · settings persist and apply
live · difficulty audibly changes drain · rebind persists · fully playable via
keyboard, mouse, and gamepad.

## M4 — Audio & atmosphere (~1–2 wk)

| Branch | Scope |
|---|---|
| `feat/audio-director` | Deletes `AudioMute`; mixer routing; pooled one-shots; footsteps; interaction SFX |
| `feat/music-states` | Phase-layered music via mixer snapshots; replace broken `.mov` track |
| `feat/postfx` [H: profile tuning] | Enable PPv2: bloom/FXAA/grade/vignette + fog + procedural skybox & moon |
| `content/sfx-sourcing` [H: CC0 picks + import] | UI/footstep/shrine/pact/Zeal/death/Herald/dawn audio |
| `feat/dawn-sequence` | Skybox lerp, light warm-up, monsters burn, victory flash (GDD §2.2) |

**✔ Checklist:** no silent interactions · music tracks the phases and death ·
bloom sells tier-3 fake lights · dawn burn moment lands · WebGL audio starts on
first user gesture.

## M5 — Content depth & meta (~2–3 wk)

| Branch | Scope |
|---|---|
| `feat/enemy-variants` | 7 encounters via `EnemyDefinition` SOs (GDD §4.3) |
| `feat/herald-boss` | False-Dawn boss: slam, summon circle, dawn-burn alternative |
| `feat/rites` | Shrine draft UI + ~12 run-scoped boons |
| `feat/dark-pacts` | Desecrated shrines + 4 launch pacts |
| `feat/meta-save` | JSON in PlayerPrefs; Embers earned from run stats |
| `feat/campfire-screen` | Between-run upgrade tree (Body/Arms/Spirit) |
| `feat/daily-dawn` | Date-hashed seed mode, one attempt |
| `balance/economy-pass` | Tune drain curve, costs, rewards against playtests |

**✔ Checklist:** full loop run→death→Embers→upgrade→stronger next run · save
survives page reload · variants visually distinct at night · Herald telegraphs
readable · pacts genuinely tempt.

## M6 — Steam release (~3–4 wk + wishlist tail)

| Branch | Scope |
|---|---|
| `ci/desktop-builds` | Windows/Linux targets in GameCI alongside WebGL |
| `feat/steamworks` | Steamworks.NET: achievements (First Dawn, Dawnbreaker, No-Pact Dawn, All-Pacts Survivor, Deeper Nights V…), Steam Cloud for meta JSON, rich presence |
| `feat/engine-upgrade-eval` | Unity 6 LTS + URP as ONE bundled migration — go/no-go decided here, not before |
| [H] Store presence | Capsule art, trailer, screenshots, description; demo config (WebGL demo stays free) |
| Release | Closed beta → wishlist campaign → launch |

**✔ Checklist:** green desktop builds · achievements fire · cloud save
round-trips · store page live.

---

## Sequencing rationale

Linear color space and Input-System-Both are the two cheap-now/expensive-later
flips → M0. Combat feel before procgen so steering is proven before maps get
cluttered. UI after procgen (HUD needs the seed; settings need the mixer/difficulty
surface). Audio after the settings that control it. Meta last, on stabilized run
stats. URP/engine upgrade deliberately deferred to M6 as a single migration.

*Phases are ordered but not rigid — pull a later item forward when it unblocks or
sharpens the current focus.*
