using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class DropDownFlags : MonoBehaviour, IPointerClickHandler, ICancelHandler
{
    public CanvasGroup _dropDownCanvasGroup;
    private GameObject _blocker;
    [SerializeField] private LocalFlagDb _localFlagDb;

    [SerializeField] private RectTransform _itemsContainer;
    private List<DropDownFlagsItem> _itemsUI = new List<DropDownFlagsItem>();
    [SerializeField] private GameObject _itemPrefab;

    [SerializeField] private Image _selectedFlagImage;
    

    private int _selectedFlagImageIndex = 0;

    IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;
         
        RectTransform itemSelectedRectTransform = _selectedFlagImage.GetComponent<RectTransform>();

        var options = new List<Dropdown.OptionData>();
        
        for (int index = 0; index < LocalizationSettings.AvailableLocales.Locales.Count; ++index)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[index];
            if (LocalizationSettings.SelectedLocale == locale)
                _selectedFlagImageIndex = index;
            GameObject item = Instantiate(_itemPrefab, _itemsContainer);

            Debug.Log(locale.name);

            DropDownFlagsItemData data = new DropDownFlagsItemData
            {
                sprite = _localFlagDb._flags[locale.name],
                name = locale.name
            };
            item.GetComponent<DropDownFlagsItem>().Init(data, index, LocalizationSettings.SelectedLocale == locale);
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            itemRectTransform.sizeDelta = itemSelectedRectTransform.sizeDelta;

            _itemsUI.Add(item.GetComponent<DropDownFlagsItem>());
        }

        
        _itemsContainer.sizeDelta = new Vector2(
            itemSelectedRectTransform.sizeDelta.x * LocalizationSettings.AvailableLocales.Locales.Count 
                + _itemsContainer.GetComponent<HorizontalLayoutGroup>().spacing * (LocalizationSettings.AvailableLocales.Locales.Count - 1),
            itemSelectedRectTransform.sizeDelta.y);

        //_selectedFlagImage.sprite = _items[_selectedFlagImageIndex].sprite;
    }

    public void OnCancel(BaseEventData eventData)
    {
        Hide();
    }

    protected virtual GameObject CreateBlocker(Canvas rootCanvas)
    {
        // Create blocker GameObject.
        GameObject blocker = new GameObject("Blocker");

        // Setup blocker RectTransform to cover entire root canvas area.
        RectTransform blockerRect = blocker.AddComponent<RectTransform>();
        blockerRect.SetParent(rootCanvas.transform, false);
        blockerRect.anchorMin = Vector3.zero;
        blockerRect.anchorMax = Vector3.one;
        blockerRect.sizeDelta = Vector2.zero;

        // Make blocker be in separate canvas in same layer as dropdown and in layer just below it.
        Canvas blockerCanvas = blocker.AddComponent<Canvas>();
        blockerCanvas.overrideSorting = true;
        Canvas dropdownCanvas = _dropDownCanvasGroup.GetComponent<Canvas>();
        blockerCanvas.sortingLayerID = dropdownCanvas.sortingLayerID;
        blockerCanvas.sortingOrder = dropdownCanvas.sortingOrder - 1;

        // Find the Canvas that this dropdown is a part of
        Canvas parentCanvas = null;
        Transform parentTransform = _dropDownCanvasGroup.transform.parent;
        while (parentTransform != null)
        {
            parentCanvas = parentTransform.GetComponent<Canvas>();
            if (parentCanvas != null)
                break;

            parentTransform = parentTransform.parent;
        }

        // If we have a parent canvas, apply the same raycasters as the parent for consistency.
        if (parentCanvas != null)
        {
            Component[] components = parentCanvas.GetComponents<BaseRaycaster>();
            for (int i = 0; i < components.Length; i++)
            {
                Type raycasterType = components[i].GetType();
                if (blocker.GetComponent(raycasterType) == null)
                {
                    blocker.AddComponent(raycasterType);
                }
            }
        }
        else
        {
            // Add raycaster since it's needed to block.
            GetOrAddComponent<GraphicRaycaster>(blocker);
        }


        // Add image since it's needed to block, but make it clear.
        Image blockerImage = blocker.AddComponent<Image>();
        blockerImage.color = Color.clear;

        // Add button since it's needed to block, and to close the dropdown when blocking area is clicked.
        Button blockerButton = blocker.AddComponent<Button>();
        blockerButton.onClick.AddListener(Hide);

        return blocker;
    }

    private static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T comp = go.GetComponent<T>();
        if (!comp)
            comp = go.AddComponent<T>();
        return comp;
    }

    public void Hide()
    {
        _dropDownCanvasGroup.DOFade(0, 0.3f);
        _dropDownCanvasGroup.blocksRaycasts = false;
        _dropDownCanvasGroup.interactable = false;
        _selectedFlagImage.DOFade(1f, 0.3f);

        if (_blocker != null)
            Destroy(_blocker);
    }

    public void show()
    {
        Canvas rootCanvas = GetComponentInParent<Canvas>();
        _blocker = CreateBlocker(rootCanvas);
        _dropDownCanvasGroup.DOFade(1, 0.3f);
        _dropDownCanvasGroup.blocksRaycasts = true;
        _dropDownCanvasGroup.interactable = true;
        _selectedFlagImage.DOFade(0.6f, 0.3f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        show();
    }


    public void Select(int index)
    {
        _itemsUI[_selectedFlagImageIndex].setOn(false);

        _selectedFlagImageIndex = index;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        //_selectedFlagImage.sprite = _items[index].sprite;

        Hide();
    }

}
