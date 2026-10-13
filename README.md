# Meowtopia

Meowtopia is a virtual cat-care game inspired by Tamagotchi and older online pet communities. The goal is to give each cat its own appearance and personality, make care activities interactive, and eventually let players share their pets and connect with each other.

The current focus is a client-first Unity prototype for browser play: one cat, local progress, feeding, grooming, resting, and playing with a draggable toy. Accounts and social features are planned for a later stage.

## Project status

This repository brings together three early projects. They contain reusable artwork, models, screens, and gameplay experiments, but do not yet form a finished, integrated game.

The newer Unity 6 cat-care prototype is being developed in a separate working copy and has **not yet been imported into this monorepo**. The Unity project currently committed here still records Unity **2022.3.37f1**.

## Repository structure

| Folder | Purpose | Technology |
| --- | --- | --- |
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
2. The imported project's recorded Editor version is **2022.3.37f1**. The Unity 6 migration is ongoing in the separate prototype working copy.
3. Open `Assets/Scenes/00_Boot.unity` as the original entry scene.

The imported project currently lacks a committed `Packages/manifest.json`. Package configuration, including Unity UI and TextMesh Pro dependencies, needs to be restored before this checkout can be treated as a reproducible build.

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
