# Playing the game in the browser (WebGL)

Unity can compile this project to **WebGL** — a set of static files (`index.html`, a
`Build/` folder, and `TemplateData/`) that any static host can serve. GitHub Pages
hosts them for free, so anyone can play the game from a link, no Unity install
needed. This is exactly how the browser-playable version at university worked.

There are two ways to do it. **Option A (automated)** is set up and ready — it just
needs a free Unity license added as a secret. **Option B (manual)** needs no setup
but you rebuild by hand each time.

---

## Option A — Automated build & deploy (recommended)

The workflow at [`.github/workflows/deploy-webgl.yml`](../.github/workflows/deploy-webgl.yml)
builds WebGL with [GameCI](https://game.ci) and publishes it to GitHub Pages on
every push to `main`. One-time setup:

### 1. Enable GitHub Pages via Actions
Repo → **Settings → Pages → Build and deployment → Source → GitHub Actions**.

### 2. Add your Unity credentials as secrets
Repo → **Settings → Secrets and variables → Actions → New repository secret**. Add:

| Secret | Value |
|---|---|
| `UNITY_EMAIL` | The email for your Unity account |
| `UNITY_PASSWORD` | Your Unity account password |
| `UNITY_LICENSE` | The contents of your `.ulf` license file (see below) |

A free **Unity Personal** license is fine. To get the `.ulf`:

1. In the **Actions** tab, the first build will fail without a license. Instead,
   generate an activation file. The simplest path is GameCI's helper:
   run the [`unity-request-activation-file`](https://game.ci/docs/github/activation)
   action once (or follow the linked docs) to produce a `Unity_v20XX.alf` artifact.
2. Go to https://license.unity3d.com/manual, upload the `.alf`, and download the
   resulting `Unity_v20XX.ulf`.
3. Open the `.ulf` in a text editor, copy **all** its contents, and paste them as
   the value of the `UNITY_LICENSE` secret.

Full walkthrough: https://game.ci/docs/github/activation

### 3. Push
Any push to `main` (or a manual run via **Actions → Build & Deploy WebGL → Run
workflow**) now builds and deploys. The live URL appears under
**Settings → Pages** and on the workflow's `deploy` job, typically:

```
https://unsf5294.github.io/before-dawn-survival/
```

> First WebGL build takes ~15–25 min (cold Library cache); later builds are faster.

---

## Option B — Manual build (no secrets, no CI)

If you'd rather not deal with Unity licensing on CI, build locally and push the
output.

1. In Unity: **File → Build Settings → WebGL → Switch Platform**, then **Build**.
   Save the output to a folder, e.g. `webgl-build/`.
2. Publish that folder to a `gh-pages` branch. From the repo root:
   ```bash
   git switch --orphan gh-pages
   git rm -rf .
   cp -r /path/to/webgl-build/* .
   git add .
   git commit -m "Deploy WebGL build"
   git push -u origin gh-pages
   git switch main
   ```
3. Repo → **Settings → Pages → Source → Deploy from a branch → `gh-pages` / root**.

The game goes live at `https://unsf5294.github.io/before-dawn-survival/`.

> Keep the WebGL build out of `main` — it's large generated output. The `gh-pages`
> branch (or Option A's Actions artifact) is the right home for it.

---

## Tips for a smoother WebGL build

- **Compression**: Unity's default Brotli/Gzip compression can trip up some static
  hosts. GitHub Pages handles Gzip fine; if you see load errors, set
  **Project Settings → Player → WebGL → Publishing Settings → Compression Format**
  to `Disabled` or `Gzip`.
- **Build size**: this project ships several large texture packs. Consider crunching
  textures and stripping unused assets to keep load times reasonable.
- **Memory**: if the browser tab runs out of memory, lower the WebGL memory size in
  Player settings.
