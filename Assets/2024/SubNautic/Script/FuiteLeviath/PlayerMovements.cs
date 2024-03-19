using System.Collections;
using System.Collections.Generic;

using UnityEngine;


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

        // public void MoveRight()
        // {
        //     if (movementIndex + move.x < limitRight && movementIndex + move.x > limitLeft)
        //     {
        //         if (move.x > 0)
        //         { Move(Vector3.right); }
        //     }
        // }

        // public void MoveLeft()
        // {
        //     MovePlayer (Vector3.left);
        // }

        public void MovePlayer(Vector2 move)
        {
            if (movementIndex + move.x < limitRight && movementIndex + move.x > limitLeft)
            {
                print ("move");
                if (move.x > 0)
                {
                    transform.position += Vector3.right * movementSpeed;
                    movementIndex++;
                }
                if (move.x < 0)
                {
                    transform.position += Vector3.left * movementSpeed;
                    movementIndex--;
                }

            }
        }

        /* public void OnWASD(InputValue value)
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

        } */
    }

    // public void OnDrag(InputValue value)
    /*   {
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
      } */






}

