using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent _onClick;
    [SerializeField] private Image _image;
    private CanvasGroup _canvasgroup;   

    private bool _isActivated = true;

    public bool IsActivated { get => _isActivated; }

    public void Awake()
    {
        _canvasgroup = GetComponent<CanvasGroup>();

        if(_canvasgroup == null)
            return;

        _canvasgroup.alpha = 0.5f;
    }

    public void Activate()
    {
        _canvasgroup.DOFade(1, 0.2f).SetEase(Ease.OutCubic);
        _canvasgroup.interactable = true;
        _canvasgroup.blocksRaycasts = true;
        _isActivated = true;
    }

    public void Deactivate()
    {
        _canvasgroup.DOFade(0.5f, 0.2f).SetEase(Ease.OutCubic);
        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;
        _isActivated = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.1f);
        _onClick.Invoke();
    }
}
