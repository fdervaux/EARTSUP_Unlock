using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PromotionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private UnityEngine.UI.Image image;

    [SerializeField] private RectTransform gamesContainer;
    [SerializeField] private RectTransform gamesContainerHeight;

    public RectTransform GamesContainer { get => gamesContainer; }

    private HSV_UI hsv_ui;


    private UnityEvent _onClick = new UnityEvent();
    private Promotion promotion;

    public UnityEvent OnClick { get => _onClick; }

    private bool _isAnimating = false;

    public bool IsAnimating { get => _isAnimating; }

    public bool IsActivated { get => promotion.activated; }


    public void Select(bool showGames = true)
    {
        if(!promotion.activated)
            return;

        DOTween.To(() => hsv_ui.Saturation, x => hsv_ui.Saturation = x, 0f, 0.5f);
        if (showGames)
        {
            gamesContainerHeight.gameObject.SetActive(true);
            gamesContainerHeight.sizeDelta = new Vector2(gamesContainerHeight.sizeDelta.x, 0);
            _isAnimating = true;
            DOTween.Complete(gamesContainerHeight);
            gamesContainerHeight.DOSizeDelta(new Vector2(gamesContainerHeight.sizeDelta.x, 400), 0.5f).OnComplete(
                () => _isAnimating = false).SetEase(Ease.InOutCubic);
        }
        else
        {
            _isAnimating = true;
            DOTween.Complete(gamesContainerHeight);
            gamesContainerHeight.DOSizeDelta(new Vector2(gamesContainerHeight.sizeDelta.x, 0), 0.5f).OnComplete(
                () => {
                    gamesContainerHeight.gameObject.SetActive(false);
                    _isAnimating = false;
                    }).SetEase(Ease.InOutCubic);
        }
    }

    public void Unselect()
    {
        if(!promotion.activated)
            return;

        _isAnimating = true;

        DOTween.To(() => hsv_ui.Saturation, x => hsv_ui.Saturation = x, -1f, 0.5f);

        _isAnimating = true;
        DOTween.Complete(gamesContainerHeight);
        gamesContainerHeight.DOSizeDelta(new Vector2(gamesContainerHeight.sizeDelta.x, 0), 0.5f).OnComplete(
                () => {
                    gamesContainerHeight.gameObject.SetActive(false);
                    _isAnimating = false;
                    }).SetEase(Ease.InOutCubic);
        
    }

    private void Start()
    {
        hsv_ui = image.GetComponent<HSV_UI>();

        if(!promotion.activated)
        {
            hsv_ui.Saturation = -1f;
        }
    }

    public void SetPromotion(Promotion promotion)
    {
        title.text = promotion.PromotionName;
        image.sprite = promotion.image;
        this.promotion = promotion;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isAnimating)
            return;

        if(!promotion.activated)
            return;

        DOTween.To(() => hsv_ui.Value, x => hsv_ui.Value = x, -0.02f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_isAnimating)
            return;
        
        if(!promotion.activated)
            return;

        DOTween.To(() => hsv_ui.Value, x => hsv_ui.Value = x, 0f, 0.1f);
        if (!eventData.dragging)
            _onClick.Invoke();
    }
}
