param(
	[Parameter(Mandatory = $true)]
	[string]$SteamUsername,
	[string]$SteamCmdPath = "steamcmd"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$content = Join-Path $env:TEMP "CustomizableEnergy-WorkshopContent"
$staging = Join-Path $env:TEMP "CustomizableEnergy-WorkshopStaging"
$package = Join-Path $content "CustomizableEnergy.zip"
$vdfPath = Join-Path $PSScriptRoot "create-workshop-item.vdf"

Remove-Item -LiteralPath $content,$staging -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $content,$staging -Force | Out-Null

dotnet restore (Join-Path $root "CustomizableEnergy.csproj")
dotnet build (Join-Path $root "CustomizableEnergy.csproj") --configuration Release --no-restore

Copy-Item (Join-Path $root "bin\Release\netstandard2.1\CustomizableEnergy.dll") $staging
Copy-Item (Join-Path $root "mod.yaml"), (Join-Path $root "mod_info.yaml"), (Join-Path $root "README.md") $staging
Copy-Item (Join-Path $root "third_party\ONIMods\PLibCore\bin\Release\netstandard2.1\PLibCore.dll") $staging
Copy-Item (Join-Path $root "third_party\ONIMods\PLibUI\bin\Release\netstandard2.1\PLibUI.dll") $staging
Copy-Item (Join-Path $root "third_party\ONIMods\PLibOptions\bin\Release\netstandard2.1\PLibOptions.dll") $staging
Compress-Archive -Path "$staging\*" -DestinationPath $package -Force

$vdf = Get-Content (Join-Path $PSScriptRoot "workshop.vdf.template") -Raw
$vdf = $vdf.Replace('${CONTENT_PATH}', $package.Replace('\', '/')).Replace('${PREVIEW_PATH}', (Join-Path $PSScriptRoot "preview.png").Replace('\', '/')).Replace('${WORKSHOP_ITEM_ID}', '0')
Set-Content -LiteralPath $vdfPath -Value $vdf -NoNewline

& $SteamCmdPath +login $SteamUsername +workshop_build_item $vdfPath +quit
if ($LASTEXITCODE -ne 0) {
	throw "SteamCMD failed with exit code $LASTEXITCODE."
}

$itemId = [regex]::Match((Get-Content $vdfPath -Raw), '"publishedfileid"\s+"(\d+)"').Groups[1].Value
if ([string]::IsNullOrWhiteSpace($itemId) -or $itemId -eq '0') {
	throw "SteamCMD did not return a Workshop item ID."
}

Remove-Item -LiteralPath $vdfPath -Force
"Workshop item created: $itemId"
"Set the GitHub repository variable STEAM_WORKSHOP_ITEM_ID to $itemId before enabling CI publishing."
