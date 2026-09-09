using UnityEngine;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;

namespace ValheimVRMod.Scripts
{
    /// <summary>
    /// Gestionnaire de caméra spectatrice / miroir PC cinématique.
    /// Lisse la rotation et les mouvements sur l'écran d'ordinateur pour éliminer la nausée
    /// pour les personnes qui regardent ou lors des diffusions en direct (Discord/Twitch).
    /// </summary>
    public class VRSpectatorCamera : MonoBehaviour
    {
        private Camera _spectatorCam;
        private Quaternion _smoothedRotation;
        private Vector3 _smoothedPosition;
        private bool _initialized = false;

        private void Start()
        {
            SetupSpectatorCamera();
        }

        private void SetupSpectatorCamera()
        {
            if (_spectatorCam != null || VRPlayer.vrCam == null)
            {
                return;
            }

            GameObject camObj = new GameObject("VRSmoothedSpectatorCamera");
            camObj.transform.SetParent(transform, false);

            _spectatorCam = camObj.AddComponent<Camera>();
            _spectatorCam.CopyFrom(VRPlayer.vrCam);
            _spectatorCam.depth = 3.5f;
            _spectatorCam.stereoTargetEye = StereoTargetEyeMask.None;
            _spectatorCam.cullingMask |= (1 << LayerUtils.CHARARCTER_TRIGGER);
            _spectatorCam.cullingMask |= (1 << LayerUtils.getUiPanelLayer());
            _spectatorCam.cullingMask |= (1 << LayerUtils.getWorldspaceUiLayer());
            _spectatorCam.enabled = VHVRConfig.IsSpectatorSmoothingEnabled();

            _smoothedPosition = VRPlayer.vrCam.transform.position;
            _smoothedRotation = VRPlayer.vrCam.transform.rotation;
            _initialized = true;
        }

        private void LateUpdate()
        {
            if (VRPlayer.vrCam == null)
            {
                return;
            }

            if (!_initialized)
            {
                SetupSpectatorCamera();
                return;
            }

            bool isEnabled = VHVRConfig.IsSpectatorSmoothingEnabled() && !VHVRConfig.UseThirdPersonCameraOnFlatscreen();
            if (_spectatorCam.enabled != isEnabled)
            {
                _spectatorCam.enabled = isEnabled;
            }

            if (!isEnabled)
            {
                return;
            }

            float smoothFactor = VHVRConfig.GetSpectatorSmoothFactor();
            float t = Time.unscaledDeltaTime * smoothFactor;

            _smoothedPosition = Vector3.Lerp(_smoothedPosition, VRPlayer.vrCam.transform.position, Mathf.Clamp01(t));
            _smoothedRotation = Quaternion.Slerp(_smoothedRotation, VRPlayer.vrCam.transform.rotation, Mathf.Clamp01(t));

            _spectatorCam.transform.position = _smoothedPosition;
            _spectatorCam.transform.rotation = _smoothedRotation;
            _spectatorCam.fieldOfView = VRPlayer.vrCam.fieldOfView > 0 ? VRPlayer.vrCam.fieldOfView : 75f;
        }
    }
}
