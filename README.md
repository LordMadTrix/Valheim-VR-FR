# ᚢ ᚨ ᛚ ᚺ ᛖ ᛁ ᛗ   ᚡ ᚱ • Valheim VR (Édition Française)

<div align="center">

<img src="https://raw.githubusercontent.com/LordMadTrix/Valheim-VR-FR/master/docs/banner.png" alt="Bannière Valheim VR" width="100%" />

[![Release](https://img.shields.io/badge/Release-v0.9.22--FR-gold.svg?style=for-the-badge&logo=github)](https://github.com/LordMadTrix/Valheim-VR-FR/releases)
[![Valheim Compatibility](https://img.shields.io/badge/Valheim-Unity%206%20%7C%20Ashlands%20%7C%20Bog%20Witch-blue.svg?style=for-the-badge&logo=steam)](https://store.steampowered.com/app/892970/Valheim/)
[![Langue](https://img.shields.io/badge/Langue-100%25%20Fran%C3%A7ais-green.svg?style=for-the-badge)](#-traduction-int%C3%A9grale-en-fran%C3%A7ais)
[![OpenVR / SteamVR](https://img.shields.io/badge/VR-SteamVR%20%7C%20OpenVR-orange.svg?style=for-the-badge&logo=steamvr)](https://store.steampowered.com/app/250820/SteamVR/)
[![License](https://img.shields.io/badge/Licence-GPL--3.0-purple.svg?style=for-the-badge)](LICENSE)

**Plongez au cœur du dixième monde nordique en immersion totale à 360° grâce à la Réalité Virtuelle native.**  
*Édition bonifiée avec installateur exécutable GUI aux couleurs de Valheim, consommation physique, confort anti-cinétose, visée stabilisée, mode assis et traduction intégrale en français.*

</div>

---

## ⚔️ Points Forts de l'Édition Française

### 🥽 1. Installateur Graphique Dédié (`ValheimVR-Setup.exe`)
- **Design Viking & Nordique** : Interface soignée reprenant les teintes sombres et dorées de Valheim avec icône applicative sur mesure.
- **Détection Automatique** : Localise instantanément votre installation Steam de Valheim (support multi-disques et bibliothèques personnalisées).
- **Déploiement en 1 Clic** : Installe BepInEx, les bibliothèques OpenVR/XRSDK, les profils SteamVR et le mod compilé.
- **100% Réversible** : Bouton de désinstallation/restauration en version écran plat d'origine (Vanilla) sans altérer vos sauvegardes ni personnages.

### 🍻 2. Immersion Gestuelle : Consommation Physique (Manger & Boire)
- Portez la nourriture, vos potions ou votre chope d'hydromel directement à votre casque pour les consommer physiquement !
- Détection de proximité naturelle, vibration haptique progressive et compatibilité bHaptics (vestes et accessoires haptiques).

### 🛡️ 3. Vignette Dynamique de Confort (Anti-Cinétose)
- Système réduisant la cinétose et les étourdissements.
- Obscurcissement progressif et fluide de la vision périphérique lors des mouvements à forte accélération :
  - **Rotations rapides** de la caméra et demi-tours.
  - **Sprints effrénés** à travers les forêts noires et les plaines.
  - **Navigation en drakkar** au cœur des tempêtes océaniques agitées.

### 🏹 4. Lissage & Stabilisation de Visée à l'Arc
- Réduction active des micro-tremblements musculaires et du jitter des manettes VR.
- Interpolation sphérique dynamique (Slerp) lorsque la corde de l'arc ou de l'arbalète est tendue pour des tirs chirurgicaux à longue distance.

### 🛋️ 5. Mode Assis & Recalibration Instantanée (L3 + R3)
- **Mode Assis ("Seated Mode")** : compensation de hauteur réglable (+0.5m) pour jouer détendu dans votre canapé ou chaise de bureau tout en gardant une taille de guerrier debout normale en jeu.
- **Recalibration Rapide** : un appui simultané de 1 seconde sur les deux joysticks (`L3 + R3`) recentre instantanément votre hauteur et orientation sans passer par les menus.

### 🎯 6. Optimiseur Graphique & Netteté VR
- **Filtrage anisotrope 16x forcé** pour des textures nettes et une lisibilité parfaite des inscriptions et détails d'armures.
- **Atténuation de l'éblouissement solaire** : adoucissement du bloom excessif pour protéger vos yeux dans les lentilles VR.

### 🇫🇷 7. Traduction Intégrale en Français
- **100% de l'interface du mod en français** : menus de configuration VR, onglets, intitulés, infobulles d'aide détaillées et options graphiques.

---

## 🚀 Installation & Démarrage

### Option A : Via l'Installateur Graphique (Recommandé)
1. Téléchargez **`ValheimVR-Setup.exe`** depuis la section [Releases](https://github.com/LordMadTrix/Valheim-VR-FR/releases).
2. Lancez l'exécutable.
3. Vérifiez le répertoire détecté de votre jeu Valheim (ou cliquez sur *Parcourir...*).
4. Cliquez sur **⚔️ INSTALLER VALHEIM VR (FR)**.
5. Branchez votre casque, démarrez **SteamVR**, puis cliquez sur **🥽 LANCER EN VR** ou utilisez le raccourci créé sur votre Bureau !

### Option B : Via les Scripts Automatisés
- Si vous préférez la ligne de commande :
  - Lancez simplement `Installer-ValheimVR-FR.cmd`.
  - Pour désinstaller : lancez `Desinstaller-ValheimVR-FR.cmd`.

---

## 🥽 Casques & Matériel Compatibles

Compatible avec l'ensemble des casques PCVR et autonomes connectés via SteamVR :
- **Meta Quest 2, 3, 3S & Pro** (via Quest Link filaire, AirLink, Virtual Desktop ou Steam Link)
- **Valve Index** (avec prise en charge du finger tracking haptique)
- **HTC Vive, Vive Pro & Cosmos**
- **Pico 4 & Neo 3**
- **Casques Windows Mixed Reality (WMR)** & **Bigscreen Beyond**

---

## 🎮 Commandes et Gestes en VR

| Action | Geste / Contrôleur |
| :--- | :--- |
| **Manger / Boire** | Approchez l'aliment ou la chope directement de votre casque |
| **Recalibration instantanée** | Maintenez les deux joysticks enfoncés (`L3 + R3`) pendant 1 seconde |
| **Attaque au corps à corps** | Donnez un coup physique avec votre arme en main |
| **Blocage / Parade** | Levez votre bouclier ou votre arme face à l'ennemi au bon timing |
| **Tir à l'arc** | Tirez la corde vers votre joue avec la main arrière et lâchez la gâchette |
| **Bûcheronnage / Minage** | Frappez physiquement les arbres ou les filons avec votre hache / pioche |
| **Menu d'inventaire & artisanat** | Flottant en réalité virtuelle, interactif avec le pointeur laser ou boutons |

---

## 🛠️ Compilation Depuis les Sources

Pour compiler le projet vous-même :

### Prérequis
- Windows 10/11 x64
- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- Jeu Valheim installé via Steam

### Étapes
```powershell
# 1. Cloner le dépôt
git clone https://github.com/LordMadTrix/Valheim-VR-FR.git
cd Valheim-VR-FR

# 2. Compiler le mod en C# Release
dotnet build ValheimVRMod.sln -c Release

# 3. Compiler l'installateur autonome
dotnet publish ValheimVRInstaller\ValheimVRInstaller.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o SetupOutput
```

---

## 📜 Licence & Crédits

- **Projet Original VHVR** : Développé initialement par [Brandon Mousseau (brandonmousseau/vhvr-mod)](https://github.com/brandonmousseau/vhvr-mod).
- **Framework BepInEx** : [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) développé par la communauté modding Valheim.
- **Édition Française & Setup GUI** : Réalisé par **LordMadTrix**.
- **Licence** : Ce projet est distribué sous licence **GPL-3.0**. Consultez le fichier [LICENSE](LICENSE) pour plus de détails.
