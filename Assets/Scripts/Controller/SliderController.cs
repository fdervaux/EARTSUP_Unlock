using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour
{
    [SerializeField] private RectTransform _sliderRectTransform;
    private float _progress = 0;
    [SerializeField] private RectTransform _rectTransform;


    public void SetProgress(float progress)
    {
        _progress = Mathf.Clamp01(progress);
        float max = _rectTransform.rect.width;
        _sliderRectTransform.anchoredPosition = new Vector2((_progress - 1) * max, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
