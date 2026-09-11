# CI/CD and Steam Workshop publishing

## GitHub-hosted runners

The workflows run on GitHub-hosted `windows-latest` runners. They use SteamCMD to download Oxygen Not Included into the temporary runner workspace, then compile the mod against the downloaded Managed assemblies. No self-hosted runner, local ONI path, or runner labels are required.

Because Steam credentials are required to download ONI, the build workflow runs on pushes to `main` and manual dispatch only. Pull requests do not run the credentialed build.

Configure these repository values and secrets:

| Type | Name | Purpose |
|---|---|---|
| Variable | `STEAM_WORKSHOP_ITEM_ID` | Steam Workshop item ID: `3799490472`. |
| Secret | `STEAM_USERNAME` | Steam account name used by SteamCMD. |
| Secret | `STEAM_CONFIG_VDF` | Base64-encoded authenticated SteamCMD `config.vdf`. |

## Build workflow

`.github/workflows/build.yml` runs for pushes to `main` and manual dispatch. It downloads ONI, restores/builds the mod, and uploads a ZIP artifact containing the mod DLL, metadata, README, and required PLib DLLs.

## Publishing a release

The publish workflow runs on a pushed `v*` tag or manually with a version input. It downloads ONI, builds the mod, creates Steam Workshop content, updates Workshop item `3799490472`, and creates a GitHub release.

```powershell
git tag v0.1.0
git push origin v0.1.0
```

Do not commit Steam credentials or the rendered `Steam/workshop.vdf` file.
