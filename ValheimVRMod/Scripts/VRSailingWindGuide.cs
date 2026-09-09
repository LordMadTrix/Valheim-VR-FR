using UnityEngine;
using ValheimVRMod.Utilities;
using ValheimVRMod.VRCore;

namespace ValheimVRMod.Scripts
{
    /// <summary>
    /// Indicateur physique de vent et girouette pour les drakkars et bateaux vikings.
    /// Permet de barrer et régler la voilure à l'instinct sans quitter des yeux l'océan
    /// ni devoir regarder la minicarte.
    /// </summary>
    public class VRSailingWindGuide : MonoBehaviour
    {
        private GameObject _indicatorObj;
        private Transform _arrowTransform;
        private LineRenderer _lineRenderer;

        private void Start()
        {
            CreateIndicator();
        }

        private void CreateIndicator()
        {
            if (_indicatorObj == null)
            {
                _indicatorObj = new GameObject("VRSailingWindIndicator");
                _indicatorObj.transform.SetParent(transform, false);

                var arrowObj = new GameObject("WindArrow");
                arrowObj.transform.SetParent(_indicatorObj.transform, false);
                _arrowTransform = arrowObj.transform;

                _lineRenderer = arrowObj.AddComponent<LineRenderer>();
                _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                _lineRenderer.startWidth = 0.04f;
                _lineRenderer.endWidth = 0.01f;
                _lineRenderer.useWorldSpace = true;
                _lineRenderer.positionCount = 3;

                _indicatorObj.SetActive(false);
            }
        }

        private void Update()
        {
            if (!VHVRConfig.IsSailingWindIndicatorEnabled())
            {
                if (_indicatorObj != null && _indicatorObj.activeSelf)
                {
                    _indicatorObj.SetActive(false);
                }
                return;
            }

            Ship ship = Ship.GetLocalShip();
            var player = Player.m_localPlayer;

            if (ship == null || player == null || EnvMan.instance == null || VRPlayer.vrCam == null)
            {
                if (_indicatorObj != null && _indicatorObj.activeSelf)
                {
                    _indicatorObj.SetActive(false);
                }
                return;
            }

            if (!_indicatorObj.activeSelf)
            {
                _indicatorObj.SetActive(true);
            }

            // Direction du vent mondial
            Vector3 windDir = EnvMan.instance.GetWindDir();
            windDir.y = 0;
            windDir.Normalize();

            // Positionnement à 1.5m devant et légèrement au-dessus de la vue ou de la barre
            Vector3 centerPos = VRPlayer.vrCam.transform.position + (ship.transform.forward * 1.2f) + (Vector3.up * 0.35f);

            // Calcul du vent relatif au bateau
            float windAngle = ship.GetWindAngle();
            Color windColor;
            if (windAngle < 45f)
            {
                // Vent de face : voile inefficace (Rouge)
                windColor = new Color(1.0f, 0.25f, 0.25f, 0.85f);
            }
            else if (windAngle < 90f)
            {
                // Vent de travers modéré (Orange)
                windColor = new Color(1.0f, 0.75f, 0.2f, 0.85f);
            }
            else
            {
                // Vent portant optimal : pleine vitesse (Vert émeraude)
                windColor = new Color(0.2f, 1.0f, 0.4f, 0.9f);
            }

            if (_lineRenderer != null)
            {
                _lineRenderer.startColor = windColor;
                _lineRenderer.endColor = new Color(windColor.r, windColor.g, windColor.b, 0.2f);

                Vector3 startPos = centerPos - (windDir * 0.25f);
                Vector3 midPos = centerPos + (windDir * 0.25f);
                Vector3 endPos = midPos + (windDir * 0.15f);

                _lineRenderer.SetPosition(0, startPos);
                _lineRenderer.SetPosition(1, midPos);
                _lineRenderer.SetPosition(2, endPos);
            }
        }

        private void OnDestroy()
        {
            if (_indicatorObj != null)
            {
                Destroy(_indicatorObj);
            }
        }
    }
}
