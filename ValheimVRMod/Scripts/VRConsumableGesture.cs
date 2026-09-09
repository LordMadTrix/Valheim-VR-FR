using UnityEngine;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;
using Valve.VR;

namespace ValheimVRMod.Scripts
{
    /// <summary>
    /// Permet de consommer physiquement de la nourriture ou des potions
    /// en portant la main tenant l'aliment directement à sa bouche/casque.
    /// </summary>
    public class VRConsumableGesture : MonoBehaviour
    {
        private const float MOUTH_DISTANCE_THRESHOLD = 0.22f;
        private const float CONSUME_HOLD_DURATION = 0.32f;
        private const float COOLDOWN_DURATION = 1.2f;

        private float _rightHandHoldTimer = 0f;
        private float _leftHandHoldTimer = 0f;
        private float _cooldownTimer = 0f;

        private void Update()
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
            }

            if (!VHVRConfig.IsPhysicalConsumptionEnabled() || Game.IsPaused() || VRPlayer.ShouldPauseMovement)
            {
                _rightHandHoldTimer = 0f;
                _leftHandHoldTimer = 0f;
                return;
            }

            var localPlayer = Player.m_localPlayer;
            if (localPlayer == null || localPlayer.IsDead() || VRPlayer.vrCam == null)
            {
                return;
            }

            // Position approximative de la bouche (légèrement en dessous de la caméra VR)
            Vector3 mouthPosition = VRPlayer.vrCam.transform.position - (VRPlayer.vrCam.transform.up * 0.07f);

            // Vérification Main Droite
            CheckHandConsumption(
                localPlayer,
                localPlayer.GetRightItem(),
                VRPlayer.rightHand?.transform,
                true,
                ref _rightHandHoldTimer,
                mouthPosition
            );

            // Vérification Main Gauche
            CheckHandConsumption(
                localPlayer,
                localPlayer.GetLeftItem(),
                VRPlayer.leftHand?.transform,
                false,
                ref _leftHandHoldTimer,
                mouthPosition
            );
        }

        private void CheckHandConsumption(
            Player player,
            ItemDrop.ItemData item,
            Transform handTransform,
            bool isRightHand,
            ref float holdTimer,
            Vector3 mouthPosition)
        {
            if (item == null || handTransform == null)
            {
                holdTimer = 0f;
                return;
            }

            // Vérifie si l'objet est un consommable (Nourriture, Potion, Hydromel)
            if (item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Consumable)
            {
                holdTimer = 0f;
                return;
            }

            float distance = Vector3.Distance(handTransform.position, mouthPosition);
            float threshold = VHVRConfig.GetMouthProximityDistance();

            if (distance <= threshold)
            {
                holdTimer += Time.deltaTime;

                // Vibration de pré-ingestion légère
                if (holdTimer > 0.15f && holdTimer < 0.20f)
                {
                    ExecuteHaptic(isRightHand, 0.05f, 50f, 0.15f);
                }

                if (holdTimer >= CONSUME_HOLD_DURATION && _cooldownTimer <= 0f)
                {
                    // Consomme l'objet
                    player.UseItem(player.GetInventory(), item, false);

                    // Retour haptique franc
                    ExecuteHaptic(isRightHand, 0.25f, 120f, 0.6f);

                    // bHaptics si supporté
                    TriggerBhaptics(item);

                    _cooldownTimer = COOLDOWN_DURATION;
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }

        private void ExecuteHaptic(bool isRightHand, float duration, float frequency, float amplitude)
        {
            if (!VHVRConfig.IsHapticConsumptionFeedbackEnabled())
            {
                return;
            }

            try
            {
                var hand = isRightHand ? VRPlayer.rightHand : VRPlayer.leftHand;
                var source = isRightHand ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
                hand?.hapticAction?.Execute(0, duration, frequency, amplitude, source);
            }
            catch
            {
                // Ignore si l'action haptique est indisponible
            }
        }

        private void TriggerBhaptics(ItemDrop.ItemData item)
        {
            try
            {
                if (VHVRConfig.BhapticsEnabled() && !BhapticsTactsuit.suitDisabled)
                {
                    string name = item.m_shared.m_name.ToLower();
                    if (name.Contains("mead") || name.Contains("potion") || name.Contains("hydromel"))
                    {
                        BhapticsTactsuit.PlaybackHaptics("Drinking");
                    }
                    else
                    {
                        BhapticsTactsuit.PlaybackHaptics("Eating");
                    }
                }
            }
            catch
            {
                // Ignore
            }
        }
    }
}
