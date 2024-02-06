using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unlock.Mario
{
    public class CurvedLineRenderer : MonoBehaviour
    {
        [SerializeField, Min(3)] private int _segmentsCount;    
        [SerializeField] private float _trajectoryTimeStep = .05f;
        [SerializeField] private int _trajectoryStepsCount = 15;

        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void DrawTrajectory(Vector2 _velocity)
        {
            Vector3[] positions = new Vector3[_trajectoryStepsCount];

            for (int i = 0; i < _trajectoryStepsCount; i++)
            {
                float t = i * _trajectoryTimeStep;
                Vector3 pos = (Vector2)transform.position + _velocity * t + .5f * Physics2D.gravity * t * t;

                positions[i] = pos;
            }

            _lineRenderer.positionCount = _trajectoryStepsCount;
            _lineRenderer.SetPositions(positions);
        }

        public void InitializeLineRenderer()
        {
            _lineRenderer.positionCount = _segmentsCount + 1;

            for (int i = 0; i < _segmentsCount; i++)
            {
                _lineRenderer.SetPosition(i, GetCurvePoint(a, b, c, i / (float)_segmentsCount));
            }

            _lineRenderer.SetPosition(_segmentsCount, GetCurvePoint(a, b, c, 1));
        }

        private Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 p1 = Vector3.Lerp(a, b, t);
            Vector3 p2 = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(p1, p2, t);
        }
    }
}
