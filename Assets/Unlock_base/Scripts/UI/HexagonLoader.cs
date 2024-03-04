using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HexagonLoader : MonoBehaviour
{
    private List<Vector2> _pivotPoints;
    private List<Vector2> _pivotPositions;

    public const float sqrt3on2 = 0.86602540378f;

    public int _currentHexagonIndex = 1;
    public int _currentPositionIndex = 0;

    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _pivotPoints = new List<Vector2>();
        _pivotPositions = new List<Vector2>();

        float sqrt3on2 = Mathf.Sqrt(3) / 2;

        //Hexagon veritcies
        _pivotPoints.Add(new Vector2(-1, 0));
        _pivotPoints.Add(new Vector2(-0.5f, sqrt3on2));
        _pivotPoints.Add(new Vector2(0.5f, sqrt3on2));
        _pivotPoints.Add(new Vector2(1, 0));
        _pivotPoints.Add(new Vector2(0.5f, -sqrt3on2));
        _pivotPoints.Add(new Vector2(-0.5f, -sqrt3on2));


        _pivotPositions.Add(new Vector2(-0.75f, sqrt3on2 / 2));
        _pivotPositions.Add(new Vector2(0f, sqrt3on2));
        _pivotPositions.Add(new Vector2(0.75f, sqrt3on2 / 2));
        _pivotPositions.Add(new Vector2(0.75f, -sqrt3on2 / 2));
        _pivotPositions.Add(new Vector2(0f, -sqrt3on2));
        _pivotPositions.Add(new Vector2(-0.75f, -sqrt3on2 / 2));

        for (int i = 0; i < _pivotPoints.Count; i++)
        {
            _pivotPoints[i] = _pivotPoints[i] / 2 + new Vector2(0.5f, 0.5f);
        }

        UpdatePivot();
        _rectTransform.DORotate(new Vector3(0, 0, 60), 0.8f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.OutExpo).OnStepComplete(UpdatePivot);

    }

    public void UpdatePivot()
    {
        _rectTransform.SetPivot(_pivotPoints[_currentHexagonIndex]);
        _rectTransform.localPosition = _pivotPositions[_currentPositionIndex] * _rectTransform.rect.size.x; 
        _currentHexagonIndex = (_currentHexagonIndex + 2) % 6;
        _currentPositionIndex = (_currentPositionIndex + 1) % 6;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
