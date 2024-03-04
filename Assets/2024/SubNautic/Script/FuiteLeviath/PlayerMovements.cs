using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Subnautica
{
    public class PlayerMovements : MonoBehaviour
    {
        // public GameObject posLeft, posRight, posMiddle;
        // bool isMoving;
        public float movementSpeed = 5f;
        public float movementSpeedUp = 5f;
        public float movementSpeedVertical = 5f;
        public float limitLeft = 0;
        public float limitRight = 4;
        public float magTriggerMovement = .5f;
        private int movementIndex = 1;
        private Vector2 lastFrameInputPosition;


        void Update()
        {
            transform.Translate(Vector3.up * movementSpeedUp * Time.deltaTime);
        }

        public void OnDrag(InputValue value)
        {
            // print("Input ! : " + value.Get<Vector2>());
            Vector2 drag = value.Get<Vector2>() - lastFrameInputPosition;

            if(drag.magnitude < magTriggerMovement)
                return;

            drag = drag.normalized;
            if (movementIndex + drag.x < limitRight && movementIndex + drag.x > limitLeft)
            {
                if (drag.x > 0)
                    transform.position += Vector3.right * movementSpeed;
                if (drag.x < 0)
                    transform.position += Vector3.left * movementSpeed;

                print("Move and add index : " + (int)drag.x);
                if(drag.x > 0)
                    movementIndex++;
                if(drag.x < 0)
                    movementIndex--;
            }

            // if(value.Get<Vector2>() != Vector2.zero)
            lastFrameInputPosition = value.Get<Vector2>();
        }

        public void OnWASD(InputValue value)
        {
            Vector2 move = value.Get<Vector2>();
            if (movementIndex + move.x < limitRight && movementIndex + move.x > limitLeft)
            {
                if (move.x > 0)
                    transform.position += Vector3.right * movementSpeed;
                if (move.x < 0)
                    transform.position += Vector3.left * movementSpeed;
                movementIndex += (int)move.x;
            }

        }


        //     void Update()
        //     {
        //         bool moveLeft = Input.GetKey("left");
        //         bool moveRight = Input.GetKey("right");


        //         if (moveLeft == true && isMoving == false)
        //         {
        //             bool left = true;
        // //comentairex
        // //autre commentaire
        //             Move(left: left);
        //         }
        //         else if (moveRight == true && isMoving == false)
        //         {
        //             bool right = true;

        //             Move(right: right);

        //         }
        //         if (moveRight == true | moveLeft == true)
        //             isMoving = true;
        //         else
        //             isMoving = false;


        //     }

        //     private void Move(bool left = false, bool right = false)
        //     {
        //         // GameObject player;
        //         // player = GameObject.FindGameObjectWithTag("Player");
        //         // if (left == true && player.transform.position == posMiddle.transform.position)
        //         //     player.transform.position = posLeft.transform.position;
        //         // if (left == true && player.transform.position == posRight.transform.position)
        //         //     player.transform.position = posMiddle.transform.position;
        //         // if (right == true && player.transform.position == posLeft.transform.position)
        //         //     player.transform.position = posMiddle.transform.position;
        //         // if (right == true && player.transform.position == posMiddle.transform.position)
        //         //     player.transform.position = posRight.transform.position;

        //     }
    }
}

// void FixedUpdate()
// {

//     float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * Speed;
//     Vector3 newPosition = rb.position + Vector3.right * x;
//     newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth);
//     rb.MovePosition(newPosition);

//transform.position = transform.position + new Vector3(0, 0, 0);
// }
//}
