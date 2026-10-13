# Unity helpers

The active project is `../meowtopia-unity`, opened with Unity **6000.6.5f1**. The `meowtopia-unity-v2` folder is the preserved copy moved from Game Dev; use the main project for future edits.

Run commands from the repository root with PowerShell 7 (`pwsh`).

| Helper | Purpose |
| --- | --- |
| `unity-mcp-check.ps1` | Connect to the Unity MCP server at `http://127.0.0.1:8080/mcp` and read diagnostics or invoke tools. |
| `install-unity-screens.ps1` | Reinstall the original prototype scripts from `unity-screen-staging/` into the main project. These are snapshots; running it replaces the destination scripts. |
| `migrate-unity6.ps1` | Apply the earlier TextMesh Pro and home-screen compatibility fixes. The main project already includes these fixes. |
| `unity-screen-staging/` | Script snapshots required by the installer. Edit the main project's scripts for ongoing development. |

To read Unity console errors while the MCP server is running:

```powershell
pwsh -NoProfile -File .\helpers\unity-mcp-check.ps1 -Tool read_console -ArgumentsJson '{"action":"get","types":["error"],"count":10}'
```

The installer and migration helper accept `-ProjectPath` to target another project explicitly; otherwise they resolve the main project relative to this folder. Neither runs automatically.

The root `unity-*.log` files and `unity-import-plan.json` are historical migration output, separate from these helpers. Recovery backups are stored locally under `.local-recovery/` and ignored by Git.
