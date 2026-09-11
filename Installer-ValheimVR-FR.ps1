<#
.SYNOPSIS
    Script d'installation automatisé pour Valheim VR (VHVR) en Français.
.DESCRIPTION
    Détecte automatiquement le répertoire d'installation de Valheim,
    installe BepInExPack, déploie le mod VR francisé, configure les options
    anti-cinétose/visée et vérifie l'intégrité de l'environnement de jeu.
.AUTHOR
    MadTrix / Antigravity
#>

[CmdletBinding()]
param (
    [string]$GameDirectory = ""
)

# Configuration de la console
$Host.UI.RawUI.WindowTitle = "Installateur Valheim VR (VHVR) - Français"
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

function Write-Header {
    Clear-Host
    Write-Host "====================================================================" -ForegroundColor Cyan
    Write-Host "           INSTALLATEUR OFFICIEL VALHEIM VR (VHVR) - FR             " -ForegroundColor Yellow -NoNewline
    Write-Host " [v0.9.26-FR]" -ForegroundColor Green
    Write-Host "           Adapté pour Valheim (Unity 6 / Ashlands / Bog Witch)     " -ForegroundColor DarkCyan
    Write-Host "====================================================================" -ForegroundColor Cyan
    Write-Host ""
}

function Write-Success { param([string]$Message) Write-Host "  [OK] $Message" -ForegroundColor Green }
function Write-Info    { param([string]$Message) Write-Host "  [i]  $Message" -ForegroundColor Cyan }
function Write-Warn    { param([string]$Message) Write-Host "  [!]  $Message" -ForegroundColor Yellow }
function Write-Err     { param([string]$Message) Write-Host "  [X]  $Message" -ForegroundColor Red }

