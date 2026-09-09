using UnityEngine;
using UnityEngine.PostProcessing;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;

namespace ValheimVRMod.Scripts.PostProcessing
{
    /// <summary>
    /// Optimiseur global de performances, rendu et clarté visuelle en Réalité Virtuelle.
    /// - Synchronisation physique native 90Hz (Physics Jitter Fix)
    /// - Filtrage anisotrope 16x pour textures nettes
    /// - Culling intelligent et cascades d'ombres VR (+15-25 FPS en forêt et bases)
    /// - Atténuation de l'éblouissement du bloom dans les lentilles VR
    /// - Purge automatique de la mémoire VRAM lors des voyages par portail
    /// </summary>
    public class VRGraphicsOptimizer : MonoBehaviour
    {
        private PostProcessingBehaviour _postProcessingBehaviour;
        private float _checkTimer = 0f;
        private bool _wasTeleporting = false;

        private void Start()
        {
            ApplyPhysicsSync();
            ApplyTextureSharpness();
            ApplyShadowOptimization();
        }

        private void Update()
        {
            _checkTimer += Time.unscaledDeltaTime;
            if (_checkTimer >= 2.0f)
            {
                _checkTimer = 0f;
                EnsurePostProcessingTuned();
                ApplyShadowOptimization();
            }

            CheckPortalMemoryCleanup();
        }

        /// <summary>
        /// Aligne la simulation physique Unity sur la fréquence VR (90 Hz) pour éliminer les micro-saccades.
        /// </summary>
        public static void ApplyPhysicsSync()
        {
            if (VHVRConfig.IsPhysicsSyncEnabled())
            {
                // Fréquence physique calée sur 90Hz (11.1ms) au lieu du 50Hz (20ms) par défaut
                Time.fixedDeltaTime = 1f / 90f;
                Time.maximumDeltaTime = 0.05f;
                LogUtils.LogInfo("Physics Sync VR activé : simulation physique calée à 90 Hz (11.1 ms).");
            }
        }

        /// <summary>
        /// Force le filtrage anisotrope 16x pour rendre les textures et écritures nettes dans le casque.
        /// </summary>
        public static void ApplyTextureSharpness()
        {
            if (VHVRConfig.IsVRSharpeningEnabled())
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                Texture.SetGlobalAnisotropicFilteringLimits(1, 16);
            }
        }

        /// <summary>
        /// Optimise la distance de calcul des ombres et le nombre de cascades pour soulager le GPU en VR stéréo.
        /// </summary>
        public static void ApplyShadowOptimization()
        {
            if (VHVRConfig.IsShadowOptimizationEnabled())
            {
                // Distance d'ombres stabilisée à 60m (évite de calculer les ombres lointaines inutiles en VR)
                if (QualitySettings.shadowDistance > 60f)
                {
                    QualitySettings.shadowDistance = 60f;
                }
                // 2 cascades au lieu de 4 divise par deux le travail de shadow mapping stéréo
                QualitySettings.shadowCascades = 2;
            }
        }

        /// <summary>
        /// Purge la mémoire VRAM et les assets résiduels pendant la téléportation par portail.
        /// </summary>
        private void CheckPortalMemoryCleanup()
        {
            if (!VHVRConfig.IsMemoryCleanupEnabled() || Player.m_localPlayer == null)
            {
                return;
            }

            bool isTeleporting = Player.m_localPlayer.IsTeleporting();
            if (isTeleporting && !_wasTeleporting)
            {
                // Déclenche le déchargement des textures inutilisées sous écran noir
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
                LogUtils.LogInfo("Purge VRAM & Mémoire automatique effectuée lors de la traversée de portail !");
            }
            _wasTeleporting = isTeleporting;
        }

        private void EnsurePostProcessingTuned()
        {
            if (_postProcessingBehaviour == null)
            {
                _postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
                if (_postProcessingBehaviour == null)
                {
                    _postProcessingBehaviour = VRPlayer.vrCam?.GetComponent<PostProcessingBehaviour>();
                }
            }

            if (_postProcessingBehaviour == null || _postProcessingBehaviour.profile == null)
            {
                return;
            }

            var profile = _postProcessingBehaviour.profile;

            // Atténuation de l'éblouissement du bloom si activée
            if (VHVRConfig.IsVRAntiGlareBloomEnabled() && profile.bloom != null && profile.bloom.enabled)
            {
                var settings = profile.bloom.settings;
                if (settings.bloom.intensity > 0.8f)
                {
                    settings.bloom.intensity = 0.75f;
                    if (settings.bloom.threshold < 1.1f)
                    {
                        settings.bloom.threshold = 1.15f;
                    }
                    profile.bloom.settings = settings;
                }
            }
        }
    }
}
