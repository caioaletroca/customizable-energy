# Customizable Energy

Configure the output power of Oxygen Not Included generators.

## Features

- Discovers compatible base-game, DLC, and mod-added generators automatically.
- Provides an independent **ON/OFF** override for every discovered generator.
- Lets you set each enabled generator from **0 W** to **100,000 W**.
- Keeps the original generator wattage unless its override is enabled.
- Saves settings between game restarts.
- Includes **Reset to Default** to restore original wattages and disable every override.

## Compatibility

- Oxygen Not Included build **U58-722606** or newer.
- Base game and Spaced Out! are supported.
- Compatible generator buildings added by other mods are discovered when ONI registers them.
- Existing generators retain their normal behavior until you enable an override.

## How to use

1. Enable **Customizable Energy** in the ONI Mods menu and restart the game when prompted.
2. Select **Options** for Customizable Energy.
3. Find the generator you want to adjust.
4. Press its **OFF** button to turn it **ON**.
5. Enter the desired output in watts.
6. Press **OK** and restart ONI when prompted.
7. Build or inspect the generator in-game to confirm the new output.

Changes are applied only to entries set to **ON**. An **OFF** entry continues to use the wattage supplied by ONI or the generator's original mod.

## Resetting settings

Select **Reset to Default** in the options window to restore every discovered generator to its original wattage and turn all overrides off. Press **OK** to save the reset, then restart if prompted.

## Configuration file

The mod saves its settings in:

`%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\dev\CustomizableEnergy\config.json`

Steam Workshop releases store their configuration in the installed mod folder instead. Use the in-game options screen whenever possible; edit the file only while ONI is closed.

## Troubleshooting

See [docs/TROUBLESHOOTING.md](docs/TROUBLESHOOTING.md) for common problems and reporting details.

## Planned Steam Workshop release

This project is being prepared for Steam Workshop publication. The current local/dev build is intended for testing until a Workshop release is available.
