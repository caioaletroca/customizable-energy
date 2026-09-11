# Customizable Energy Implementation Plan

> **For implementers:** This is a rolling-wave plan. Phase 1 tasks are
> dispatch-ready. Later phases are epic-level only -- elaborate them against
> the real codebase when execution reaches them. This document is the living
> source of truth -- task elaboration for later phases is written back into it
> during execution.

**Goal:** Build an ONI mod that discovers power generators, including modded generators, and lets players independently enable and configure output wattage through Mod Manager.

**Architecture:** Start from a maintained ONI C# mod template. Use Harmony around the common building-definition creation path, discover definitions with generator behavior at runtime, retain each original wattage as the default, and apply per-building overrides only when enabled. Use the latest compatible Mod Manager/ONY Lib configuration API discovered during implementation.

**Tech Stack:** C#, .NET Framework/MSBuild, Oxygen Not Included KMod API, Harmony, Mod Manager/ONY Lib configuration APIs. Verification is intentionally lightweight: compilation, assembly/API inspection, logs, and focused in-game smoke tests; no broad C# test harness is required.

## Phase Overview

| Phase | Milestone | Epics | Status |
|---|---|---|---|
| 1 | Buildable mod skeleton and verified API spike | 1.1, 1.2 | Detailed |
| 2 | Runtime discovery and wattage overrides work | 2.1, 2.2 | Epic-level |
| 3 | Mod Manager UI, persistence, packaging, and compatibility validation complete | 3.1, 3.2, 3.3 | Epic-level |

---

### Epic 1.1: Project scaffold and build baseline

**Goal:** The repository contains a buildable ONI mod assembly loaded by the game mod system.
**Scope:** Project file, mod entry point, metadata, build configuration, dependency references.
**Dependencies:** none
**Done when:** MSBuild produces the mod DLL and ONI recognizes the mod metadata without compilation errors.
**Status:** Pending

#### Task 1.1.1: Scaffold the maintained ONI mod template

- [ ] Done

**Context:** The workspace is empty; there are no existing source files, project files, tests, or `.rigor/config.yaml`. The approved design selects `O-n-y/OxygenNotIncludedModTemplate` as the starting point.

**Implementation vision:** Obtain the template at its current version rather than copying stale snippets. Preserve its project layout and dependency conventions. Configure assembly metadata, `mod_info.yaml`, supported base game/DLC declarations, and output paths for a local ONI mods directory or a clearly documented build artifact. Do not add comments to source files.

**Files:**
- Create: template-defined project and source files under the repository root
- Create: `mod_info.yaml`
- Create: template-defined build/configuration files

**Verification:** Run the template's documented MSBuild command; expected result is a successful build producing one mod DLL and no compiler errors.

**Done when:** A clean checkout builds the minimal mod assembly and contains valid ONI mod metadata for base game and supported DLC.

#### Task 1.1.2: Confirm current runtime and configuration contracts

- [ ] Done

**Context:** The design intentionally leaves the latest Mod Manager/ONY Lib API and generic generator-detection hook as implementation learning goals. The workspace has no local API references yet.

**Implementation vision:** Inspect the current Mod Manager/ONY Lib source or packaged assemblies, the selected template, and current ONI assemblies. Record exact namespaces, configuration registration methods, persistence behavior, building-definition lifecycle hooks, and a reliable generator-identification signal. Verify whether the API is a compile-time dependency or an optional runtime integration, and choose the latest compatible contract without inventing a custom UI.

**Files:**
- Modify: project dependency/reference files created by Task 1.1.1
- Create: a small source/API spike in the template's established source directory, if needed
- Create: implementation notes in the plan during execution

**Verification:** Run the build after adding only confirmed references and the minimal entry-point/API spike; expected result is successful compilation against the selected ONI and Mod Manager/ONY Lib versions.

**Done when:** The exact configuration API and generator-definition hook are evidenced by compilable code or a reproducible assembly inspection, and the dependency choice is recorded in this plan.

---

### Epic 1.2: Minimal mod lifecycle

