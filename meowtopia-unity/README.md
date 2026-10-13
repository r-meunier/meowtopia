# Meowtopia — Unity prototype

A client-first virtual cat-care game built with Unity **6000.6.5f1** and uGUI / TextMesh Pro.

## Open and play

1. In Unity Hub, add this `meowtopia-unity` directory from the monorepo.
2. Open it with Unity 6000.6.5f1. Wait for package resolution and script compilation.
3. Open `Assets/Scenes/Meowtopia_Loading.unity`.
4. Press **Play** in the Editor.

The loading scene initializes the local save and opens `Meowtopia_Home`. The older `00_Boot`, cat-creation, and other experimental scenes are retained.

## Controls

- **Feed:** replenish food.
- **Play:** start a yarn-ball session. Drag the ball with the mouse or touch near the cat to gradually increase happiness and spend energy. A stationary or distant toy gives no reward.
- **Done playing:** finish the session and save its progress. Play also ends when the cat is tired.
- **Brush:** improve cleanliness.
- **Rest:** replenish energy.
- **Click the cat:** pet it outside a play session.
- **Menu icon:** open Save progress and Back to cat controls.

Progress is stored locally through `SaveSystem`. The cat has a stable body pose, gentle breathing, blinking, and eyes that follow the toy during play. Eye coordinates are currently fitted to the Ragdoll artwork.

## Code and artwork

| Location | Purpose |
| --- | --- |
| `Assets/Scripts/Screens/` | Loading, home UI, cat animation, toy dragging, and play-session logic |
| `Assets/Scripts/Core/` | Existing save models, persistence, economy, and shared services |
| `Assets/Editor/MeowtopiaScreenSetup.cs` | Screen setup and Editor smoke-check menu commands |
| `Assets/Art/Pets/Ragdoll/` | Ragdoll sprite sheet and preview sprite |
| `Packages/` | Package manifest and resolution lock file |
| `ProjectSettings/` | Unity version, build scenes, and project configuration |

## Development checks

With Home running in Play mode, the **Meowtopia** menu provides:

- **Check interactive play:** validates pointer dragging, rewards, distance/idle rejection, cleanup, saving, exhaustion, and toy gaze-target setup. Test stats are restored afterward.
- **Check toy eye tracking:** checks left/right gaze and return to centre.
- **Check animation stability:** checks the body sprite, position, and rotation over an idle cycle.
- **Check care buttons in Play mode:** checks care controls, displayed needs, saving, menu controls, and loaded animation frames.

## Version control

Commit `Assets`, including `.meta` files, `Packages`, and `ProjectSettings`. Unity's metadata preserves GUIDs used by scenes, scripts, materials, and sprites.

`Library`, temporary files, local Editor settings, build output, and diagnostic screenshots are excluded. Work in this monorepo directory so future edits are captured by its Git history.

The backend and Expo projects are not connected to this prototype yet. Browser delivery is the intended direction; WebGL export and browser persistence still need validation.
