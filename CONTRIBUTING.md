# Contributing

Conventions for this repo — for the maintainer and for any Claude Code agents
working on it. Agents: read [CLAUDE.md](CLAUDE.md) too.

## Golden rule

**`main` is always deployable.** Every push to `main` auto-builds the WebGL player
and deploys it to GitHub Pages. Never commit directly to `main` — always work on a
branch and merge via a pull request that has a green build.

## Branching

- Branch off the latest `main`.
- Name branches `<type>/<short-kebab-description>`, where `<type>` matches the commit
  types below. Examples:
  - `feat/victory-screen`
  - `fix/spawn-inside-walls`
  - `refactor/ability-system`
  - `docs/add-conventions-and-roadmap`
- **One logical change per branch.** Keep PRs small and reviewable.
- After merge, delete the branch.

## Commit messages — Conventional Commits

Format:

```
<type>(<scope>): <imperative summary, lowercase, no trailing period>

<optional body: what changed and, more importantly, why>

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>   # on agent commits only
```

Rules:
- **Subject ≤ 72 chars**, imperative mood — "add", not "added" or "adds".
- One logical change per commit.
- `<scope>` is optional but encouraged.

### Types

| Type | Use for |
|---|---|
| `feat` | A new gameplay feature or player-facing capability |
| `fix` | A bug fix |
| `refactor` | Code restructuring with no behavior change |
| `perf` | A performance improvement |
| `docs` | Documentation only |
| `chore` | Tooling, deps, housekeeping (no src/gameplay change) |
| `test` | Adding or fixing tests |
| `ci` | CI/workflow changes |
| `build` | Build system / project-settings changes |
| `style` | Formatting only, no logic change |

### Scopes (project-specific)

`combat`, `abilities`, `enemies`, `cycle`, `ui`, `camera`, `world`, `shaders`,
`meta`, `ci`, `deploy`. Add new scopes as new systems appear.

### Examples

```
feat(cycle): add victory screen with run summary
fix(enemies): stop monsters spawning inside walls
refactor(abilities): unify inline and holder systems into ScriptableObjects
perf(combat): cache monster list instead of FindGameObjectsWithTag per attack
docs: document WebGL deployment steps
```

## Pull requests

1. Push your branch and open a PR against `main`.
2. Wait for the **PR build**
   ([.github/workflows/pr-build.yml](.github/workflows/pr-build.yml)) to pass — it
   compiles the WebGL build. Since there are no unit tests, this is the automated gate.
3. **Squash-merge.** This keeps `main` as one clean Conventional Commit per feature;
   make the PR title a valid Conventional Commit subject (it becomes the squash commit).
4. Delete the branch.

## Unity-specific notes

- Commit the `.meta` file alongside every asset/script you add, move, or delete.
- Avoid hand-editing scene/prefab/asset YAML; prefer C# changes.
- Some behavior is wired via `UnityEvent`s in the Inspector — a code change may need a
  matching manual wiring step in the editor. Call this out in the PR description.
