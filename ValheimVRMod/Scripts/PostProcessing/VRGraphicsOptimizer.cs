using UnityEngine;
using UnityEngine.PostProcessing;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;

namespace ValheimVRMod.Scripts.PostProcessing
{
    /// <summary>
    /// Optimiseur de rendu et de clarté visuelle en Réalité Virtuelle.
    /// Améliore la netteté des textures (Anisotropie forcée & Mipmap Bias)
    /// et atténue l'éblouissement agressif du bloom dans les lentilles VR.
    /// </summary>
    public class VRGraphicsOptimizer : MonoBehaviour
    {
        private PostProcessingBehaviour _postProcessingBehaviour;
        private bool _isOptimized = false;
        private float _checkTimer = 0f;

        private void Start()
        {
            ApplyTextureSharpness();
        }

        private void Update()
        {
            _checkTimer += Time.unscaledDeltaTime;
            if (_checkTimer >= 2.0f)
            {
                _checkTimer = 0f;
                EnsurePostProcessingTuned();
            }
        }

        /// <summary>
        /// Force le filtrage anisotrope et améliore la lisibilité des textures à distance.
        /// </summary>
        public static void ApplyTextureSharpness()
        {
            if (VHVRConfig.IsVRSharpeningEnabled())
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                Texture.SetGlobalAnisotropicFilteringLimits(1, 16);
            }
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
                // Si le bloom est trop éblouissant (au-delà de 0.8f), on le tempère doucement
                if (settings.bloom.intensity > 0.8f)
                {
                    settings.bloom.intensity = 0.75f;
                    // Seuil légèrement rehaussé pour ne faire briller que les sources très lumineuses
                    if (settings.bloom.threshold < 1.1f)
                    {
                        settings.bloom.threshold = 1.15f;
                    }
                    profile.bloom.settings = settings;
                }
            }

            _isOptimized = true;
        }
    }
}
