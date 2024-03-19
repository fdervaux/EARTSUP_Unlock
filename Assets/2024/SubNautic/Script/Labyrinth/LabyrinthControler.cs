using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Subnautica
{
    public class LabyrinthControler : MonoBehaviour
    {
        [SerializeField] private LabyrinthCanvas _canvasManager;
        [Space]
        [SerializeField] private float _wallBumpLenth;
        [SerializeField] private float _resetDuration;
        [SerializeField] private AnimationCurve _moveAnimationCurve;
        private int _moveDistance = 2;
        private Vector3 _startPosition;
        private Quaternion _startQuaternion;
        private bool _canMove = true;


        void Start()
        {
            _startPosition = transform.position;
            _startQuaternion = transform.rotation;
        }

        [ContextMenu("Reset")]
        public void ResetGame()
        {
            _canvasManager.HideGame(_resetDuration, () =>
            {
                transform.position = _startPosition;
                transform.rotation = _startQuaternion;
            });
        }

        public void Move(Vector3 direction)
        {
            if (!_canMove)
                return;

            _canMove = false;
            if (!CheckWall(transform.forward * direction.z * _moveDistance))
                transform.position += transform.forward * direction.z * _moveDistance;
            else
            {
                Vector3 startPos = transform.position;
                DOTween.To((time) =>
                {
                    transform.position = startPos + transform.forward * _wallBumpLenth * _moveAnimationCurve.Evaluate(time);
                }, 0, 1, .09f)
                .SetEase(Ease.Linear)
                .OnComplete(() => transform.position = startPos);
            }

            Vector3 newEulerAngle = transform.eulerAngles;
            newEulerAngle.y += direction.x * 90;
            transform.DORotate(newEulerAngle, .1f)
            .OnComplete(() => _canMove = true);
        }

        bool CheckWall(Vector3 direction)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position
                        , direction
                        , out hit
                        , direction.magnitude);

            LayerAtHome l = hit.collider?.GetComponent<LayerAtHome>();
            if (l)
                return true;
            else
                return false;
        }

        public void OnWASD(InputValue value)
        {
            Vector3 inputVector = Vector3.zero;
            inputVector.x = value.Get<Vector2>().x;
            inputVector.z = value.Get<Vector2>().y;

            if (inputVector != Vector3.zero)
                Move(inputVector);
        }

        public void MoveForward() { Move(Vector3.forward); }
        public void MoveBackward() { Move(Vector3.back); }
        public void MoveRight() { Move(Vector3.right); }
        public void MoveLeft() { Move(Vector3.left); }

    }
}
