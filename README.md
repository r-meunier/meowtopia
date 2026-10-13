# Meowtopia

Meowtopia is a virtual cat-care game inspired by Tamagotchi and older online pet communities. The goal is to give each cat its own appearance and personality, make care activities interactive, and eventually let players share their pets and connect with each other.

The current focus is a client-first Unity prototype for browser play: one cat, local progress, feeding, grooming, resting, and playing with a draggable toy. Accounts and social features are planned for a later stage.

## Project status

This repository brings together three early projects. They contain reusable artwork, models, screens, and gameplay experiments, but do not yet form a finished, integrated game.

The Unity cat-care prototype is included in this monorepo and uses Unity **6000.6.5f1**. It includes loading and home screens, an animated Ragdoll, local saves, and a draggable yarn-ball activity. The earlier backend and mobile client remain separate prototypes.

## Repository structure

| Folder | Purpose | Technology |
| --- | --- | --- |
| [`helpers`](helpers/) | Unity diagnostics and earlier setup scripts | PowerShell |
| [`meowtopia-unity`](meowtopia-unity/) | Game scenes, artwork, local saves, needs, inventory, and economy experiments | Unity / C# |
| [`meowtopia-backend`](meowtopia-backend/) | Accounts, profiles, cats, breeds, shop items, inventory, and transactions | Java 22, Spring Boot 3.3.1, PostgreSQL |
| [`meowtopia-mobile-app`](meowtopia-mobile-app/) | Earlier mobile client with pet-care, inventory, shop, and profile screens | Expo SDK 51, React Native 0.74, React 18 |

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

From `meowtopia-backend`, run:

```sh
docker compose up -d db
```

Then start Spring Boot using the Maven wrapper.

Windows PowerShell:

```powershell
.\mvnw.cmd spring-boot:run
```

macOS / Linux:

```sh
./mvnw spring-boot:run
```

Database settings are in `src/main/resources/application.properties`; the Compose configuration is in `docker-compose.yml`. The existing development configuration uses `spring.jpa.hibernate.ddl-auto=create`, which recreates the schema on startup. Use a disposable development database with this configuration.

The application uses the default Spring Boot HTTP port, `8080`. These are development entry points; the imported backend has not been validated as a complete game service.

### Mobile client

From `meowtopia-mobile-app`, install the locked dependencies and start Expo:

```sh
npm ci
npm start
```

The client targets Expo SDK **51**. Its backend URL and prototype authentication settings are currently defined in `api/instance.js`. Configure those for your backend and device; a physical device needs a reachable server address rather than its own `localhost`.

Android, iOS, and web launch scripts are also present in `package.json`; platform compatibility and the web target still need validation.

## Git history

The three original repositories were imported with `git subtree` **without squashing**. Their original commits and history are retained in the combined `main` branch.

Original branches are preserved under:

- `archive/backend/...`
- `archive/mobile/...`
- `archive/unity/...`

The archive branches retain their original project layouts. Historical commits also retain their original paths; the project folder prefixes were added by the subtree import commits.

Future development can use ordinary commits and branches in this monorepo. A normal clone contains all three project folders.
