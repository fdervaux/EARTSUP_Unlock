using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unlock.Mario
{
    public class SlingShot : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private float _launchForce = 1.5f;
        [SerializeField] private CurvedLineRenderer _curvedLineRenderer;
        [SerializeField] private GameObject _toadPrefab;

        private Vector2 _startMousePos;
        private Vector2 _velocity;
        private bool isLaunched = false;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _curvedLineRenderer = GetComponent<CurvedLineRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isLaunched)
            {
                float angle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            print("Begin drag");

            _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            print("Drag");

            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _velocity = (_startMousePos - currentMousePosition) * _launchForce;

            _curvedLineRenderer.DrawTrajectory(_velocity);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            print("End drag");

            isLaunched = true;

            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.velocity = _velocity;

            _curvedLineRenderer.GetComponent<LineRenderer>().enabled = false;
        }

    }
}
