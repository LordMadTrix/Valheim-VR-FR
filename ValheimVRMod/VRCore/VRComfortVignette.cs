using System;
using UnityEngine;
using UnityEngine.UI;
using ValheimVRMod.Utilities;

namespace ValheimVRMod.VRCore
{
    /// <summary>
    /// Dynamic VR Comfort Vignette to reduce motion sickness during rapid turning, sprinting, and sailing.
    /// </summary>
    public class VRComfortVignette : MonoBehaviour
    {
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private Image _vignetteImage;
        private Texture2D _vignetteTexture;

        private float _lastCameraYaw;
        private float _currentAlpha;

        private void Awake()
        {
            CreateVignetteUI();
            _lastCameraYaw = transform.eulerAngles.y;
        }

        private void OnDestroy()
        {
            if (_vignetteTexture != null)
            {
                Destroy(_vignetteTexture);
            }
            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
            }
        }

        private void CreateVignetteUI()
        {
            var canvasGo = new GameObject("VRComfortVignetteCanvas");
            canvasGo.transform.SetParent(transform, false);
            canvasGo.transform.localPosition = new Vector3(0f, 0f, 0.25f);
            canvasGo.transform.localRotation = Quaternion.identity;

            _canvas = canvasGo.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.sortingOrder = 9999;

            var rectTransform = canvasGo.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0.8f, 0.8f);

            _canvasGroup = canvasGo.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            var imageGo = new GameObject("VignetteImage");
            imageGo.transform.SetParent(canvasGo.transform, false);

            var imgRect = imageGo.AddComponent<RectTransform>();
            imgRect.anchorMin = Vector2.zero;
            imgRect.anchorMax = Vector2.one;
            imgRect.sizeDelta = Vector2.zero;

            _vignetteImage = imageGo.AddComponent<Image>();
            _vignetteTexture = GenerateVignetteTexture(128);
            _vignetteImage.sprite = Sprite.Create(
                _vignetteTexture,
                new Rect(0, 0, _vignetteTexture.width, _vignetteTexture.height),
                new Vector2(0.5f, 0.5f)
            );
            _vignetteImage.color = Color.black;
            _vignetteImage.raycastTarget = false;
        }

        private Texture2D GenerateVignetteTexture(int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;

            float half = size * 0.5f;
            Color[] pixels = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = (x - half) / half;
                    float dy = (y - half) / half;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    // Transparent in center, progressive dark towards outer ring
                    float innerRadius = 0.35f;
                    float outerRadius = 0.95f;
                    float t = Mathf.Clamp01((dist - innerRadius) / (outerRadius - innerRadius));
                    float alpha = t * t * (3f - 2f * t); // smoothstep

                    pixels[y * size + x] = new Color(0f, 0f, 0f, alpha);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        private void Update()
        {
            if (!VHVRConfig.EnableComfortVignette())
            {
                if (_canvasGroup != null && _canvasGroup.alpha > 0f)
                {
                    _canvasGroup.alpha = 0f;
                }
                return;
            }

            float currentYaw = transform.eulerAngles.y;
            float dt = Time.deltaTime;
            if (dt <= 0f) return;

            float turnRate = Mathf.Abs(Mathf.DeltaAngle(_lastCameraYaw, currentYaw)) / dt;
            _lastCameraYaw = currentYaw;

            float maxIntensity = VHVRConfig.ComfortVignetteIntensity();
            float targetAlpha = 0f;

            // 1. Turning vignette (smooth turning or sudden snap)
            if (VHVRConfig.ComfortVignetteOnTurning() && turnRate > 25f)
            {
                float turnFactor = Mathf.Clamp01((turnRate - 25f) / 60f);
                targetAlpha = Mathf.Max(targetAlpha, turnFactor * maxIntensity);
            }

            // 2. Sprint / High-velocity locomotion vignette
            if (Player.m_localPlayer != null && !Player.m_localPlayer.IsDead())
            {
                Vector3 vel = Player.m_localPlayer.GetVelocity();
                float horizontalSpeed = new Vector2(vel.x, vel.z).magnitude;

                if (VHVRConfig.ComfortVignetteOnSprint() && horizontalSpeed > 4.5f)
                {
                    float sprintFactor = Mathf.Clamp01((horizontalSpeed - 4.5f) / 4.0f);
                    targetAlpha = Mathf.Max(targetAlpha, sprintFactor * maxIntensity * 0.85f);
                }

                // 3. Sailing vignette (boat rocking and sea movement)
                if (VHVRConfig.ComfortVignetteOnSailing() && Player.m_localPlayer.GetStandingOnShip() != null)
                {
                    float boatSpeed = horizontalSpeed;
                    if (boatSpeed > 2.5f)
                    {
                        float sailFactor = Mathf.Clamp01((boatSpeed - 2.5f) / 6.0f);
                        targetAlpha = Mathf.Max(targetAlpha, sailFactor * maxIntensity * 0.75f);
                    }
                }
            }

            // Smooth fade in and out
            float fadeSpeed = (targetAlpha > _currentAlpha) ? 5.0f : 2.5f;
            _currentAlpha = Mathf.MoveTowards(_currentAlpha, targetAlpha, fadeSpeed * dt);

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = _currentAlpha;
            }
        }
    }
}
