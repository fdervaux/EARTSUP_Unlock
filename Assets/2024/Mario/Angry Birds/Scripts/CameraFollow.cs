using UnityEngine;
using System.Collections;

namespace Unlock.Mario
{
    // Camera follow only for 4 points rectangle PolygonCollider2D
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D cameraLimits;

        private Vector2[] _worldPositionColliderPoints;

        private float _cameraWidth, _cameraHeight;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            
            if (!cameraLimits)
            {
                cameraLimits = FindObjectOfType<PolygonCollider2D>();
                
                if (!cameraLimits)
                {
                    GameObject newGameObject = Instantiate(new GameObject());
                    cameraLimits = newGameObject.AddComponent<PolygonCollider2D>();
                    cameraLimits.points = new Vector2[4];
                    newGameObject.name = "Camera Limits";
                }
            }
            
            _worldPositionColliderPoints = new Vector2[cameraLimits.points.Length];

            for (int i = 0; i < _worldPositionColliderPoints.Length; i++)
            {
                _worldPositionColliderPoints[i] = (Vector2)cameraLimits.transform.position + cameraLimits.points[i] + cameraLimits.offset;
            }

            if (Camera.main)
            {
                _cameraHeight = Camera.main.orthographicSize * 2;
                _cameraWidth = _cameraHeight * _mainCamera.aspect;
            }
        }
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);

            if (!Camera.main)
            {
                Debug.LogError("Il n'y a pas de caméra dans la scène !");
                AppHelper.Quit();
            }

            if (cameraLimits.points.Length != 4)
            {
                Debug.LogError("Le PolygonCollider2D du CameraFollow ne contient pas 4 points !");
                AppHelper.Quit();
            }
        }

        private void Update()
        {
            Vector3 cameraPosition = _mainCamera.transform.position;

            if (transform.position.x > _worldPositionColliderPoints[1].x) return;

            float xPosition = Mathf.Clamp(transform.position.x, _worldPositionColliderPoints[0].x + _cameraWidth / 2, _worldPositionColliderPoints[1].x - _cameraWidth / 2);
            float yPosition = Mathf.Clamp(transform.position.y, _worldPositionColliderPoints[3].y + _cameraHeight / 2, _worldPositionColliderPoints[0].y - _cameraHeight / 2);
            
            cameraPosition = new Vector3(xPosition, yPosition, cameraPosition.z);
            
            _mainCamera.transform.position = cameraPosition;
        }
    }
}
