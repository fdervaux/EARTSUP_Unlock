using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameController : MonoBehaviour
{
    [SerializeField] private GameObject promotionItemPrefab;
    [SerializeField] private PromotionsData promotionData;
    [SerializeField] private RectTransform promotionContainer;

    private PromotionItem _selectedPromotion = null;
    private List<PromotionItem> _promotionItems = new List<PromotionItem>();

    public void SelectPromotion(PromotionItem promotion)
    {
        if (_selectedPromotion == promotion)
        {
            _selectedPromotion = null;
        }
        else
        {
            _selectedPromotion = promotion;
        }

        UpdateSelection();
    }

    public void UpdateSelection()
    {
        if (_selectedPromotion == null)
        {
            foreach (var promotionItem in _promotionItems)
            {
                promotionItem.Select();
            }
            return;
        }

        foreach (var promotionItem in _promotionItems)
        {
            if (promotionItem == _selectedPromotion)
            {
                promotionItem.Select();
            }
            else
            {
                promotionItem.Unselect();
            }
        }
    }
    


    // Start is called before the first frame update
    void Start()
    {
        _promotionItems.Clear();

        foreach (var promotion in promotionData.promotion)
        {
            var item = Instantiate(promotionItemPrefab, promotionContainer);
            PromotionItem promotionItem = item.GetComponent<PromotionItem>();
            promotionItem.SetPromotion(promotion);
            promotionItem.OnClick.AddListener(() =>
            {
                Debug.Log("Promotion selected: " + promotionItem.name);
                SelectPromotion(promotionItem);
            });

            _promotionItems.Add(promotionItem);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
