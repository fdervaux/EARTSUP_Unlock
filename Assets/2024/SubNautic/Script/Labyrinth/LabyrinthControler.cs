using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class LabyrinthControler : MonoBehaviour
{


    public void OnWASD(InputValue value)
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = value.Get<Vector2>().x;
        newPos.z = value.Get<Vector2>().y;
        transform.position += newPos;

        Vector3 newEulerAngle = transform.eulerAngles;
        newEulerAngle.y += 90 * newPos.x;
        transform.eulerAngles = newEulerAngle;
    }
}