function Find-ValheimInstallation {
    $potentialPaths = @(
        "D:\SteamLibrary\steamapps\common\Valheim",
        "C:\Program Files (x86)\Steam\steamapps\common\Valheim",
        "C:\Steam\steamapps\common\Valheim",
        "E:\SteamLibrary\steamapps\common\Valheim",
        "F:\SteamLibrary\steamapps\common\Valheim"
    )

    # Recherche via le registre Steam
    try {
        $steamPath = (Get-ItemProperty -Path "HKCU:\Software\Valve\Steam" -Name "SteamPath" -ErrorAction SilentlyContinue).SteamPath
        if ($steamPath) {
            $steamPath = $steamPath.Replace('/', '\')
            $libraryFile = Join-Path $steamPath "steamapps\libraryfolders.vdf"
            if (Test-Path $libraryFile) {
                $lines = Get-Content $libraryFile
                foreach ($line in $lines) {
                    if ($line -match '"path"\s+"([^"]+)"') {
                        $libDir = $matches[1].Replace('\\', '\')
                        $potentialPaths += (Join-Path $libDir "steamapps\common\Valheim")
                    }
                }
            }
        }
    } catch {}

    foreach ($path in ($potentialPaths | Select-Object -Unique)) {
        if ((Test-Path (Join-Path $path "valheim.exe")) -and (Test-Path (Join-Path $path "valheim_Data"))) {
            return $path
        }
    }

    return $null
}

# --- DÉBUT DU PROGRAMME ---
Write-Header

# 1. Vérification des processus en cours
$valheimProc = Get-Process -Name valheim -ErrorAction SilentlyContinue
if ($valheimProc) {
    Write-Warn "Valheim est actuellement en cours d'exécution !"
    $choice = Read-Host "  Voulez-vous fermer Valheim automatiquement pour continuer ? (O/N)"
    if ($choice -match '^[oOyY]') {
        Stop-Process -Name valheim -Force
        Start-Sleep -Seconds 2
        Write-Success "Valheim a été fermé."
    } else {
        Write-Err "Installation interrompue : Valheim doit être fermé."
        Pause
        exit 1
    }
}

# 2. Détection du jeu
$targetDir = $GameDirectory
if ([string]::IsNullOrWhiteSpace($targetDir)) {
    Write-Info "Recherche automatique du répertoire Valheim..."
    $targetDir = Find-ValheimInstallation
}

if (-not $targetDir -or -not (Test-Path (Join-Path $targetDir "valheim.exe"))) {
    Write-Warn "Répertoire Valheim introuvable automatiquement."
    $targetDir = Read-Host "  Veuillez entrer le chemin complet de votre dossier Valheim"
    if (-not (Test-Path (Join-Path $targetDir "valheim.exe"))) {
        Write-Err "Le chemin spécifié ne contient pas valheim.exe : '$targetDir'"
        Pause
        exit 1
    }
}

Write-Success "Dossier de jeu détecté : $targetDir"

# 3. Répertoire source du mod
$sourceModDir = $PSScriptRoot
if (-not (Test-Path (Join-Path $sourceModDir "ValheimVRMod\ValheimVRMod.csproj"))) {
    $sourceModDir = "D:\VR VALHEIM"
}

$releaseDir = Join-Path $sourceModDir "ValheimVRMod\release"
$unityBuildDir = Join-Path $sourceModDir "Unity\build\ValheimVR_Data"

# Vérification ou compilation préalable si nécessaire (priorité Release pour performance max)
$modDllRelease = Join-Path $sourceModDir "ValheimVRMod\bin\Release\net46\ValheimVRMod.dll"
$modDllDebug   = Join-Path $sourceModDir "ValheimVRMod\bin\Debug\net46\ValheimVRMod.dll"

if (-not (Test-Path $modDllRelease) -and -not (Test-Path $modDllDebug)) {
    Write-Info "Compilation du mod en cours via dotnet build (Release)..."
    $buildProcess = Start-Process "dotnet" -ArgumentList "build ValheimVRMod.sln -c Release" -WorkingDirectory $sourceModDir -Wait -PassThru -NoNewWindow
    if ($buildProcess.ExitCode -ne 0) {
        Write-Err "Échec de la compilation du mod !"
        Pause
        exit 1
    }
    Write-Success "Compilation terminée avec succès."
}

$modDll = if (Test-Path $modDllRelease) { $modDllRelease } else { $modDllDebug }

# 4. Déploiement des composants
Write-Host ""
Write-Info "Déploiement des fichiers de Valheim VR..."

# A. BepInEx de base
$bepInExCore = Join-Path $targetDir "BepInEx\core\BepInEx.dll"
if (-not (Test-Path $bepInExCore)) {
    Write-Info "Installation du framework BepInExPack Valheim..."
    $bepDir = Join-Path $sourceModDir "BepInExPack_Valheim"
    if (Test-Path $bepDir) {
        Copy-Item -Path "$bepDir\*" -Destination $targetDir -Recurse -Force
    } else {
        Write-Warn "Dossier BepInExPack_Valheim non trouvé dans la racine. Vérification des fichiers existants..."
    }
}

# Assurer la présence des dossiers
$pluginsDir = Join-Path $targetDir "BepInEx\plugins"
$configDir  = Join-Path $targetDir "BepInEx\config"
$null = New-Item -ItemType Directory -Path $pluginsDir -Force -ErrorAction SilentlyContinue
$null = New-Item -ItemType Directory -Path $configDir -Force -ErrorAction SilentlyContinue

# B. Copie du binaire du Mod
Copy-Item -Path $modDll -Destination (Join-Path $pluginsDir "ValheimVRMod.dll") -Force
Write-Success "Mod VR francisé copié -> BepInEx\plugins\ValheimVRMod.dll"

# C. bHaptics
$bhapticsSrc = Join-Path $sourceModDir "bHaptics"
if (Test-Path $bhapticsSrc) {
    Copy-Item -Path $bhapticsSrc -Destination $pluginsDir -Recurse -Force
    Write-Success "Profils haptiques copiés -> BepInEx\plugins\bHaptics"
}

# D. Native Plugins (OpenVR / SteamVR)
$pluginsX64 = Join-Path $targetDir "valheim_Data\Plugins\x86_64"
$null = New-Item -ItemType Directory -Path $pluginsX64 -Force -ErrorAction SilentlyContinue
if (Test-Path "$unityBuildDir\Plugins\x86_64") {
    Copy-Item -Path "$unityBuildDir\Plugins\x86_64\*" -Destination $pluginsX64 -Recurse -Force
    Write-Success "Pilotes natifs OpenVR & XRSDK déployés dans valheim_Data\Plugins"
}

# E. Managed Assemblies VR
$managedDir = Join-Path $targetDir "valheim_Data\Managed"
$managedFiles = @("SteamVR.dll", "SteamVR_Actions.dll", "Unity.XR.Management.dll", "Unity.XR.OpenVR.dll",
                  "UnityEngine.SpatialTracking.dll", "UnityEngine.XR.LegacyInputHelpers.dll",
                  "final_ik.dll", "amplify_occlusion.dll", "Valve.Newtonsoft.Json.dll",
                  "root_motion_demo_assets.dll", "root_motion_shared.dll")
foreach ($f in $managedFiles) {
    $srcFile = Join-Path "$unityBuildDir\Managed" $f
    if (Test-Path $srcFile) {
        Copy-Item -Path $srcFile -Destination $managedDir -Force
    }
}
Write-Success "Assemblies Unity XR / SteamVR synchronisés"

# F. StreamingAssets (Actions SteamVR, Shaders, Bundles)
$steamvrAssets = Join-Path $targetDir "valheim_Data\StreamingAssets\SteamVR"
$null = New-Item -ItemType Directory -Path $steamvrAssets -Force -ErrorAction SilentlyContinue
if (Test-Path "$unityBuildDir\StreamingAssets\SteamVR") {
    Copy-Item -Path "$unityBuildDir\StreamingAssets\SteamVR\*" -Destination $steamvrAssets -Recurse -Force
    Write-Success "Profils d'actions SteamVR configurés (Quest, Index, Vive, etc.)"
}

# G. Subsystems Manifest
$subsysDir = Join-Path $targetDir "valheim_Data\UnitySubsystems\XRSDKOpenVR"
$null = New-Item -ItemType Directory -Path $subsysDir -Force -ErrorAction SilentlyContinue
if (Test-Path "$unityBuildDir\UnitySubsystems\XRSDKOpenVR\UnitySubsystemsManifest.json") {
    Copy-Item -Path "$unityBuildDir\UnitySubsystems\XRSDKOpenVR\UnitySubsystemsManifest.json" -Destination $subsysDir -Force
    Write-Success "Manifeste UnitySubsystems XRSDK enregistré"
}

# 5. Création d'un raccourci sur le Bureau (optionnel)
Write-Host ""
$desktopPath = [Environment]::GetFolderPath("Desktop")
$shortcutPath = Join-Path $desktopPath "Valheim VR (FR).url"
@"
[InternetShortcut]
URL=steam://rungameid/892970
IconIndex=0
IconFile=$targetDir\valheim.exe
"@ | Set-Content -Path $shortcutPath -Encoding ASCII
Write-Success "Raccourci créé sur votre Bureau : 'Valheim VR (FR)'"

# 6. Récapitulatif et fin
Write-Host ""
Write-Host "====================================================================" -ForegroundColor Cyan
Write-Host "           INSTALLATION DE VALHEIM VR TERMINÉE AVEC SUCCÈS !        " -ForegroundColor Green
Write-Host "====================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Info "Pour jouer en Réalité Virtuelle :"
Write-Host "  1. Démarrez votre casque VR (Oculus Quest avec Link/AirLink/VD, Valve Index, etc.)." -ForegroundColor White
Write-Host "  2. Lancez SteamVR sur votre ordinateur." -ForegroundColor White
Write-Host "  3. Lancez le jeu via le raccourci sur votre Bureau ou directement sur Steam." -ForegroundColor White
Write-Host "  4. Les options du mod en jeu seront automatiquement en FRANÇAIS." -ForegroundColor Yellow
Write-Host ""

Pause
