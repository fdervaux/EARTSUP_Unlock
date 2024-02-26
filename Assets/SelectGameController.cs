using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectGameController : MonoBehaviour
{
    [SerializeField] private GameObject promotionItemPrefab;
    [SerializeField] private GameObject gameItemPrefab;
    [SerializeField] private PromotionsData promotionData;
    [SerializeField] private RectTransform promotionContainer;
    [SerializeField] private ScrollRect scrollRect;

    private PromotionItem _selectedPromotion = null;
    private List<PromotionItem> _promotionItems = new List<PromotionItem>();

    public void SelectPromotion(PromotionItem promotion)
    {
        bool isAnimating = false;
        foreach (var promotionItem in _promotionItems)
        {
            if (promotionItem.IsAnimating)
            {
                isAnimating = true;
                break;
            }
        }

        if (isAnimating)
        {
            return;
        }

        if (_selectedPromotion == promotion)
        {
            _selectedPromotion = null;
        }
        else
        {
            _selectedPromotion = promotion;

            var index = _promotionItems.IndexOf(_selectedPromotion);

            var normalizedPosition = 1 - (float)index / (_promotionItems.Count - 1);

            bool heightdiff = scrollRect.content.rect.height <= scrollRect.GetComponent<RectTransform>().rect.height;

            if (heightdiff && index == 0)
            {
                scrollRect.verticalNormalizedPosition = 1;
            }
            else
            {
                DOTween.To(
                    () => scrollRect.verticalNormalizedPosition,
                    x => scrollRect.verticalNormalizedPosition = x,
                    normalizedPosition,
                    0.5f).SetEase(Ease.InOutCubic);
            }

        }

        UpdateSelection();
    }

    public void UpdateSelection()
    {
        if (_selectedPromotion == null)
        {
            foreach (var promotionItem in _promotionItems)
            {
                promotionItem.Select(false);
            }
            return;
        }

        foreach (var promotionItem in _promotionItems)
        {
            if (promotionItem == _selectedPromotion)
            {
                promotionItem.Select(true);
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
                SelectPromotion(promotionItem);
            });

            foreach (var game in promotion.games)
            {
                var gameItem = Instantiate(gameItemPrefab, promotionItem.GamesContainer);
                LevelSelectorItem levelSelectorItem = gameItem.GetComponent<LevelSelectorItem>();
                levelSelectorItem.SetUnlockGame(game);
                levelSelectorItem.OnClick.AddListener(() =>
                {
                    Debug.Log("Game Selected: " + game.gameName);
                    //promotionItem.Unselect();
                });
            }

            _promotionItems.Add(promotionItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
