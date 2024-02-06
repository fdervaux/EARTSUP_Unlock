using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PromotionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private UnityEngine.UI.Image image;

    private HSV_UI hsv_ui;


    private UnityEvent _onClick = new UnityEvent();
    private Promotion promotion;

    public UnityEvent OnClick { get => _onClick;}

    public void Select()
    {
        DOTween.To(() => hsv_ui.Saturation, x => hsv_ui.Saturation = x, 0f, 0.5f);
    }

    public void Unselect()
    {
        DOTween.To(() => hsv_ui.Saturation, x => hsv_ui.Saturation = x, -1f, 0.5f);
    }

    private void Start()
    {
        hsv_ui = image.GetComponent<HSV_UI>();
    }

    public void SetPromotion(Promotion promotion)
    {
        title.text = promotion.PromotionName;
        image.sprite = promotion.image;
        this.promotion = promotion;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.To(() => hsv_ui.Value, x => hsv_ui.Value = x, -0.02f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        DOTween.To(() => hsv_ui.Value, x => hsv_ui.Value = x, 0f, 0.1f);
        _onClick.Invoke();
    }
}
