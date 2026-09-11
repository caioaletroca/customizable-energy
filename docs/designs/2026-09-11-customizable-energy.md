# Customizable Energy -- Design Document

> **Status:** Approved
> **Date:** 2026-09-11
> **Exploration:** 3 alternatives evaluated

## Context

Customizable Buildings allows changing the Steam Generator output, but does not expose all power generators. This mod will discover generator buildings generically, including DLC and compatible third-party generators, and allow their output power to be overridden through Mod Manager.

## Architecture

A standalone C# Oxygen Not Included mod scaffolded from `O-n-y/OxygenNotIncludedModTemplate`. Harmony will observe initialized building definitions and identify generator buildings at runtime. Mod Manager/ONY Lib's latest available configuration integration will provide persisted settings and the in-game configuration UI.

## Components

| Component | Type | Purpose |
|---|---|---|
| `CustomizableEnergyMod` | new | Loads Harmony patches and registers mod metadata |
| `GeneratorDiscovery` | new | Finds generator building definitions and stable building IDs |
| `GeneratorPowerPatch` | new | Applies configured wattage after definitions initialize |
| `EnergySettings` | new | Stores per-building enable flags and wattage values |
| Mod Manager config integration | new | Displays discovered generators and saves user values |
| `mod_info.yaml` / project files | new | ONI metadata and build configuration |

## Data Flow

1. ONI loads building configuration classes.
2. A Harmony patch observes initialized building definitions.
3. Discovery identifies definitions exposing generator behavior and wattage.
4. Each generator receives a stable ID, display name, original wattage, and persisted settings.
5. Mod Manager displays an `Enable overwrite` toggle and wattage value for each generator.
6. Wattage values are clamped to 0–100,000 W.
7. Enabled entries overwrite `BuildingDef.GeneratorWattageRating`; disabled entries retain the original value.
8. Existing and newly created generator buildings use the resulting output.

## Key Decisions

| Decision | Chosen | Rejected Alternative | Why |
|---|---|---|---|
| Generator selection | Runtime discovery | Hardcoded generator registry | Supports DLC and compatible mod generators |
| Configuration granularity | Per building ID | One global wattage | Gives precise control |
| Configuration UI | Latest Mod Manager/ONY Lib integration | Separate custom UI dependency | Matches the requested workflow |
| Override behavior | Per-generator enable toggle | Always overwrite discovered generators | Preserves vanilla/mod defaults unless requested |
| Value range | 0–100,000 W | Unrestricted values | Matches the Dev Generator maximum and prevents invalid values |
| Default value | Original discovered wattage | Fixed mod defaults | Preserves vanilla and other mod behavior |
| Patch strategy | Post-initialization definition modification | Per-generator handwritten patches | Reduces maintenance and supports future generators |

## Open Questions

- Confirm the latest Mod Manager/ONY Lib configuration API and registration pattern during implementation.
- Confirm the runtime property or component that reliably distinguishes all generators from non-generator buildings.
- Confirm the correct initialization hook and timing across current ONI and DLC versions.
- Determine how to handle duplicate building IDs or generators added after initial discovery.

## Alternatives Considered

1. **Direct Harmony patches plus configuration:** simpler, but requires manually maintaining every generator class and would not reliably support mod-added generators.
2. **Generic building-definition discovery:** selected because it supports DLC and compatible mod generators with one centralized mechanism.
3. **Hybrid known-generator registry plus fallback:** rejected as unnecessary complexity for the initial implementation; runtime discovery already provides the desired extensibility.
