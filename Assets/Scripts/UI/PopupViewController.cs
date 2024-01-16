using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupViewController : MonoBehaviour
{
    private CanvasGroup _canvasgroup;
    [SerializeField] private RectTransform _popupRectTransform;
    [SerializeField] private AnimationCurve _popupAnimationCurve;

    // Start is called before the first frame update
    void Start()
    {
        _canvasgroup = GetComponent<CanvasGroup>();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
