# Before Dawn: the Land in Obscurity — Game Design Document

> *One knight. Five minutes of night. Faith is everything — your life, your clock,
> and the price of every power. Survive to dawn, die remembering, return stronger.*

**Status: living document.** This is the authoritative design for the Steam-quality
version of Before Dawn. Any gameplay PR that changes design updates this file in the
same branch. Implementation order and verification checklists live in
[docs/ROADMAP.md](docs/ROADMAP.md). The original university GDD is archived at
[docs/GDD_ORIGINAL.md](docs/GDD_ORIGINAL.md).

**Genre:** 2.5D medieval night-survival roguelike (Hades-style camera, horde-lite
pacing). **Targets:** WebGL (free demo, continuously deployed) → Windows/Linux on
Steam. **Engine:** Unity 2022.3.8f1, Built-in Render Pipeline.

---

## 1. Vision & Pillars

Every feature must serve at least one pillar; anything serving none is cut.

1. **Faith is everything.** One resource is health, timer, ammo, and currency.
   Every mechanic reads or writes Faith; every decision is spend-life-for-power.
2. **The night has a shape.** A run is a five-minute drama in four escalating
   phases. Players learn its rhythm and plan around it.
3. **Deaths teach, dawns crown.** Roguelike fairness: telegraphed attacks, readable
   danger, seeded reproducible maps. Meta-progression frames every loss as memory.
4. **A knight who feels heavy but never clumsy.** Weighty crusader combat — commit
   to swings — with modern escape valves (dodge i-frames, cancel windows).

## 2. Core Loops

- **Moment loop (seconds):** fight → Faith drains → kill/claim to refill → spend
  Faith on abilities → overheal into Zeal → push deeper or play safe.
- **Run loop (5 minutes):** spawn → explore the procedural night → shrines grant
  Rites, tempt with Pacts → survive four phases → face the Herald at False Dawn →
  dawn breaks, monsters burn → results.
- **Meta loop (sessions):** die or win → earn Embers of Memory → spend at the
  Campfire → next run starts stronger → Deeper Nights unlock.

### 2.1 The Night — four phases

Replaces the flat 90/180/240s wave events with a shaped drama. Driven by `GameTimer`
(refactored to raise phase events instead of writing UI text directly).

| Phase | Time | Passive drain | Spawns | Mood |
|---|---|---|---|---|
| **Dusk** | 0:00–1:30 | 12 Faith/min | wolves, sparse | learn the map, first shrine |
| **Midnight** | 1:30–3:00 | 18/min | + hobgoblins, steady | pressure begins, second Rite |
| **Blood Hour** | 3:00–4:30 | 24/min | + trolls; elites appear | horde peak, pact temptations |
| **False Dawn** | 4:30–5:00 | 30/min | everything + **Herald of Dusk** | horizon glows, last stand |

Idle drain totals ≈ 96 Faith: doing nothing still kills you just before dawn — the
founding rule holds — but the curve front-loads safety and back-loads drama. Phases
also drive spawn tables, music layers, fog/moon color, and the phase banner.

### 2.2 The Dawn Sequence (signature moment)

