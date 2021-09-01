# BabyCoyote Mod

A mod for the game [Baby Coyote]

[Baby Coyote]: https://store.steampowered.com/app/1367780/Baby_Coyote/

## Install

Download the [latest release] and extract into the game folder. `doorstop_config.ini`, `version.dll` and the `mod`
sub-folder must end up in the same folder as `Baby Coyote.exe`.

The mod remains disabled by default. To enable it, either
- edit `doorstop_config.ini` and replace `enabled=false` with `enabled=true` or
- add `--doorstop-enable true` to the game's launch options in Steam

Edit `mod.config` to enable or disable the features you want.

The mod uses the [Doorstop] and [HarmonyX] libraries to inject code into the game without modifying game files.

[latest release]: https://github.com/kalimag/BabyCoyote-Mod/releases
[Doorstop]: https://github.com/NeighTools/UnityDoorstop/
[HarmonyX]: https://github.com/BepInEx/HarmonyX/

## Features

- Make level 6-2 beatable by adjusting a death plane that covers a required teleport exit
- Restore three never-before-seen cutscenes to delve even deeper into the lore of Baby Coyote
- Skippable cutscenes
- Quick retry after dying
- Hotkeys for a number of cheats/shortcuts including
  - Healing, Invincibility
  - Teleporting
  - Unlocking all levels
  - Restart level and level select
- Free camera (move, zoom)
- Show location of death planes

All can be individually enabled/disabled in the `mod.config` file

## Minimal patch

Alternatively to the full mod, there is also a minimal binary patch included which
only adjusts the location of the death plane in 6-2 to make it beatable and does
not inject code or contain any other changes. 

## Hotkeys

| Hotkey         | Function                                       | Required `mod.config` entry |
|----------------|------------------------------------------------|-----------------------------|
| Escape         | Skip cutscene                                  | AllowCutsceneSkipping       |
| Ctrl+R         | Restart level                                  | Cheats                      |
| Ctrl+F         | Finish current level                           | Cheats                      |
| Ctrl+L         | Level select                                   | Cheats                      |
| Ctrl+U         | Unlock all levels                              | Cheats                      |
| F5             | Save current position as teleport              | Cheats                      |
| F9             | Teleport to saved position                     | Cheats                      |
| Left shift     | Fly upwards                                    | Cheats                      |
| H              | Restore health                                 | Cheats                      |
| Ctrl+I         | Toggle invincibility (damage and death planes) | Cheats                      |
| Numpad +       | Zoom camera in                                 | Camera                      |
| Numpad -       | Zoom camera out                                | Camera                      |
| Numpad 2/4/6/8 | Move camera                                    | Camera                      |
| Numpad 0       | Reset camera                                   | Camera                      |
| Numpad Enter   | Switch between free camera and normal camera   | Camera                      |
| Ctrl+V         | Show location of death planes                  | Visuals                     |
