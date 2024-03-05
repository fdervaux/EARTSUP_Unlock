using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bowser_Movements : MonoBehaviour
{
    [SerializeField] private Transform _centre;
    [SerializeField] private float RotateSpeed = 5f;
    [SerializeField] private float Radius = 0.1f;
    private float _angle;

    private void Update()
    {
        _angle += RotateSpeed * Time.deltaTime;
        Vector3 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre.position + offset;
    }
}
