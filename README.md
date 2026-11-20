# Meowtopia

Meowtopia is a virtual cat-care game inspired by Tamagotchi and older online pet communities. The goal is to give each cat its own appearance and personality, make care activities interactive, and eventually let players share their pets and connect with each other.

The current focus is a client-first Unity prototype for browser play: one cat, local progress, feeding, grooming, resting, and playing with a draggable toy. Accounts and social features are planned for a later stage.

## Project status

This repository brings together several early projects. They contain reusable artwork, models, screens, and gameplay experiments, but do not yet form a finished, integrated game.

The Unity cat-care prototype is included in this monorepo and uses Unity **6000.6.5f1**. It includes loading and home screens, an animated Ragdoll, local saves, and a draggable yarn-ball activity. The earlier backend and mobile client remain separate prototypes.

## Repository structure

| Folder | Purpose | Technology |
| --- | --- | --- |
| [`helpers`](helpers/) | Unity diagnostics and earlier setup scripts | PowerShell |
| [`meowtopia-unity`](meowtopia-unity/) | Game scenes, artwork, local saves, needs, inventory, and economy experiments | Unity / C# |
| [`meowtopia-backend`](meowtopia-backend/) | Accounts, profiles, cats, breeds, shop items, inventory, and transactions | Java 22, Spring Boot 3.3.1, PostgreSQL |
| [`meowtopia-mobile-app`](meowtopia-mobile-app/) | Earlier mobile client with pet-care, inventory, shop, and profile screens | Expo SDK 51, React Native 0.74, React 18 |
| [`meowtopia-frontend`](meowtopia-frontend/) | Early web client from July 2024; still close to the Vite starter, with a header component | React 18, TypeScript, Vite |
| [`meowtopia-godot`](meowtopia-godot/) | Early 2D prototype with placeholder character and terrain sprites | Godot 4.2 (mobile renderer) |
| [`resources`](resources/) | Source artwork: cats, foods, drinks, objects and bowls, status icons, and line-icon drafts | PNG, JPEG, AI, PSD |

There is no shared root build command. Each project has its own setup and dependencies.

## Gameplay concepts

The backend captures much of the original game model:

- **Care:** hunger, thirst, energy, happiness, cleanliness, and health.
- **Personality:** playfulness, affection, curiosity, independence, laziness, and intelligence.
- **Identity:** name, breed, fur and eye colours, gender, weight, and neutered status.
- **Aging:** life stages from baby and kitten through adulthood and old age.
- **Items:** food with several stat effects, flavoured drinks, toys, prices, stock, and inventory quantities.
- **Ownership:** multiple cats per account, player profiles, and transaction records.

These models express design ideas; several behaviours are incomplete or inconsistent. Personality-driven behaviour, the aging timeline, currency spending, and social interactions still need implementation or revision.

## Getting started

Clone the monorepo normally; no submodule initialization is required:

```sh
git clone https://github.com/r-meunier/meowtopia.git
cd meowtopia
```

### Unity

1. Add `meowtopia-unity` as a project in Unity Hub.
2. Open it with Unity **6000.6.5f1** and allow package resolution and asset import to complete.
3. Open `Assets/Scenes/Meowtopia_Loading.unity` and press **Play**. The loading scene prepares local progress and opens the home screen.
4. Click **Play** in the game to drag the yarn ball near the cat; choose **Done playing** to end the activity.

Package manifests and asset metadata are included. See the [Unity README](meowtopia-unity/README.md) for controls and development checks. Browser delivery remains the target; a WebGL build has not yet been validated.

### Backend

Requirements: JDK **22** and a PostgreSQL database. Docker Compose can start the development database supplied with the project.

From `meowtopia-backend`, create your local credentials file, then start the database:

```sh
cp .env.example .env   # then set your own passwords in .env
docker compose up -d db
```

`.env` is git-ignored. Spring Boot and Docker Compose both read it.

Then start Spring Boot using the Maven wrapper.

Windows PowerShell:

```powershell
.\mvnw.cmd spring-boot:run
```

macOS / Linux:

```sh
./mvnw spring-boot:run
```

Database settings are in `src/main/resources/application.properties`; the Compose configuration is in `docker-compose.yml`. On startup the application runs `src/main/resources/schema.sql` (`spring.sql.init.mode=always`), and Hibernate schema generation is off (`ddl-auto=none`). The script uses plain `CREATE TABLE` statements, so startup fails if the tables already exist. Recreate the development database before starting again:

```sh
docker compose down && docker compose up -d db
```

The Compose database keeps no volume, so `down` discards all data. Use it only as a disposable development database.

The application uses the default Spring Boot HTTP port, `8080`. These are development entry points; the imported backend has not been validated as a complete game service.

### Mobile client

From `meowtopia-mobile-app`, create your local settings file, install the locked dependencies and start Expo:

```sh
cp .env.example .env   # then set the API address and test account in .env
npm ci
npm start
```

The client targets Expo SDK **51**. `api/instance.js` reads the backend URL and the prototype test-account credentials from `.env`, which is git-ignored. Expo embeds `EXPO_PUBLIC_*` values in the app bundle, so use test credentials only. A physical device needs a reachable server address rather than its own `localhost`.

Android, iOS, and web launch scripts are also present in `package.json`; platform compatibility and the web target still need validation.

### Web client

From `meowtopia-frontend`, install the locked dependencies and start the Vite dev server:

```sh
npm ci
npm run dev
```

The web client is not connected to the backend yet.

## Git history

The three original repositories were imported with `git subtree` **without squashing**. Their original commits and history are retained in the combined `main` branch.

Original branches are preserved under:

- `archive/backend/...`
- `archive/mobile/...`
- `archive/unity/...`

The archive branches retain their original project layouts. Historical commits also retain their original paths; the project folder prefixes were added by the subtree import commits.

`meowtopia-frontend`, `meowtopia-godot`, and the artwork in `resources` had no earlier Git history. They were added as plain snapshots.

Future development can use ordinary commits and branches in this monorepo. A normal clone contains every project folder.
