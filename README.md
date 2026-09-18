# Persistent Work Areas — Preview 1

Keep a building's working-area outline visible after deselecting it, including while painting crops, planting trees, or using other tools.

## Install

1. Close Timberborn.
2. Extract `PersistentWorkAreas-v0.1.0.zip` into your Timberborn `Mods` folder (normally `Documents\Timberborn\Mods`). The result should be `Mods\PersistentWorkAreas\version-1.1\manifest.json` and `PersistentWorkAreas.dll` beside it.
3. Start Timberborn and enable **Persistent Work Areas** in the mod manager. Restart if prompted.

Requires Timberborn **1.1.2.4** or a compatible 1.1 build. Built and checked against the installed 1.1.2.4 assemblies. Later versions may change the internal renderer API.

No extra dependency is required. Keep your existing BeaverBuddies Preview 6 installation as it is. Installing this mod on both players' computers is recommended for the first co-op test; pins are always local to each player. The code does not require the other player to install it, but asymmetric installation has not been playtested.

## Use

- Select a farm, forester, lumberjack flag, gatherer, or another building with a terrain/road-spill working range.
- Check **Keep working area visible**.
- Deselect it and use your planting/building tools. The outline remains.
- Pin additional buildings as needed. Overlapping pinned areas merge into a combined outline.
- Click **Clear pinned areas (N)** at the top right to remove every pin, without finding or selecting any building.
- For a keyboard shortcut, assign **Clear all pinned working areas** under **Persistent Work Areas** in the game's key-binding settings. It starts unbound to avoid taking an existing shortcut.
- To remove just one pin, select its building and uncheck the checkbox.

The currently selected building still uses its normal game outline. Clearing pins restores normal selection behavior; deselecting that building then hides its normal outline. Pins reset on map exit, loading, and multiplayer resynchronization. They are not saved. Deleting a pinned building removes its pin.

This preview supports navigation-based working areas. It does not pin district road coloring, every kind of effect-radius overlay, or building-placement ghosts. UI text is currently English.

## Compatibility and validation

Designed for **BeaverBuddies Stability Preview 6** (`1.1.0-stability.6`). The mod does not patch game methods, change simulation or building data, send multiplayer events, or modify saves. It uses the same navigation queries as the game's selected-building visualizer and a separate instance of its outline renderer.

Release build and 36 automated lifecycle/API checks passed. **Unity rendering, checkbox placement, and two-player operation have not yet been tested in-game.** See `VALIDATION.md` for scope and the short playtest checklist.

## Build from source

Install the .NET 8 SDK and have Timberborn installed, then run:

```powershell
.\build.ps1 -GameDir 'C:\Program Files (x86)\Steam\steamapps\common\Timberborn'
```

The script builds the mod, runs checks, and creates `dist\PersistentWorkAreas-v0.1.0.zip`. No game, Unity, Harmony, or BeaverBuddies DLLs are redistributed. The game DLLs are used only as build references.
