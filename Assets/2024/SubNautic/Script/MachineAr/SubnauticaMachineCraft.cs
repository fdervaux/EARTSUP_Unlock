using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Craft
{
    public string itemName;
    public Image itemImage;
    public Image targetAR;
    [Space]
    public string eventName;
}

public class SubnauticaMachineCraft : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPanel;
    [SerializeField] private GameObject _selectedCraftPanel;
    [Space]
    [SerializeField] private TextMeshProUGUI _textItemName;
    [SerializeField] private Image _selectedCraftItemImage;
    [SerializeField] private Image _selectedCraftTargetAR;
    private Craft _currentCraft;
    private Coroutine _valideCraft;

    void Start()
    {
        ResetUI();
    }

    public void OnRecipeFind()
    {
        if(_currentCraft != null)
            UnlockGameManager.Instance.TriggerEvent(_currentCraft.eventName);
    }

    public void SelectCraft(Craft craft)
    {
        _currentCraft = craft;

        _buttonPanel.SetActive(false);
        _textItemName.text = _currentCraft.itemName;
        _selectedCraftItemImage = _currentCraft.itemImage;
        _selectedCraftTargetAR = _currentCraft.targetAR;
        _selectedCraftPanel.SetActive(true);

        _valideCraft = StartCoroutine(DelayCraftValidation());
    }

    public void ResetUI()
    {
        if(_valideCraft != null)
            StopCoroutine(_valideCraft);

        _selectedCraftPanel.SetActive(false);
        _textItemName.text = "";
        _selectedCraftItemImage = null;
        _selectedCraftTargetAR = null;
        _buttonPanel.SetActive(true);
    }

    private IEnumerator DelayCraftValidation()
    {
        yield return new WaitForSeconds(2f);
        OnRecipeFind();
    }
}