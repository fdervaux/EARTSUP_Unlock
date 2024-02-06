using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Subnautica
{
    public class LabyrinthControler : MonoBehaviour
    {
        public int _distance;
        public Vector3 _startPosition;
        public Quaternion _startQuaternion;

        void Start()
        {
            _startPosition = transform.position;
            _startQuaternion = transform.rotation;
        }

        [ContextMenu("Reset")]
        public void ResetGame()
        {
            transform.position = _startPosition;
            transform.rotation = _startQuaternion;
        }

        public void Move(Vector3 direction)
        {
            // print("Input do stuff !");

            if (!CheckWall(transform.forward * direction.z * _distance))
                transform.position += transform.forward * direction.z * _distance;

            Vector3 newEulerAngle = transform.eulerAngles;
            newEulerAngle.y += 90 * direction.x;
            transform.eulerAngles = newEulerAngle;
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
            {
                // print(l.name);
                return true;
            }
            else
            {
                // print("Y'a r");
                return false;
            }
        }

        public void OnWASD(InputValue value)
        {
            // Vector2 osef = value.Get<Vector2>();
            // if (osef.x > 0 && transform.position.x < 1)
            //     transform.position += Vector3.right;
            // //! droite
            // if (osef.x < 0 && transform.position.x > -1)
            //     transform.position += Vector3.left;
            //     //! gauche

            //     // print("Input");
            Vector3 inputVector = Vector3.zero;
            inputVector.x = value.Get<Vector2>().x;
            inputVector.z = value.Get<Vector2>().y;

            if (inputVector != Vector3.zero)
                Move(inputVector);
        }
    }
}
