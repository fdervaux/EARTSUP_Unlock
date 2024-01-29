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
 
    // Start is called before the first frame update
    void Start()
    {
        _canvasgroup = GetComponent<CanvasGroup>();
        _canvasgroup.alpha = 0;
    }

    public void openPopup()
    {
        _canvasgroup.DOFade(1, 0.2f).SetEase(Ease.OutCubic);

        _popupRectTransform
            .DOScale(1, 0.5f)
            .SetDelay(0.2f)
            .From(0)
            .SetEase(Ease.OutBack);

        _canvasgroup.interactable = true;
        _canvasgroup.blocksRaycasts = true;

        _OnPopupOpen.Invoke();
    }

    public void closePopupImmediate()
    {
        _canvasgroup.alpha = 0;
        _popupRectTransform.localScale = Vector3.zero;
        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;

        _OnPopupClose.Invoke();
    }

    public void closePopup()
    {
        _canvasgroup.DOFade(0, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.5f);

        _popupRectTransform
            .DOScale(0, 0.5f)
            .From(1)
            .SetEase(Ease.InBack);

        _canvasgroup.interactable = false;
        _canvasgroup.blocksRaycasts = false;

        _OnPopupClose.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
