using System;
using System.Collections.Generic;

namespace ValheimVRMod.Utilities
{
    /// <summary>
    /// Gestionnaire de localisation français pour les menus, options et textes VR.
    /// </summary>
    public static class VHVRLocalization
    {
        private static readonly Dictionary<string, string> Sections = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "General", "Général" },
            { "UI", "Interface (UI)" },
            { "VR Hud", "Affichage VR (HUD)" },
            { "Controls", "Commandes & Déplacement" },
            { "Graphics", "Graphismes & Rendu" },
            { "Motion Control", "Contrôles Gestuels" },
            { "Comfort", "Confort Anti-Cinétose" }
        };

        private static readonly Dictionary<string, string> Keys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Général
            { "RecenterOnStart", "Recentrer la vue au démarrage" },
            { "MirrorMode", "Mode miroir écran PC" },
            { "PlayerHeightAdjust", "Ajustement taille personnage" },
            { "ImmersiveShipCameraStanding", "Caméra bateau debout" },
            { "ImmersiveShipCameraSitting", "Caméra bateau assis" },
            { "ImmersiveDodgeRoll", "Rotation caméra pendant esquive" },
            { "AllowMovementWhenInMenu", "Autoriser mouvement dans menu" },
            { "ShowDebugColliders", "Afficher colliders de debug" },
            { "HipTrackerIndex", "Tracker de hanche (index)" },
            { "LeftFootTrackerIndex", "Tracker pied gauche (index)" },
            { "RightFootTrackerIndex", "Tracker pied droit (index)" },
            { "RoomscaleFadeToBlack", "Fondu au noir hors zone de jeu" },
            { "DisableRecenterPose", "Désactiver pose de recentrage" },

            // Confort
            { "EnableComfortVignette", "Activer vignette anti-cinétose" },
            { "ComfortVignetteIntensity", "Opacité max de la vignette" },
            { "ComfortVignetteOnTurning", "Vignette lors des rotations" },
            { "ComfortVignetteOnSailing", "Vignette en bateau sur les vagues" },
            { "ComfortVignetteOnSprint", "Vignette lors du sprint" },

            // Interface UI
            { "OverlayWidth", "Largeur du panneau interface" },
            { "OverlayCurvature", "Courbure de l'interface" },
            { "OverlayDistance", "Distance de l'interface" },
            { "OverlayVerticalPosition", "Hauteur de l'interface" },
            { "UiPanelSize", "Échelle du panneau UI" },
            { "UiPanelDistance", "Distance du panneau en jeu" },
            { "UiPanelVerticalOffset", "Décalage vertical du panneau" },
            { "ShowStaticCrosshair", "Afficher réticule statique" },
            { "CrosshairScale", "Taille du réticule" },
            { "ShowRepairHammer", "Afficher icône marteau réparation" },
            { "ShowEnemyHuds", "Afficher jauges de vie ennemis" },
            { "EnemyHudScale", "Taille des jauges ennemis" },
            { "StationaryGuiRecenterAngle", "Angle recentrage UI immobile" },
            { "MobileGuiRecenterAngle", "Angle recentrage UI en mouvement" },
            { "RecenterGuiOnMove", "Recentrer UI lors du déplacement" },
            { "GuiRecenterSpeed", "Vitesse de recentrage UI" },
            { "UnlockDesktopCursor", "Déverrouiller curseur PC" },
            { "QuickMenuType", "Style du Menu Rapide" },
            { "QuickMenuVerticalAngle", "Angle vertical menu rapide" },
            { "LockGuiWhileInventoryOpen", "Fixer l'UI quand inventaire ouvert" },
            { "AutoOpenKeyboardOnInteract", "Ouvrir clavier virtuel auto" },

            // HUD VR
            { "UseLegacyHud", "Utiliser l'ancien HUD classique" },
            { "CameraHudScale", "Échelle du HUD caméra" },
            { "HealthPanelPlacement", "Position jauge de santé" },
            { "StaminaPanelPlacement", "Position jauge d'endurance" },
            { "EitrPanelPlacement", "Position jauge d'Eitr (magie)" },
            { "AdrenalinePanelPlacement", "Position jauge d'adrénaline" },
            { "StaggerPanelPlacement", "Position jauge d'étourdissement" },
            { "MinimapPanelPlacement", "Position de la minicarte" },
            { "AllowHudFade", "Estompage automatique du HUD" },
            { "HideHotbar", "Masquer la barre de raccourcis" },
            { "AlwaysShowStamina", "Toujours afficher l'endurance" },
            { "AttachInventoryToHand", "Ancrer inventaire à la main" },
            { "AttachBuildMenuToHand", "Ancrer menu build à la main" },
            { "RightWristPos", "Position poignet droit" },
            { "LeftWristPos", "Position poignet gauche" },
            { "RightWristQuickBarPos", "Position barre rapide droite" },
            { "LeftWristQuickBarPos", "Position barre rapide gauche" },
            { "QuickActionOnLeftHand", "Action rapide sur main gauche" },
            { "QuickBarQuantity", "Nombre d'emplacements barre rapide" },

            // Contrôles
            { "JoystickForwardDirection", "Orientation marche (tête ou main)" },
            { "DominantHand", "Main dominante" },
            { "SnapTurnEnabled", "Activer rotation par crans (Snap)" },
            { "SnapTurnAngle", "Angle de rotation par cran" },
            { "SmoothSnapTurn", "Transition fluide des crans" },
            { "SmoothSnapSpeed", "Vitesse de transition par cran" },
            { "SmoothTurnSpeed", "Vitesse rotation fluide" },
            { "InvertXAxis", "Inverser axe horizontal rotation" },
            { "CharaterMovesWithHeadset", "Déplacement avec le casque" },
            { "RoomScaleSneakHeight", "Hauteur pour posture accroupie" },
            { "SneakInput", "Méthode d'accroupissement" },
            { "GesturedLocomotion", "Locomotion gestuelle (nage/saut)" },
            { "GesturedJumpPreparationHeight", "Hauteur prépa saut physique" },
            { "GesturedJumpMinSpeed", "Vitesse min saut gestuel" },
            { "WalkSpeedSmoothener", "Lissage vitesse marche" },
            { "SwingSpeedRequirement", "Vitesse mini frappe d'arme (m/s)" },
            { "MomentumScalesAttackDamage", "Dégâts indexés sur la vitesse" },
            { "RunIsToggled", "Maintenir pour courir (Toggle)" },
            { "AutoRunThreshold", "Seuil de course automatique" },
            { "OneHandedBow", "Maniement arc à une seule main" },
            { "AdvancedBuildMode", "Mode construction avancée" },
            { "FreePlaceAutoReturn", "Retour auto placement libre" },
            { "AdvancedRotationUpWorld", "Rotation selon repère monde" },
            { "BuildOnRelease", "Construire au relâchement bouton" },
            { "BuildAngleSnap", "Alignement angulaire construction" },

            // Graphismes
            { "UseAmplifyOcclusion", "Activer occlusion ambiante (Amplify)" },
            { "TaaSharpenAmmount", "Netteté TAA (Anti-aliasing)" },
            { "NearClipPlane", "Plan de découpe proche caméra" },
            { "RangedWeaponGlow", "Lueur des armes à distance" },
            { "MeleeWeaponGlow", "Lueur des armes de mêlée" },
            { "MagicBarrierOvelay", "Effet visuel barrière magique" },
            { "EnemyRenderDistance", "Distance d'affichage des ennemis" },
            { "BuildingPieceDetailReductionFactor", "Optimisation détails bâtiments" },
            { "ShowDamageText", "Afficher chiffres de dégâts" },
            { "ShowAttackOutline", "Afficher contours d'attaque" },

            // Contrôles Gestuels (Motion Control)
            { "EnableBowAimSmoothing", "Lissage visée arc (anti-tremblement)" },
            { "BowAimSmoothingStrength", "Force du lissage de l'arc" },
            { "UseArrowPredictionGraphic", "Trajectoire de flèche visible" },
            { "ArrowParticleSize", "Taille des particules de flèche" },
            { "SpearThrowingMode", "Mode de lancer de la lance" },
            { "UseSpearDirectionGraphic", "Ligne de visée de lance" },
            { "FullThrowSpeed", "Vitesse main pour lancer max" },
            { "SpearInverseWield", "Tenue de lance inversée (estoc)" },
            { "TwoHandedWield", "Maniement à deux mains" },
            { "TwoHandedWithShield", "Arme 2 mains avec bouclier" },
            { "ArrowRestElevation", "Hauteur du repose-flèche" },
            { "ArrowRestSide", "Côté d'appui de la flèche" },
            { "BowDrawRestrictType", "Restriction tension de l'arc" },
            { "BowFullDrawLength", "Allonge complète de l'arc (m)" },
            { "BowAccuracyIgnoresDrawLength", "Précision basée sur le temps" },
            { "BowStaminaAdjust", "Consommation endurance à l'arc" },
            { "CrossbowSaggitalRotationSource", "Source rotation arbalète" },
            { "CrossbowManualReload", "Rechargement manuel arbalète" },
            { "BlockingType", "Méthode de blocage et parade" },
            { "KnifeMovementSecondaryAttack", "Bond d'attaque au couteau" }
        };

        private static readonly Dictionary<string, string> Descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "EnableComfortVignette", "Assombrit dynamiquement les bords de l'écran lors des mouvements vifs, rotations et navigation en bateau pour éliminer le mal des transports (cinétose)." },
            { "ComfortVignetteIntensity", "Niveau d'opacité maximal de l'effet de vignette de confort (0.1 = très léger, 1.0 = très prononcé)." },
            { "ComfortVignetteOnTurning", "Active la vignette de confort lors des rotations rapides de caméra ou de tête." },
            { "ComfortVignetteOnSailing", "Active la vignette de confort lors de la navigation sur les vagues de l'océan." },
            { "ComfortVignetteOnSprint", "Active la vignette de confort lors des accélérations et sprints rapides." },
            { "EnableBowAimSmoothing", "Filtre les micro-tremblements physiques des manettes VR lorsque vous tendez l'arc pour assurer des tirs précis à longue distance." },
            { "BowAimSmoothingStrength", "Intensité du filtre stabilisateur (0.1 = faible, 0.9 = stabilisation très forte)." },
            { "RecenterOnStart", "Recentrer automatiquement l'affichage du casque VR vers l'avant au chargement de la partie." },
            { "SnapTurnEnabled", "Tourne par crans angulaires au lieu d'une rotation continue pour préserver le confort en VR." },
            { "SnapTurnAngle", "Nombre de degrés pivotés à chaque impulsion du stick analogique (ex: 30°, 45°)." },
            { "SmoothTurnSpeed", "Vitesse de rotation fluide si la rotation par crans est désactivée." },
            { "DominantHand", "Définit votre main principale (Right = Droitier, Left = Gaucher)." },
            { "OneHandedBow", "Option d'accessibilité permettant d'armer et tirer à l'arc avec une seule manette." },
            { "TwoHandedWield", "Permet de tenir les armes lourdes et d'hast à deux mains avec les deux manettes VR." },
            { "CrossbowManualReload", "Nécessite d'armer physiquement la corde de l'arbalète avec la seconde main." },
            { "ShowDamageText", "Affiche les indicateurs de dégâts textuels au-dessus des cibles touchées." },
            { "UseAmplifyOcclusion", "Améliore la profondeur des ombres de contact et le relief ambiant en réalité virtuelle." }
        };

        private static readonly Dictionary<string, string> Values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Right", "Droitier / Droite" },
            { "Left", "Gaucher / Gauche" },
            { "None", "Désactivé" },
            { "Classic", "Classique" },
            { "Full", "Complet" },
            { "Partial", "Partiel" },
            { "Gesture", "Mouvement Gestuel" },
            { "GrabButton", "Bouton Saisie" },
            { "Realistic", "Réaliste (Impact précis)" },
            { "Disabled", "Désactivé" },
            { "Sticky", "Verrouillé aux mains" },
            { "PolearmSticky", "Verrouillage armes d'hast" },
            { "SwimAndSteering", "Nage et Gouvernail" },
            { "WorldUp", "Verticale Monde" },
            { "ShipUp", "Incliné avec le Bateau" }
        };

        public static string Localize(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (Sections.TryGetValue(text, out string s)) return s;
            if (Keys.TryGetValue(text, out string k)) return k;
            if (Values.TryGetValue(text, out string v)) return v;
            return text;
        }

        public static string LocalizeSection(string section)
        {
            return Sections.TryGetValue(section, out string val) ? val : section;
        }

        public static string LocalizeKey(string key)
        {
            return Keys.TryGetValue(key, out string val) ? val : key;
        }

        public static string LocalizeValue(string val)
        {
            return Values.TryGetValue(val, out string res) ? res : val;
        }

        public static string LocalizeDescription(string key, string fallbackDescription)
        {
            if (Descriptions.TryGetValue(key, out string desc))
            {
                return desc;
            }
            return fallbackDescription;
        }
    }
}
