# CI/CD and Steam Workshop publishing

## GitHub-hosted release pipeline

The workflows run on GitHub-hosted `windows-latest` runners. SteamCMD downloads Oxygen Not Included into the temporary runner workspace so the mod can compile against the current Managed assemblies.

Configure these repository secrets:

| Secret | Purpose |
|---|---|
| `STEAM_USERNAME` | Steam account name used to download ONI through SteamCMD. |
| `STEAM_CONFIG_VDF` | Base64-encoded authenticated SteamCMD `config.vdf`. |

`STEAM_WORKSHOP_ITEM_ID` is no longer used by CI because ONI Workshop publishing requires Klei's official uploader.

## Build workflow

`.github/workflows/build.yml` runs for pushes to `main` and manual dispatch. It downloads ONI, builds the mod, and uploads a test artifact.

## Semantic release workflow

`.github/workflows/publish.yml` runs on pushes to `main`. Semantic-release reads Conventional Commit messages, chooses the next version, updates `mod_info.yaml`, creates the GitHub release, builds the mod, and attaches an ONI Uploader-ready ZIP.

- `feat`: minor release
- `fix`, `perf`, `refactor`, or `ci`: patch release
- A breaking-change footer: major release

If no commit requires a version bump, no release artifact is created.

## Publishing to Steam Workshop

Steam Workshop publication is manual because ONI uses Klei's legacy single-file Workshop format. Generic SteamCMD `workshop_build_item` uploads are not compatible with ONI's importer.

For each release:

1. Download `CustomizableEnergy-vX.Y.Z.zip` from the GitHub release.
2. Extract it to an empty folder.
3. Open **Oxygen Not Included Uploader** from the Steam Library tools.
4. Select the extracted folder.
5. Select the existing Customizable Energy Workshop item, or create a new item if replacing an incompatible item.
6. Use `Steam/preview.png` as the preview image.
7. Publish the update.
8. Subscribe/download and perform an ONI smoke test.

Do not upload the GitHub ZIP directly unless the uploader explicitly asks for an archive; it normally expects the extracted mod folder.
