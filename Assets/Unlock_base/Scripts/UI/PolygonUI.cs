using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIExtensions
{
    /// Set pivot without changing the position of the element
    /// </summary>
    public static void SetPivot(this RectTransform rect, Vector2 pivot)
    {
        Vector3 deltaPosition = rect.pivot - pivot;    // get change in pivot

        deltaPosition.Scale(rect.rect.size);           // apply sizing
        deltaPosition.Scale(rect.localScale);          // apply scaling

        deltaPosition = rect.transform.localRotation * deltaPosition; // apply rotation

        rect.pivot = pivot;                            // change the pivot
        rect.localPosition -= deltaPosition;           // reverse the position change
    }
}

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/Primitives/UI Polygon")]
    public class PolygonUI : MaskableGraphic
    {
        [SerializeField]
        Texture m_Texture;
        public bool _fill = true;
        public float _thickness = 5;
        [Range(3, 360)]
        public int _sides = 3;
        [Range(0, 360)]
        public float _rotation = 0;
        [Range(0, 1)]
        public float[] _verticesDistances = new float[3];
        private float _size = 0;

        public override Texture mainTexture
        {
            get
            {
                return m_Texture == null ? s_WhiteTexture : m_Texture;
            }
        }
        public Texture texture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                if (m_Texture == value) return;
                m_Texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }
        public void DrawPolygon(int _sides)
        {
            this._sides = _sides;
            _verticesDistances = new float[_sides + 1];
            for (int i = 0; i < _sides; i++) _verticesDistances[i] = 1; ;
            _rotation = 0;
        }
        public void DrawPolygon(int _sides, float[] _VerticesDistances)
        {
            this._sides = _sides;
            _verticesDistances = _VerticesDistances;
            _rotation = 0;
        }
        public void DrawPolygon(int _sides, float[] _VerticesDistances, float _rotation)
        {
            this._sides = _sides;
            _verticesDistances = _VerticesDistances;
            this._rotation = _rotation;
        }
        void Update()
        {
            _size = rectTransform.rect.width;
            if (rectTransform.rect.width > rectTransform.rect.height)
                _size = rectTransform.rect.height;
            else
                _size = rectTransform.rect.width;
            _thickness = (float)Mathf.Clamp(_thickness, 0, _size / 2);
        }
        protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Vector2 prevX = Vector2.zero;
            Vector2 prevY = Vector2.zero;
            Vector2 uv0 = new Vector2(0, 0);
            Vector2 uv1 = new Vector2(0, 1);
            Vector2 uv2 = new Vector2(1, 1);
            Vector2 uv3 = new Vector2(1, 0);
            Vector2 pos0;
            Vector2 pos1;
            Vector2 pos2;
            Vector2 pos3;
            float degrees = 360f / _sides;
            int vertices = _sides + 1;
            if (_verticesDistances.Length != vertices)
            {
                _verticesDistances = new float[vertices];
                for (int i = 0; i < vertices - 1; i++) _verticesDistances[i] = 1;
            }

            //Debug.Log(rectTransform.pivot + " " + rectTransform.rect.center + "  " + rectTransform.anchoredPosition);


            // last vertex is also the first!
            _verticesDistances[vertices - 1] = _verticesDistances[0];
            for (int i = 0; i < vertices; i++)
            {
                float outer = -0.5f * _size * _verticesDistances[i];
                float inner = outer + _thickness;
                float rad = Mathf.Deg2Rad * (i * degrees + _rotation);
                float c = Mathf.Cos(rad);
                float s = Mathf.Sin(rad);
                uv0 = new Vector2(0, 1);
                uv1 = new Vector2(1, 1);
                uv2 = new Vector2(1, 0);
                uv3 = new Vector2(0, 0);
                pos0 = prevX;
                pos1 = new Vector2(outer * c, outer * s);
                if (_fill)
                {
                    pos2 = Vector2.zero;
                    pos3 = Vector2.zero;
                }
                else
                {
                    pos2 = new Vector2(inner * c, inner * s);
                    pos3 = prevY;
                }
                prevX = pos1;
                prevY = pos2;
                pos0 += rectTransform.rect.center;
                pos1 += rectTransform.rect.center;
                pos2 += rectTransform.rect.center;
                pos3 += rectTransform.rect.center;
                vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
            }
        }
    }
}
