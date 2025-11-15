# Gloomhaven Bug Fixes
This mod currently fixes a small number of bugs
- Fixes a crash at end of turn when using item number 150 (solo scenario campaign item for class #17)
- Fixes a bug with choosing not to use the ability from a level 9 card for the sun class
- Fixes a multiplayer-only bug with action selection in scenario 81
- Fixes a bug that would cause modded save files to be named incorrectly and "disappear"
- Fixes a crash that would happen if a summon tried to summon
- Enables the debug menu when started with `-debug`

## Known Issues
If another player grants an extra turn to class #17, and the item is used after using the action from one card (as opposed to both cards), the game will still crash.

## Installation
1. Install BepInEx 5 (Stable) ([Windows/SteamDeck/Linux](https://docs.bepinex.dev/articles/user_guide/installation/index.html?tabs=tabid-win#where-to-download-bepinex) | [Mac](https://docs.bepinex.dev/articles/user_guide/installation/index.html?tabs=tabid-nix#where-to-download-bepinex))
2. Download `BugFixes.dll` from the [latest release](https://github.com/gummyboars/gloomhaven-bugfixes/releases)
3. Copy `BugFixes.dll` to the `BepInEx/plugins/` folder
4. SteamDeck/Proton/Wine users only: [Configure wine](https://docs.bepinex.dev/articles/advanced/proton_wine.html) for BepInEx

## Compatibility
This mod must be installed for all players in a multiplayer game. If players without the mod attempt to join a game hosted by a player with the mod (or vice versa), they will get a game version error.

## Other Mods
You may be interested in other mods for Gloomhaven:
- [Camera Tweaks](https://github.com/gummyboars/gloomhaven-camera)
- [UI Tweaks](https://github.com/gummyboars/gloomhaven-uitweaks)
- [Fast Load](https://github.com/gummyboars/gloomhaven-fastload)
