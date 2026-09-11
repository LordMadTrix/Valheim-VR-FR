# ᚢ ᚨ ᛚ ᚺ ᛖ ᛁ ᛗ   ᚡ ᚱ • Valheim VR (Édition Française)

<div align="center">

<img src="https://raw.githubusercontent.com/LordMadTrix/Valheim-VR-FR/main/docs/banner.png" alt="Bannière Valheim VR" width="100%" />

<br/>

[![Dernière Release](https://img.shields.io/badge/Release-v0.9.26--FR-gold.svg?style=for-the-badge&logo=github)](https://github.com/LordMadTrix/Valheim-VR-FR/releases)
[![Compatibilité Valheim](https://img.shields.io/badge/Valheim-Unity%206%20%7C%20Ashlands%20%7C%20Bog%20Witch-blue.svg?style=for-the-badge&logo=steam)](https://store.steampowered.com/app/892970/Valheim/)
[![Langue](https://img.shields.io/badge/Langue-100%25%20Fran%C3%A7ais-green.svg?style=for-the-badge)](#-7-traduction-int%C3%A9grale-en-fran%C3%A7ais)
[![OpenVR / SteamVR](https://img.shields.io/badge/VR-SteamVR%20%7C%20OpenVR-orange.svg?style=for-the-badge&logo=steamvr)](https://store.steampowered.com/app/250820/SteamVR/)
[![Licence](https://img.shields.io/badge/Licence-GPL--3.0-purple.svg?style=for-the-badge)](LICENSE)

### ⚔️ Plongez dans le dixième monde nordique en totale immersion à 360° en Réalité Virtuelle native ⚔️
*Version enrichie et francisée avec menus intégrés, installateur graphique dédié, profils matériels, synchronisation 90Hz, purge VRAM, consommation physique, ramassage à distance style Alyx, torche d'épaule, girouette de drakkar, haptique par matériau, lissage miroir PC et mode assis.*

[📦 Télécharger l'Installateur (Setup .exe)](https://github.com/LordMadTrix/Valheim-VR-FR/releases/latest) • [🎮 Commandes en VR](#-commandes-et-gestes-en-jeu) • [🛠️ Compilation](#-guide-de-compilation)

</div>

---

## 🌟 Nouveautés Majeures de l'Édition Française (v0.9.26)

<table>
<tr>
<td width="50%">

### 🎮 1. Intégration Native & Tout Configurable dans les Menus
* **Bouton Direct en Jeu** : Le menu principal et le menu pause (Échap) affichent un bouton clair et élégant **`PARAMÈTRES VR`**.
* **Accès dans les Paramètres Vanilla** : Bouton d'accès rapide **`🥽 Options VR`** directement dans la boîte de dialogue officielle des **Paramètres** de Valheim !
* **Menu Épuré** : Suppression définitive des boutons de debug encombrants (*Screenshot*, *Auto-pickup*).
* **Toutes les Nouvelles Options Réglables en Temps Réel** : Modifiez la portée de saisie, la luminosité de la torche, l'intensité des vibrations ou la fluidité spectateur directement en jeu sans redémarrer !

</td>
<td width="50%">

### 🥽 2. Nouvelles Mécaniques Immersives VR
* **Ramassage Gestuel à Distance ("Gravity Grab")** : Pointez un objet au sol et tirez-le vers votre main d'un coup de poignet ou d'une pression sur la gâchette Grip (style Half-Life: Alyx).
* **Torche Mains Libres à l'Épaule** : Éclairez les cryptes et donjons tout en maniant pioche ou armes à deux mains.
* **Girouette & Guide de Vent sur les Bateaux** : Afficheur physique dynamique de vent sur les drakkars (Rouge = face, Orange = travers, Vert = vent arrière parfait).
* **Vibrations Haptiques par Matériau** : Retour haptique distinct selon que vous frappez du bois, de la roche, de la chair ou parez un coup au bouclier.
* **Lissage Caméra Miroir PC** : Supprime les saccades pour les spectateurs sur Discord/Twitch.

</td>
</tr>
<tr>
<td width="50%">

### ⚡ 3. Optimisations Extrêmes & Fluidité VR
* **Synchronisation Physique Ajustable** : Alignement dynamique de la boucle physique (72, 80, 90, 120, 144 Hz) sur la fréquence native de votre casque VR.
* **Culling Intelligent d'Ombres & Distance Réglable** : Gain de **+15 à 25 FPS** dans les forêts denses et grands villages vikings grâce aux cascades stéréo VR.
* **Purge VRAM Automatique aux Portails** : Déchargement instantané de la mémoire lors des téléportations sous écran noir.
* **Textures Ultra Nettes** : Filtrage anisotrope forcé 16x pour un piqué d'image incomparable.

</td>
<td width="50%">

### 🍻 4. Installateur Graphique Dédié (`ValheimVR-Setup.exe`)
* **Design Viking & Nordique** : Interface soignée reprenant les teintes sombres ardoise (`#0D1015`) et dorées de Valheim.
* **Sélecteur de Profils Matériels** : Choisissez en 1 clic entre **Éco / Quest 2**, **Équilibré** ou **Ultra / Mythique**.
* **Détection Automatique Multi-Disques** : Scanne instantanément vos répertoires Steam.
* **100% Réversible** : Bouton *Restaurer Vanilla* pour repasser en version écran plat classique à tout moment.

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
4. Choisissez votre profil matériel (**Éco**, **Équilibré** ou **Ultra**) et vos options (Vignette anti-cinétose, Lissage de visée, Raccourci Bureau).
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
| **Ramassage à distance (Alyx)** | Pointez un objet/ressource au sol et pressez la gâchette Grip (Saisie) |
| **Torche mains libres** | Conservez une torche dans l'inventaire pour un éclairage automatique d'épaule |
| **Girouette de bateau** | Regardez devant le gouvernail : flèche dynamique verte/orange/rouge selon le vent |
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
