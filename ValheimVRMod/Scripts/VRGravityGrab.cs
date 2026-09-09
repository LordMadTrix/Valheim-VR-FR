using System.Collections.Generic;
using UnityEngine;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace ValheimVRMod.Scripts
{
    /// <summary>
    /// Ramassage gestuel télékinétique à distance ("Gravity Grab" style Half-Life: Alyx).
    /// Permet de pointer un objet au sol (bois, pierre, baies, loot) et de l'attirer instantanément
    /// sans avoir à se baisser physiquement à chaque fois.
    /// </summary>
    public class VRGravityGrab : MonoBehaviour
    {
        private const float GRAB_ANGLE_THRESHOLD = 30f;
        private float _checkTimer = 0f;
        private ItemDrop _targetItemRight = null;
        private ItemDrop _targetItemLeft = null;

        private void Update()
        {
            if (!VHVRConfig.IsGravityGrabEnabled() || Game.IsPaused() || Player.m_localPlayer == null)
            {
                return;
            }

            _checkTimer += Time.deltaTime;
            if (_checkTimer >= 0.08f)
            {
                _checkTimer = 0f;
                UpdateTargetItem(true, ref _targetItemRight);
                UpdateTargetItem(false, ref _targetItemLeft);
            }

            CheckGrabInput(true, ref _targetItemRight);
            CheckGrabInput(false, ref _targetItemLeft);
        }

        private void UpdateTargetItem(bool isRightHand, ref ItemDrop targetItem)
        {
            var hand = isRightHand ? VRPlayer.rightHand : VRPlayer.leftHand;
            if (hand == null)
            {
                targetItem = null;
                return;
            }

            float maxRange = VHVRConfig.GetGravityGrabRange();
            Vector3 handPos = hand.transform.position;
            Vector3 handForward = hand.transform.forward;

            Collider[] hits = Physics.OverlapSphere(handPos, maxRange, LayerMask.GetMask("item", "Default"));
            ItemDrop closest = null;
            float bestScore = float.MaxValue;

            foreach (var col in hits)
            {
                ItemDrop itemDrop = col.GetComponentInParent<ItemDrop>();
                if (itemDrop == null || !itemDrop.CanPickup())
                {
                    continue;
                }

                Vector3 dirToItem = (itemDrop.transform.position - handPos).normalized;
                float angle = Vector3.Angle(handForward, dirToItem);
                float distance = Vector3.Distance(handPos, itemDrop.transform.position);

                if (angle <= GRAB_ANGLE_THRESHOLD && distance > 0.6f && distance <= maxRange)
                {
                    float score = distance + (angle * 0.1f);
                    if (score < bestScore)
                    {
                        bestScore = score;
                        closest = itemDrop;
                    }
                }
            }

            targetItem = closest;
        }

        private void CheckGrabInput(bool isRightHand, ref ItemDrop targetItem)
        {
            if (targetItem == null) return;

            var inputSource = isRightHand ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
            var hand = isRightHand ? VRPlayer.rightHand : VRPlayer.leftHand;

            // Déclenchement sur le bouton Grip (Saisie)
            bool isGrabPressed = SteamVR_Actions.valheim_Grab.GetStateDown(inputSource);

            if (isGrabPressed)
            {
                ExecuteGravityGrab(targetItem, hand, inputSource);
                targetItem = null;
            }
        }

        private void ExecuteGravityGrab(ItemDrop itemDrop, Hand hand, SteamVR_Input_Sources inputSource)
        {
            var player = Player.m_localPlayer;
            if (player == null || itemDrop == null) return;

            // Retour haptique dans la manette
            try
            {
                hand?.hapticAction?.Execute(0, 0.15f, 130f, 0.7f * VHVRConfig.GetHapticIntensityMultiplier(), inputSource);
            }
            catch { }

            // Ramassage effectif de l'objet par le joueur
            player.Pickup(itemDrop.gameObject, autoequip: false, autoPickupDelay: false);
        }
    }
}
