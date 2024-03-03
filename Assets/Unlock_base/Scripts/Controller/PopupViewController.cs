using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PopupViewController : MonoBehaviour
{
    private CanvasGroup _canvasgroup;
    [SerializeField] private RectTransform _popupRectTransform;
    [SerializeField] private UnityEvent _OnPopupOpen;
    [SerializeField] private UnityEvent _OnPopupClose;

    public UnityEvent OnPopupOpen => _OnPopupOpen;
    public UnityEvent OnPopupClose => _OnPopupClose;

    private bool _isOpened = false;

    public bool IsOpened { get => _isOpened; }


    [SerializeField, Range(0, 1)] private float _animationTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _canvasgroup = GetComponent<CanvasGroup>();
        _canvasgroup.alpha = 0;
    }

    public void openPopup()
    {
        _canvasgroup.DOFade(1, _animationTime/2).SetEase(Ease.OutCubic);

        _popupRectTransform
            .DOScale(1, _animationTime)
            .SetDelay(_animationTime/2)
            .From(0)
            .SetEase(Ease.OutBack);

        _canvasgroup.interactable = true;
        _canvasgroup.blocksRaycasts = true;

        _OnPopupOpen.Invoke();

        _isOpened = true;
    }

    public void closePopupImmediate()
    {

        
        _canvasgroup.alpha = 0;
        _popupRectTransform.localScale = Vector3.zero;
        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;

        _OnPopupClose.Invoke();

        _isOpened = false;
    }

    public void closePopup()
    {
        _canvasgroup.DOFade(0, _animationTime/2).SetEase(Ease.OutCubic).SetDelay(_animationTime);

        _popupRectTransform
            .DOScale(0, _animationTime)
            .From(1)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                _OnPopupClose.Invoke();
            });

        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;

        _isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
