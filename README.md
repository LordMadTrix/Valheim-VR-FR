# ᚢ ᚨ ᛚ ᚺ ᛖ ᛁ ᛗ   ᚡ ᚱ • Valheim VR (Édition Française)

<div align="center">

<img src="https://raw.githubusercontent.com/LordMadTrix/Valheim-VR-FR/main/docs/banner.png" alt="Bannière Valheim VR" width="100%" />

<br/>

[![Dernière Release](https://img.shields.io/badge/Release-v0.9.22--FR-gold.svg?style=for-the-badge&logo=github)](https://github.com/LordMadTrix/Valheim-VR-FR/releases)
[![Compatibilité Valheim](https://img.shields.io/badge/Valheim-Unity%206%20%7C%20Ashlands%20%7C%20Bog%20Witch-blue.svg?style=for-the-badge&logo=steam)](https://store.steampowered.com/app/892970/Valheim/)
[![Langue](https://img.shields.io/badge/Langue-100%25%20Fran%C3%A7ais-green.svg?style=for-the-badge)](#-7-traduction-int%C3%A9grale-en-fran%C3%A7ais)
[![OpenVR / SteamVR](https://img.shields.io/badge/VR-SteamVR%20%7C%20OpenVR-orange.svg?style=for-the-badge&logo=steamvr)](https://store.steampowered.com/app/250820/SteamVR/)
[![Licence](https://img.shields.io/badge/Licence-GPL--3.0-purple.svg?style=for-the-badge)](LICENSE)

### ⚔️ Plongez dans le dixième monde nordique en totale immersion à 360° en Réalité Virtuelle native ⚔️
*Version enrichie et francisée avec installateur graphique dédié, consommation physique, confort anti-cinétose, visée stabilisée et mode assis.*

[📦 Télécharger l'Installateur (Setup .exe)](https://github.com/LordMadTrix/Valheim-VR-FR/releases/latest) • [🎮 Commandes en VR](#-commandes-et-gestes-en-jeu) • [🛠️ Compilation](#-guide-de-compilation)

</div>

---

## 🌟 Nouveautés Majeures de l'Édition Française (v0.9.22)

<table>
<tr>
<td width="50%">

### 🥽 1. Installateur Graphique Dédié (`ValheimVR-Setup.exe`)
* **Design Viking & Nordique** : Interface soignée reprenant les teintes sombres ardoise (`#0D1015`) et dorées de Valheim avec icône applicative exclusive.
* **Détection Automatique** : Scanne instantanément vos bibliothèques Steam multi-disques (`C:\`, `D:\`, `E:\`...).
* **Déploiement en 1 Clic** : Installe BepInExPack, les plugins natifs OpenVR 64-bit, les profils SteamVR et le mod compilé.
* **100% Réversible** : Un bouton *Restaurer Vanilla* permet de repasser en version écran plat classique à tout moment sans toucher à vos personnages ni sauvegardes.

</td>
<td width="50%">

### 🍻 2. Consommation Gestuelle Physique
* **Manger & Boire Naturellement** : Approchez physiquement votre main tenant de la viande, une potion ou une chope d'hydromel vers votre casque VR pour la consommer !
* **Feedback Sensoriel** : Vibration haptique progressive au rapprochement de la bouche et confirmation d'ingestion.
* **Support bHaptics** : Compatible avec les vestes haptiques pour ressentir la nourriture et les boissons.
* **Anti-Faux Positif** : Temporisation de sécurité ($0.32\text{ s}$) pour éviter toute utilisation involontaire au combat.

</td>
</tr>
<tr>
<td width="50%">

### 🛋️ 3. Mode Assis & Recalibration Instantanée
* **Mode Assis ("Seated Mode")** : Jouez détendu depuis votre canapé ou fauteuil de bureau tout en conservant une taille normale de viking debout dans le monde du jeu grâce à la compensation de hauteur virtuelle.
* **Recalibration Rapide (`L3 + R3`)** : Maintenez les deux joysticks enfoncés pendant 1 seconde pour recentrer instantanément le sol, votre orientation et votre hauteur sans jamais ouvrir de menu.

</td>
<td width="50%">

### 🎯 4. Optimiseur de Rendu & Clarté Visuelle
* **Textures Ultra Nettes** : Filtrage anisotrope forcé 16x pour un piqué d'image incomparable sur les textures de bois, pierre, herbe et inscriptions runiques.
* **Atténuation de l'Éblouissement Solaire** : Adoucissement automatique du bloom et des halos lumineux violents pour préserver le confort visuel dans les lentilles VR (Fresnel & Pancake).

</td>
</tr>
</table>

---

<div align="center">

<img src="https://raw.githubusercontent.com/LordMadTrix/Valheim-VR-FR/main/docs/gameplay.png" alt="Gameplay Valheim VR" width="100%" />

*Survie nordique en VR : immersion totale avec tir à l'arc réaliste, parade au bouclier et interface flottante.*

</div>

---

### 🛡️ 5. Vignette Dynamique de Confort (Anti-Cinétose)
Conçue spécifiquement pour les joueurs sensibles au mal des transports en réalité virtuelle :
* **Assombrissement progressif et subtil** de la vision périphérique lors des mouvements à forte accélération :
  * **Rotations de caméra** (Smooth Turn ou mouvements brusques de tête).
  * **Sprints et courses effrénées** à travers forêts et montagnes enneigées.
  * **Navigation en drakkar** balloté par les vagues et tempêtes en haute mer.
* Réglable et débrayable à tout moment depuis les options en jeu sous l'onglet *"Confort Anti-Cinétose"*.

### 🏹 6. Lissage & Stabilisation de Visée à l'Arc
* Élimine les micro-tremblements musculaires et le jitter naturel des manettes de détection de mouvement.
* Interpolation sphérique dynamique (**Slerp**) appliquée lorsque la corde de l'arc ou de l'arbalète est tendue pour réaliser des tirs chirurgicaux à longue distance.

### 🇫🇷 7. Traduction Intégrale en Français
* **100% de l'interface du mod VR traduite** :
  * Tous les onglets de configuration (*Général*, *Interface*, *HUD VR*, *Commandes & Déplacement*, *Graphismes*, *Contrôles Gestuels*, *Confort Anti-Cinétose*).
  * Toutes les options, valeurs (*Droitier*, *Gaucher*, *Classique*, *Désactivé*...) et boutons d'action.
  * L'intégralité des bulles d'aide détaillées affichées au survol dans le casque.

---

## 🚀 Guide d'Installation & Démarrage

### Méthode A : Via l'Installateur Graphique (Fortement recommandée)
1. Téléchargez **[`ValheimVR-Setup.exe`](https://github.com/LordMadTrix/Valheim-VR-FR/releases/latest)**.
2. Lancez l'exécutable (aux couleurs de Valheim).
3. Le chemin de votre jeu Valheim est détecté automatiquement (vous pouvez cliquer sur *Parcourir...* si nécessaire).
4. Cochez les options souhaitées (Vignette anti-cinétose, Lissage de visée, Raccourci Bureau).
5. Cliquez sur **⚔️ INSTALLER VALHEIM VR (FR)**.
6. Allumez votre casque, lancez **SteamVR**, puis cliquez sur **🥽 LANCER EN VR** !

> [!TIP]
> **Restauration Vanilla instantanée :** Vous souhaitez repasser sur écran plat pour une session rapide ? Ouvrez simplement l'installateur et cliquez sur **🔄 Restaurer Vanilla**. Vos sauvegardes, mondes et personnages restent 100% intacts !

### Méthode B : Via les Scripts Automatisés
Si vous préférez la ligne de commande :
* **Installation :** Double-cliquez sur `Installer-ValheimVR-FR.cmd`.
* **Désinstallation :** Double-cliquez sur `Desinstaller-ValheimVR-FR.cmd`.

---

## 🥽 Casques & Matériel Compatibles

Compatible avec tous les casques PCVR et autonomes connectés via SteamVR :
* **Meta Quest 2, Quest 3, Quest 3S & Quest Pro** (Quest Link par câble USB-C, AirLink, Virtual Desktop ou Steam Link).
* **Valve Index** (avec finger tracking haptique complet).
* **HTC Vive, Vive Pro, Vive Focus 3 & Cosmos**.
* **Pico 4, 4 Ultra & Neo 3** (via Pico Connect ou Virtual Desktop).
* **Casques Windows Mixed Reality (WMR)** & **Bigscreen Beyond**.

---

## 🎮 Commandes et Gestes en Jeu

| Action en Jeu | Geste / Contrôleur VR |
| :--- | :--- |
| **Manger / Boire** | Approchez physiquement la nourriture ou la chope de votre casque VR |
| **Recalibration rapide** | Maintenez les deux joysticks enfoncés (`L3 + R3`) pendant 1 seconde |
| **Attaque au corps à corps** | Donnez un coup physique dans la direction de l'ennemi avec votre arme |
| **Blocage / Parade** | Levez votre bouclier ou arme face au coup de l'ennemi au bon timing |
| **Tir à l'arc / arbalète** | Tendez la corde vers votre joue avec la main arrière et lâchez la gâchette |
| **Bûcheronnage / Minage** | Frappez physiquement les troncs d'arbres ou filons de minerai avec votre outil |
| **Menu d'inventaire & artisanat** | Panneau flottant dans l'espace VR, sélectionnable via le pointeur laser ou boutons |
| **Mode Assis** | Activable dans les réglages pour compenser la hauteur depuis un fauteuil |

---

## 🛠️ Guide de Compilation

Pour les développeurs souhaitant compiler le projet depuis les sources :

### Prérequis
* Windows 10/11 x64
* [.NET SDK 9.0](https://dotnet.microsoft.com/download)
* Valheim installé via Steam

### Commandes
```powershell
# 1. Cloner le dépôt
git clone https://github.com/LordMadTrix/Valheim-VR-FR.git
cd Valheim-VR-FR

# 2. Compiler la bibliothèque du mod en Release
dotnet build ValheimVRMod.sln -c Release

# 3. Compiler l'installateur autonome Single-File
dotnet publish ValheimVRInstaller\ValheimVRInstaller.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o SetupOutput
```

---

## 📜 Licence & Crédits

* **Mod VR Original** : Développé initialement par [Brandon Mousseau (brandonmousseau/vhvr-mod)](https://github.com/brandonmousseau/vhvr-mod).
* **Framework BepInEx** : [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) par la communauté modding Valheim.
* **Édition Française, Nouveautés & Setup GUI** : Réalisé par **LordMadTrix**.
* **Licence** : Ce projet est sous licence libre **GPL-3.0**. Consultez le fichier [LICENSE](LICENSE) pour plus d'informations.

---

## 👑 Signature & Auteur Officiel

<div align="center">

<a href="https://github.com/LordMadTrix">
  <img src="https://raw.githubusercontent.com/LordMadTrix/Valheim-VR-FR/main/docs/lordmadtrix_logo.png" alt="LordMadTrix Official Brand" width="180" />
</a>

### ⚡ Conçu & Forgé par **[LordMadTrix](https://github.com/LordMadTrix)** ⚡
*Architecte Systèmes • Immersion VR & Gaming • Optimisation OS & IA*

[![GitHub Profile](https://img.shields.io/badge/GitHub-LordMadTrix-181717?style=for-the-badge&logo=github)](https://github.com/LordMadTrix)
[![Édition Française](https://img.shields.io/badge/Édition-Française%20Officielle-gold?style=for-the-badge)](https://github.com/LordMadTrix/Valheim-VR-FR)

*« Forger l'excellence technologique au cœur du code et de l'immersion. »*

</div>
