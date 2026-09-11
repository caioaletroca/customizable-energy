# Troubleshooting

## The mod does not appear in the Mods menu

1. Confirm the mod folder contains `CustomizableEnergy.dll`, `mod.yaml`, and `mod_info.yaml`.
2. Make sure ONI is fully closed before replacing local mod files.
3. Start ONI, enable the mod, and restart when prompted.

## The mod is marked out of date

Use a version built for your ONI game build. The current development build targets U59-737790.

## A generator is missing from the options list

The generator must be registered by ONI as a building that produces power. Restart ONI after enabling both Customizable Energy and the mod that adds the generator.

Some power-related buildings, such as transformers or batteries, may appear if ONI reports them as a power output. Leave their override set to OFF unless you intentionally want to change that behavior.

## A setting does not apply

1. Open Customizable Energy options.
2. Make sure the generator button reads ON.
3. Enter a wattage between 0 and 100,000.
4. Press OK.
5. Restart ONI when prompted.

## Restore normal output

Open the options window, select Reset to Default, press OK, and restart ONI. This disables every override and restores original generator wattages.

## Reporting a problem

Include the following information:

- ONI build number and enabled DLC.
- Customizable Energy version.
- The generator building ID or name.
- Your configured ON/OFF state and wattage.
- Whether the generator is from the base game, DLC, or another mod.
- `Player.log` from `%USERPROFILE%\AppData\LocalLow\Klei\Oxygen Not Included`.
- Steps that reproduce the issue.
