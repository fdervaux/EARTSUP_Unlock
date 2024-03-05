using UnityEngine;

namespace Unlock.Mario
{
    public class CurvedLineRenderer : MonoBehaviour
    {
        [SerializeField, Min(3)] private int segmentsCount;    
        [SerializeField] private float trajectoryTimeStep = .05f;
        [SerializeField] private int trajectoryStepsCount = 15;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void DrawTrajectory(Vector2 velocity)
        {
            Vector3[] positions = new Vector3[trajectoryStepsCount];

            for (int i = 0; i < trajectoryStepsCount; i++)
            {
                float t = i * trajectoryTimeStep;
                Vector3 pos = (Vector2)transform.position + velocity * t + .5f * Physics2D.gravity * t * t;

                positions[i] = pos;
            }

            _lineRenderer.positionCount = trajectoryStepsCount;
            _lineRenderer.SetPositions(positions);
        }

        // Not used (test for Bezier curves)
        public void InitializeLineRenderer(Vector3 a, Vector3 b, Vector3 c)
        {
            _lineRenderer.positionCount = segmentsCount + 1;

            for (int i = 0; i < segmentsCount; i++)
            {
                _lineRenderer.SetPosition(i, GetCurvePoint(a, b, c, i / (float)segmentsCount));
            }

            _lineRenderer.SetPosition(segmentsCount, GetCurvePoint(a, b, c, 1));
        }

        public void ResetLineRenderer()
        {
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, Vector2.zero);
            _lineRenderer.SetPosition(1, Vector2.zero);
        }

        private Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 p1 = Vector3.Lerp(a, b, t);
            Vector3 p2 = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(p1, p2, t);
        }
    }
}
