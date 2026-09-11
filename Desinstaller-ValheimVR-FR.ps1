<#
.SYNOPSIS
    Script de désinstallation / restauration pour Valheim VR (VHVR).
.DESCRIPTION
    Supprime proprement les DLLs du mod et les composants VR injectés
    pour restaurer Valheim dans son état standard (écran plat).
#>

[CmdletBinding()]
param (
    [string]$GameDirectory = ""
)

$Host.UI.RawUI.WindowTitle = "Désinstallateur Valheim VR (VHVR)"
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

Clear-Host
Write-Host "====================================================================" -ForegroundColor Red
Write-Host "         DÉSINSTALLATION / RESTAURATION DE VALHEIM (VANILLA)        " -ForegroundColor Yellow
Write-Host "====================================================================" -ForegroundColor Red
Write-Host ""

$targetDir = $GameDirectory
if ([string]::IsNullOrWhiteSpace($targetDir)) {
    $potentialPaths = @(
        "D:\SteamLibrary\steamapps\common\Valheim",
        "C:\Program Files (x86)\Steam\steamapps\common\Valheim",
        "C:\Steam\steamapps\common\Valheim"
    )
    foreach ($p in $potentialPaths) {
        if (Test-Path (Join-Path $p "valheim.exe")) {
            $targetDir = $p
            break
        }
    }
}

if (-not $targetDir -or -not (Test-Path (Join-Path $targetDir "valheim.exe"))) {
    $targetDir = Read-Host "  Entrez le chemin du dossier Valheim à restaurer"
}

if (-not (Test-Path (Join-Path $targetDir "valheim.exe"))) {
    Write-Host "  [X] Dossier Valheim invalide." -ForegroundColor Red
    Pause
    exit 1
}

Write-Host "  Dossier cible : $targetDir" -ForegroundColor Cyan
$confirm = Read-Host "  Voulez-vous désactiver le mod VR et restaurer Valheim en mode standard ? (O/N)"
if ($confirm -notmatch '^[oOyY]') {
    Write-Host "  Désinstallation annulée." -ForegroundColor Yellow
    Pause
    exit 0
}

# 1. Suppression du plugin VR
$modDll = Join-Path $targetDir "BepInEx\plugins\ValheimVRMod.dll"
if (Test-Path $modDll) {
    Remove-Item -Path $modDll -Force
    Write-Host "  [OK] ValheimVRMod.dll supprimé de BepInEx\plugins" -ForegroundColor Green
}

# 2. Suppression de l'injection VR native (laisser le jeu charger en écran plat)
$vrPlugin = Join-Path $targetDir "valheim_Data\Plugins\x86_64\XRSDKOpenVR.dll"
if (Test-Path $vrPlugin) {
    Remove-Item -Path $vrPlugin -Force
    Write-Host "  [OK] Module OpenVR supprimé de valheim_Data\Plugins" -ForegroundColor Green
}

$subsys = Join-Path $targetDir "valheim_Data\UnitySubsystems\XRSDKOpenVR"
if (Test-Path $subsys) {
    Remove-Item -Path $subsys -Recurse -Force
    Write-Host "  [OK] Sous-système XRSDKOpenVR supprimé" -ForegroundColor Green
}

# 3. Raccourci bureau
$desktopPath = [Environment]::GetFolderPath("Desktop")
$shortcuts = @("Valheim VR (FR).url", "Valheim VR (Français).url")
foreach ($s in $shortcuts) {
    $scPath = Join-Path $desktopPath $s
    if (Test-Path $scPath) {
        Remove-Item -Path $scPath -Force
        Write-Host "  [OK] Raccourci '$s' supprimé du bureau" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "====================================================================" -ForegroundColor Green
Write-Host "       RESTAURATION TERMINÉE : VALHEIM EST DE NOUVEAU EN MODE PLAT  " -ForegroundColor Green
Write-Host "====================================================================" -ForegroundColor Green
Write-Host ""
Pause
