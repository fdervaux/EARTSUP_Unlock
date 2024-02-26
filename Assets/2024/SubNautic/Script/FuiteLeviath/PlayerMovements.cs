using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    // public GameObject posLeft, posRight, posMiddle;
    // bool isMoving;
    public float movementSpeed = 5f;
    public float movementSpeedUp = 5f;
    public float movementSpeedVertical = 5f;
    public float limitLeft = 0;
    public float limitRight = 4;
    private int movementIndex = 1;
    void Start()

    {

    }

    void Update()
    {
        transform.Translate(Vector3.up * movementSpeedUp * Time.deltaTime);
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

// void FixedUpdate()
// {

//     float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * Speed;
//     Vector3 newPosition = rb.position + Vector3.right * x;
//     newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth);
//     rb.MovePosition(newPosition);

//transform.position = transform.position + new Vector3(0, 0, 0);
// }
//}