**Goal:** The mod loads through `KMod.UserMod2`, installs Harmony safely, and can be enabled alongside Mod Manager.
**Scope:** Mod entry point and patch registration.
**Dependencies:** Epic 1.1
**Done when:** ONI logs successful mod initialization and no patch is installed twice during a normal startup.
**Status:** Pending

#### Task 1.2.1: Add the mod entry point and empty patch registration

- [ ] Done

**Context:** No entry point exists. The selected template establishes the expected `KMod.UserMod2.OnLoad(Harmony harmony)` lifecycle.

**Implementation vision:** Follow the template's entry-point style. Create a uniquely named Harmony ID, install the patch class through the supplied Harmony instance, and ensure initialization is idempotent. Keep generator behavior unchanged in this task so the baseline isolates loading and dependency issues.

**Files:**
- Create: template-defined source path `CustomizableEnergyMod.cs`
- Create: template-defined source path for patch registration

**Verification:** Run MSBuild; expected result is a successful build. If an ONI installation is available, launch with the mod enabled and verify the player log contains no load, missing-reference, or duplicate-patch errors.

**Done when:** The mod assembly loads through KMod and Harmony registration completes without changing game behavior.

---

### Epic 2.1: Generic generator discovery

**Goal:** All generator building definitions available at initialization are identified with stable IDs, labels, and original wattage values, including DLC and compatible mod generators.
**Scope:** Runtime discovery and definition lifecycle patching.
**Dependencies:** Phase 1
**Done when:** A diagnostic validation path demonstrates discovery of every supported vanilla/DLC generator and preserves each original wattage; unrelated buildings are excluded.
**Status:** Pending

### Epic 2.2: Configured wattage application

**Goal:** Enabled generator entries receive a clamped 0–100,000 W output while disabled entries retain their original definition value.
**Scope:** Settings model, override application, duplicate/discovery timing safeguards.
**Dependencies:** Epic 2.1
**Done when:** Overrides apply deterministically, defaults equal discovered values, values clamp at both bounds, and zero is accepted.
**Status:** Pending

### Epic 3.1: Mod Manager configuration UI and persistence

**Goal:** Mod Manager presents one `Enable overwrite` toggle and wattage setting per discovered generator, persists changes, and restores them across restarts.
**Scope:** ONY Lib/Mod Manager integration and settings serialization.
**Dependencies:** Epic 2.2
**Done when:** A user can change, save, reload, disable, and re-enable each generator independently without losing original defaults.
**Status:** Pending

### Epic 3.2: Packaging and compatibility metadata

**Goal:** The release artifact is installable through Steam/local mods and declares current base-game/DLC compatibility.
**Scope:** Build output, metadata, versioning, packaging instructions.
**Dependencies:** Epic 3.1
**Done when:** A clean build produces the distributable layout and ONI accepts it with Mod Manager enabled.
**Status:** Pending

### Epic 3.3: Verification and release hardening

**Goal:** The mod is validated against vanilla generators, DLC generators, modded generators, missing optional Mod Manager dependencies, and game reload behavior.
**Scope:** Test harness or manual verification matrix, logs, error handling, final documentation.
**Dependencies:** Epic 3.2
**Done when:** The compatibility matrix passes, no unrelated building output changes, and lint/build checks are clean.
**Status:** Pending

## Self-review

- Spec coverage: generic discovery (2.1), DLC/mod generators (2.1), per-building settings (3.1), enable toggles (3.1), defaults from existing definitions (2.2), 0–100,000 clamp (2.2), and Mod Manager integration (3.1).
- Detailed-wave vagueness: Task 1.1.2 is an explicit research spike because the approved design identifies API and lifecycle details as open questions; it has concrete inspected artifacts and a build verification requirement.
- Phase boundaries: Phase 1 builds and loads; Phase 2 changes behavior; Phase 3 completes UI, packaging, and compatibility validation.
- Configuration: no `.rigor/config.yaml` exists, so default verification is used; project-native lint/typecheck commands will be determined once the template is scaffolded.
