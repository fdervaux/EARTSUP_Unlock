using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridLayoutSizeElementByWidth : MonoBehaviour
{
    private RectTransform _container;

    private GridLayoutGroup _gridLayoutGroup;

    public void OnRectTransformDimensionsChange()
    {
        UpdateSize();
    }

    public void OnEnable()
    {
        _container = GetComponent<RectTransform>();
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        UpdateSize();
    }

    public void UpdateSize()
    {
        if (_container == null || _gridLayoutGroup == null)
        {
            return;
        }

        var width = _container.rect.width;
        var cellWidth = (width - _gridLayoutGroup.padding.left - _gridLayoutGroup.padding.right - _gridLayoutGroup.spacing.x * (_gridLayoutGroup.constraintCount - 1)) / _gridLayoutGroup.constraintCount;
        _gridLayoutGroup.cellSize = new Vector2(cellWidth, _gridLayoutGroup.cellSize.y);
    }


}