At 5:00: the skybox lerps night → sunrise gradient over ~6s, moonlight warms to
gold, fog thins, and **every living monster ignites and burns** in shiny-particle
VFX (the original GDD's "creeps burst into fireworks under the sun"). Zeal-gold
flash → victory screen. Cheap to build — skybox lerp, light tween, pooled VFX —
enormous payoff. This is the trailer shot.

## 3. Faith Economy (push-your-luck core)

One bar, four roles: **health** (hits subtract), **timer** (passive drain),
**ammo** (casting costs), **currency** (Pacts trade it). All numbers below are
launch-tuning baselines, owned by the `balance/economy-pass` milestone item.

**Income**
| Source | Faith |
|---|---|
| Night Wolf kill | +6 |
| Hobgoblin kill | +8 |
| Moss Troll kill | +12 |
| Elite kill | +20 |
| Herald of Dusk kill | +40 |
| Blessed Shrine | +25 |
| Communion channel | +20 |

**Costs**
| Sink | Faith |
|---|---|
| Ability cast | 6–15 (per ability) |
| Enemy hit | 6–20 (all telegraphed) |
| Passive drain | per phase table (§2.1) |
| Out of bounds | 10/s (existing MapBorder rule) |

### 3.1 Zeal (overheal)

Faith gained beyond max converts 1:1 into **Zeal** — a golden segment atop the bar,
cap 50. Zeal drains fast (2/s) and while present grants **+1% damage and +0.5% move
speed per point**. Kills at full health are never wasted; chain-killing at max Faith
is the optimal aggressive play. The economy itself rewards the pillar-1 fantasy.

### 3.2 Dark Pacts

At **Desecrated Shrines** (seeded, ~2 per map): permanent-for-the-run trades.
The narrative corruption, mechanized — power flows from the dark, and the god
notices (§9).

| Pact | Price | Power |
|---|---|---|
| Pact of Wrath | −20 max Faith | +30% damage |
| Pact of Wind | −15 max Faith | dodge cooldown halved |
| Pact of Hunger | drain +20% | kills give +50% Faith |
| Pact of the Moth | −1 max Faith/s while near shrines | ability costs refunded on kill within 2s |

### 3.3 Difficulty

Existing `GameSettings` backend, finally exposed in Settings: **Easy** ×0.75 drain ·
**Normal** ×1.0 · **Hard** ×1.25 + elites one phase earlier. Meta unlocks **Deeper
Nights I–V** (stacking run modifiers) after the first dawn.

## 4. Combat Design

### 4.1 Player kit (crusader + hammer — all listed clips verified in-project)

- **3-hit combo** (existing `atack1/2/3` clips): hits 1–2 quick, hit 3 heavy with a
  knockback burst. **Cancel window:** dodge cancels recovery frames, never windup —
  weight with an escape valve.
- **Shield bash** (existing clip): short-range shove that **interrupts enemy
  windups** (stagger), small damage, 4s cooldown. The read-and-punish tool.
- **Dodge** *(new — no clip exists; capsule dash + trail VFX, run clip at 1.6×)*:
  0.18s at 22 m/s, **i-frames vs hits** (passive drain still ticks — thematic),
  0.9s cooldown.
- **Communion** *(replaces the flat heal ability)*: channel 1.5s while **vulnerable
  and slowed** → +20 Faith. Cost is exposure, not Faith (a Faith-costed heal is
  self-defeating). Interrupted by hits.
- Weapon glow on swing: keep the existing `HammerHead.shader` `_AttackEffect`.

### 4.2 Feel layer

Hit-stop 45ms on connect / 90ms on kill (via ref-counted `PauseService` — never
fights pause). Knockback as physics impulse. Pooled TMP damage numbers (crit gold).
Camera shake reuses `CameraController.setCameraShake`. Monster hit-flash + wiring
the existing-but-unused `damage`/`dead` clips. Player death animation (exists,
unwired) → 1.6s hold → defeat screen (also fixes the current instant-`Destroy`
dangling-reference bug).

### 4.3 Enemy roster — 3 models → 7 encounters

Variants via `EnemyDefinition` ScriptableObjects (stats, tint, scale, animator
override, faith reward). ⭐ = elite, 👑 = boss.

| Enemy | Base model | Role | Behavior |
|---|---|---|---|
| **Night Wolf** | wolf | fast fodder, packs of 3–4 | flank-biased steering, 0.35s lunge telegraph |
| **Dire Wolf** ⭐ | wolf, red tint, ×1.3 | pack captain (Blood Hour+) | howl buffs pack speed |
| **Hobgoblin Marauder** | hobgoblin | line infantry | straight chase, 0.45s swing telegraph |
| **Hobgoblin Berserker** ⭐ | hobgoblin, dark tint | frenzy elite | below 50% HP: +40% speed/damage |
| **Moss Troll** | troll | slow siege tank | ground-slam AoE, 0.6s telegraph, big knockback |
| **Elder Troll** ⭐ | troll ×1.4 | area denial | slam leaves a 3s damaging shockwave ring |
| **Herald of Dusk** 👑 | troll ×2.5, emissive | False-Dawn boss | slam + summon circle (3 wolves); killable (+40 Faith, "Dawnbreaker" run tag) **or** survivable — burns at dawn either way |

All melee — honest constraint: no ranged model exists in the project (flagged for
future sourcing). Telegraphs = emissive flash + windup clip; **every hit is
dodgeable**. Spawning: the existing ring-around-player `MonsterSpawner` evolves into
`SpawnDirector` reading per-phase spawn tables with a budget (pooled, ~50 alive cap
on WebGL).

## 5. Abilities & Rites

**One ability system** — `AbilityDefinition` ScriptableObjects with a polymorphic
effects list (`[SerializeReference]`: AoeDamage, Push, Heal, StatMod, SpawnVfx…) —
replacing both legacy systems (the inline `CharacterControl` logic and the unused
`AbilityHolder`/`AbilityClient` pair). The existing three map onto it:

| Ability | Effect | Faith cost |
|---|---|---|
| **Communion** | channel 1.5s, +20 Faith (§4.1) | 0 (cost = exposure) |
| **Repulsion** | push radius 5 + 10 damage + 0.5s stun | 8 |
| **Zeal Surge** | damage ×2 + speed ×1.5 for 10s | 12 |

Slots: 3, granted at 0:10 / 1:00 / 2:00 (existing schedule, now config). A 4th slot
is a meta unlock.

**Rites** — run-scoped boons drafted at Blessed Shrines (seeded choice of 2 from a
pool of ~12). The shrine choice moment is the run's draft. Examples:

| Rite | Effect |
|---|---|
| Rite of Embers | combo finisher applies burn |
| Rite of the Bulwark | bash radius ×2 |
| Rite of Momentum | dodge leaves a damaging trail |
| Rite of the Tithe | kills near shrines +50% Faith |
| Rite of Stillness | standing still 1s slows drain 50% |

## 6. Procedural Map Generation

**Approach** (validated against WebGL constraints and available assets): runtime
generation of **low-res Unity Terrain** — a 4×4 grid of 50m tiles (200×200m total;
inner 150×150 playable, matching the current hand-built scale; 25m border collar
rises into a hill rim). Heightmap resolution 65 per tile generates in tens of
milliseconds, hidden behind the existing 2s camera intro. Reuses the project's 7
TerrainLayers; terrain colliders come free.

**Pipeline** (pure math, fully deterministic, in `Assets/Scripts/World/`):

1. **Seed resolve** — menu seed field / retry-same-seed / `?seed=` URL param, else time.
2. **`MapLayoutPlanner`** — 6×6 cell grid: spawn at center, 3–4 shrine cells
   (min-distance apart), 2–3 POI camps (consolidated prop sets), remaining cells
   biomed forest/rocky/meadow by value noise.
3. **`GroundBuilder`** — fBm hills (~1.2m amplitude), flattened clearings at
   spawn/shrines/POIs, raised border rim.
4. **`PropScatterer`** — per-cell Poisson-disc, per-biome density, drawing on the
   ~40-prefab medieval kit + trees. **BLOCKER** props (trees, wells, big stones) get
   primitive colliders on the `Obstacle` layer; **DECOR** (jars, buckets, coins) is
   walk-through; zero MeshColliders in generated content.
5. **Shrine/POI placement** → **border trigger ring** (existing 10 Faith/s rule) →
   **static-batch** all props.

**Seeds & determinism:** `DeterministicRng` (System.Random + named child streams so
adding a consumer never shifts other systems). Base36 seed shown on HUD and
game-over; **Retry Same Seed** button; WebGL `?seed=` links make bugs and challenges
shareable. Later: **Daily Dawn** (date-hashed seed, one attempt). Editor
`MapPreviewWindow` renders any seed's layout to a texture for per-PR visual
regression.

**Enemy navigation:** steering, **no NavMesh** — open scatter-fields don't need
pathfinding, and runtime NavMesh baking on WebGL hitches the main thread.
`EnemyMotor` = seek + 3-whisker obstacle avoidance + neighbor separation (spatial
hash at 5Hz) + wander; brains tick 10Hz round-robin. Documented upgrade path: if
walled/dungeon chunks ever arrive, add `com.unity.ai.navigation` and feed path
corners into the same steering seek.

**Night lighting under WebGL's pixel-light cap** — three tiers:
- **Tier 1:** real pixel lights, budget 4, distance-sorted by `LightBudgetManager`
  (shrines, player lantern).
- **Tier 2:** ~4 vertex lights ("Not Important" render mode — free).
- **Tier 3:** unlimited fakes — emissive mesh (bloom feeds on it) + additive halo
  billboard + ground-pool quad + flicker (extends existing `LightBlink`). All
  decorative torches/embers are tier 3.

**Existing map:** `GameScene` is converted in place (preserving the fragile
Inspector-wired HUD); the hand-built 9-tile map is duplicated to `GameScene_Legacy`
during transition and deleted (with its 8 TerrainData assets) after parity.

## 7. UI / UX Design

**Framework:** per-scene canvases + shared screen prefabs + a code `UIRouter` stack
(push/pop, Esc pops) + an **own ~200-line tween utility** (no store dependencies;
`CanvasGroup.FadeTo`, `SlideTo`, `PunchScale`; unscaled time by default so menus
animate at `timeScale 0`). All text TMP; the already-imported fantasy display font
for headers.

- **Main menu:** a 3D night diorama backdrop — the crusader idle at a campfire,
  fog, slow camera drift. *The game is the menu.* Play · Daily Dawn (later) ·
  Settings · Codex (later) · Quit (hidden on WebGL) · seed entry field.
- **HUD:** Faith bar with **drain-forecast segment** (projected 10s loss), ghost
  damage-trail, gold **Zeal overlay**, damage flash · ability bar with radial
  cooldown fills, keycaps, Faith costs · phase banner (slide/fade) · timer with
  phase ticks · seed label · pact icons row · bloodstain vignette (existing,
  smoothed from 4 hard steps to a lerp).
- **Pause:** real screen via ref-counted `PauseService` (fixes the current
  timeScale double-ownership between RunManager and GameOverUI).
- **Settings:** audio sliders on a real **AudioMixer** (Master/Music/SFX/UI —
  replaces the `AudioMute` hack) · difficulty (binds existing `GameSettings`) ·
  quality preset · key rebinding (after Input System cutover). PlayerPrefs-persisted.
- **Game over v2:** TMP prefab replacing the code-built overlay — outcome moment,
  stats grid, Embers earned, **Retry Same Seed / New Run / Campfire / Menu**.
- **Input:** migrate to Input System 1.7.0, phased ("Both" mode first, one consumer
  per PR, flip to New-only + rebind screen last). **Gamepad is first-class** — a
  Steam requirement.

## 8. Art & Audio Direction

**Look — "moonlit ink":** deep blue-black night, cool moonlight (the current red
directional becomes ~0.25-intensity cool blue, the sole shadow caster), warm
firelight accents (torch/shrine gold), **Zeal gold** as the sacred highlight color.

Achieved with: one-time **Linear color space** flip → already-installed
**post-processing v2** finally enabled (bloom threshold ~1.1 — this is what sells
tier-3 fake lights; FXAA; night color-grade with lifted blacks for readability;
vignette) → **Exp² fog enabled** (~0.018, deep blue-black — also the excuse for a
short far plane) → **procedural gradient night skybox + billboard moon**
(agent-written ~30-line shader; no skybox asset exists in the project). Stay on
**Built-in RP**; URP is re-evaluated only at the Steam milestone, bundled with a
deliberate Unity 6 upgrade as one migration. WebGL quality tier: pixelLightCount
2→4, hard shadows only, FXAA instead of MSAA.

**Audio** (current state: 4 loops — one a broken `.mov` — + 6 combat SFX, globally
muted by `AudioMute.cs`): delete the mute hack, build `AudioDirector` on the mixer.
**Music as phase-layered states** (menu / Dusk / Midnight / Blood Hour + Herald /
dawn sting / defeat) via mixer snapshots — the existing `encounter_loop` +
`tension_loop` seed it; ~6 more CC0 tracks to source. **SFX to source** (CC0 —
Kenney / Sonniss / freesound): UI clicks, footsteps, dodge whoosh, shrine chime,
pact whisper, Zeal shimmer, monster deaths, Herald roar, dawn choir. WebGL autoplay
stays gated behind the existing press-any-key intro.

## 9. Meta-Progression — "The Memory That Remains"

Every run ends at the **Campfire Before Dawn** (between-run screen; same diorama as
the menu): the crusader remembers what the god erases.

**Currency:** **Embers of Memory** = kills ×1 + shrines ×5 + pacts ×8 +
phase-reached bonus + first-dawn ×2.

**Three memory branches** (each node's flavor text is a retained memory):
- **Body** — +max Faith (5 × +5) · drain resist (3 × −5%) · Communion +10.
- **Arms** — +damage (5 × +4%) · dodge cooldown −0.1s ×3 · finisher +knockback.
- **Spirit** — 4th ability slot · Rite choice 2→3 · shrine potency +25% ·
  starting Rite · Zeal cap +25.

**Unlocks:** Deeper Nights I–V · Daily Dawn mode · Codex entries (bestiary /
rites / pacts lore — doubles as tutorialization). **Persistence:** JSON in
PlayerPrefs (WebGL-safe), Steam Cloud later. Built on the existing `RunManager`
stats seam.

**Narrative through mechanics:** the god's "gift" wipes memory (the roguelike
reset); Embers are what leaks through (meta); Pacts are the dark's counter-offer
(in-run corruption); Deeper Nights are the god testing a knight who remembers too
much. The original GDD's questioning-the-deity theme becomes systems, not cutscenes.

## 10. Content Ground Truth (what exists vs needs sourcing)

**Available in-project (verified):** 3 fully rigged melee monsters (wolf, troll,
hobgoblin) with idle/walk/run/attack/damage/dead clips (damage/dead currently
unwired) · crusader player with idle/run/3 attacks/shield bash/death clips (death
unwired; **no dodge/block/hit-react clips**) · ~40 medieval prop prefabs + ~23
consolidated sets + coins · 7 TerrainLayers + 18 ground textures · 4 music loops +
6 combat SFX · TMP with a fantasy display font · post-processing v2 installed
(unused) · 2 custom shaders (weapon glow, shiny particle).

**Must be sourced or built:** ranged/caster/boss-unique enemy models · dodge
animation (interim: capsule dash + trail) · skybox (interim: procedural shader) ·
UI/footstep/ambient/pickup SFX and ~6 music tracks (CC0) · store-page art (M6).

## 11. Technical Architecture (summary)

| Area | Decision |
|---|---|
| Movement | Dynamic Rigidbody + `rb.velocity` in FixedUpdate for player & enemies; FreezeRotation replaces the angular-drag-999999 hack; kills all `transform.position +=` writes |
| Layers | New Player/Enemy/Obstacle/Ground/Pickup/SensorZone/FX + collision matrix |
| Damage | `DamageInfo` struct + `IDamageable`; `OverlapSphereNonAlloc` + cone filter replaces `FindGameObjectsWithTag`; drain tagged as a source that skips hurt feedback |
| Enemy AI | `EnemyBrain` FSM (Wander⇄Chase→Windup→Strike→Recover, Staggered, Dead) + `EnemyMotor` steering; deletes `OnCollisionEnter` attack detection |
| Abilities | `AbilityDefinition` SO + `[SerializeReference]` effects; `AbilityCaster` + `PlayerStats` timed modifiers; deletes both legacy systems |
| Animators | Editor script via `AnimatorController` API wires existing Damage/Dead/Death clips — no hand-edited controllers |
| Procgen | Load-time pure-C# pipeline (§6) under `Assets/Scripts/World/` + editor preview window |
| Pooling | `UnityEngine.Pool.ObjectPool<T>`: monsters, VFX, damage numbers, one-shot audio |
| UI | `UIScreen`/`UIRouter` + own tweener; `PauseService` owns timeScale (ref-counted) |
| Input | Input System 1.7.0, phased Both→New behind a static `InputReader` facade |
| Rendering | Built-in RP + Linear + PPv2 + fog + procedural skybox; light-budget manager; URP deferred to Steam/Unity-6 as one bundled migration |
| Folders | `Assets/Scripts/{Player, Enemies, Combat, World, UI/{Tween,HUD,Screens}, Input, Core, Meta}` |

**Known defects fixed along the way:** player death instant-`Destroy` (dangling
HealthBar/camera references) · `MonsterMovement.pushTo` local/world-space bug
(deleted with steering) · `GameOverUI` legacy-Text → TMP · `SkillCD.cs` /
`UIcontroller.cs` class-name mismatches · broken `.mov` music track · 64MB TGA
textures (crunched during M2).

---

*Milestones, branch plans, verification checklists, and the agent-vs-human task
division live in [docs/ROADMAP.md](docs/ROADMAP.md).*
