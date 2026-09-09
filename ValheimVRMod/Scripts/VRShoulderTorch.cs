using UnityEngine;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;

namespace ValheimVRMod.Scripts
{
    /// <summary>
    /// Torche mains libres attachée à l'épaule gauche.
    /// Permet de continuer d'éclairer son chemin et les cryptes sombres tout en maniant
    /// la pioche ou les armes à deux mains, dès lors qu'une torche est possédée dans l'inventaire.
    /// </summary>
    public class VRShoulderTorch : MonoBehaviour
    {
        private GameObject _torchLightObj;
        private Light _torchLight;
        private float _checkTimer = 0f;
        private bool _hasTorchInInventory = false;

        private void Start()
        {
            CreateLight();
        }

        private void CreateLight()
        {
            if (_torchLightObj == null)
            {
                _torchLightObj = new GameObject("VRShoulderTorchLight");
                _torchLightObj.transform.SetParent(transform, false);
                _torchLightObj.transform.localPosition = new Vector3(-0.25f, -0.1f, 0.15f);

                _torchLight = _torchLightObj.AddComponent<Light>();
                _torchLight.type = LightType.Point;
                _torchLight.color = new Color(1.0f, 0.68f, 0.38f); // Teinte ambrée flamme viking
                _torchLight.range = 14f;
                _torchLight.shadows = LightShadows.None; // Pas de surcoût d'ombres
                _torchLight.enabled = false;
            }
        }

        private void Update()
        {
            if (!VHVRConfig.IsShoulderTorchEnabled())
            {
                if (_torchLight != null && _torchLight.enabled)
                {
                    _torchLight.enabled = false;
                }
                return;
            }

            _checkTimer += Time.deltaTime;
            if (_checkTimer >= 1.0f)
            {
                _checkTimer = 0f;
                CheckTorchInInventory();
            }

            if (_torchLight != null)
            {
                bool shouldBeOn = _hasTorchInInventory && Player.m_localPlayer != null && !Player.m_localPlayer.IsDead();
                _torchLight.enabled = shouldBeOn;
                if (shouldBeOn)
                {
                    _torchLight.intensity = 1.3f * VHVRConfig.GetShoulderTorchBrightness();
                    // Suivi doux de l'épaule gauche
                    if (VRPlayer.vrCam != null)
                    {
                        var camTransform = VRPlayer.vrCam.transform;
                        _torchLightObj.transform.position = camTransform.position + (camTransform.right * -0.22f) + (camTransform.up * -0.15f) + (camTransform.forward * 0.1f);
                    }
                }
            }
        }

        private void CheckTorchInInventory()
        {
            var player = Player.m_localPlayer;
            if (player == null || player.GetInventory() == null)
            {
                _hasTorchInInventory = false;
                return;
            }

            // Vérifie si le joueur a une torche dans son inventaire
            var items = player.GetInventory().GetAllItems();
            _hasTorchInInventory = false;
            foreach (var it in items)
            {
                if (it.m_shared != null && it.m_shared.m_name != null)
                {
                    string name = it.m_shared.m_name.ToLower();
                    if (name.Contains("torch") || name.Contains("torche"))
                    {
                        _hasTorchInInventory = true;
                        break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (_torchLightObj != null)
            {
                Destroy(_torchLightObj);
            }
        }
    }
}
