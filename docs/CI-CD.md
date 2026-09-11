# CI/CD and Steam Workshop publishing

## GitHub-hosted runners

The workflows run on GitHub-hosted `windows-latest` runners. They use SteamCMD to download Oxygen Not Included into the temporary runner workspace, then compile the mod against the downloaded Managed assemblies. No self-hosted runner, local ONI path, or runner labels are required.

Because Steam credentials are required to download ONI, the build and release workflows run on pushes to `main` and manual dispatch only. Pull requests do not run the credentialed build.

Configure these repository values and secrets:

| Type | Name | Purpose |
|---|---|---|
| Variable | `STEAM_WORKSHOP_ITEM_ID` | Steam Workshop item ID: `3799490472`. |
| Secret | `STEAM_USERNAME` | Steam account name used by SteamCMD. |
| Secret | `STEAM_CONFIG_VDF` | Base64-encoded authenticated SteamCMD `config.vdf`. |

## Build workflow

`.github/workflows/build.yml` runs for pushes to `main` and manual dispatch. It downloads ONI, restores/builds the mod, and uploads a ZIP artifact containing the mod DLL, metadata, README, and required PLib DLLs.

## Semantic releases and Workshop publishing

`.github/workflows/publish.yml` runs on pushes to `main`. Semantic-release reads Conventional Commit messages, chooses the next version, updates `mod_info.yaml`, creates the GitHub release, and then updates Workshop item `3799490472` in the same workflow.

- `feat`: minor release
- `fix`, `perf`, `refactor`, or `ci`: patch release
- A breaking-change footer: major release

The workflow uses the default GitHub Actions token because semantic-release and Steam publishing occur in one job. No separate GitHub PAT is required.

If no commit since the prior release requires a version bump, semantic-release finishes without publishing to Steam.

Do not commit Steam credentials or the rendered `Steam/workshop.vdf` file.
