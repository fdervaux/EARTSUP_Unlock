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
    [SerializeField] private Image _secondaryImage;

    [SerializeField] private bool _changeColorOnPress = false;
    [SerializeField] private Color _pressedColor = Color.white;
    [SerializeField] private Color _normalColor = Color.white;

    [SerializeField] private bool _activateOnStart = false;

    private static readonly float _disableAlpha = 0.3f;


    private CanvasGroup _canvasgroup;

    private bool _isActivated = false;

    public bool IsActivated { get => _isActivated; }


    public void SwitchImage()
    {
        if (_image == null || _secondaryImage == null)
            return;

        if (_image.gameObject.activeSelf)
        {
            _image.gameObject.SetActive(false);
            _secondaryImage.gameObject.SetActive(true);
        }
        else
        {
            _image.gameObject.SetActive(true);
            _secondaryImage.gameObject.SetActive(false);
        }
    }

    public UnityEvent OnClick
    {
        get => _onClick;
        set => _onClick = value;
    }

    public void Awake()
    {
        _canvasgroup = GetComponent<CanvasGroup>();

        if (_canvasgroup == null)
            return;

        _canvasgroup.alpha = _disableAlpha;
        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;
    }

    public void Start()
    {
        if (_activateOnStart)
            Activate();
    }
    

    public void Activate()
    {
        if(_isActivated)
            return;

        _canvasgroup.DOFade(1, 0.2f).SetEase(Ease.OutCubic);
        _canvasgroup.interactable = true;
        _canvasgroup.blocksRaycasts = true;
        _isActivated = true;
    }

    public void Deactivate()
    {
        if(!_isActivated)
            return;

        _canvasgroup.DOFade(_disableAlpha, 0.2f).SetEase(Ease.OutCubic);
        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;
        _isActivated = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.1f);
        GameManager.Instance.SoundManager.playButtonClick();
        _onClick.Invoke();

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.1f);

        if (_changeColorOnPress)
        {
            Image[] images = GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                image.DOColor(_normalColor, 1f).From(_pressedColor).SetEase(Ease.OutCubic);
            }

        }
    }
}
