using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class PopupMessageController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private GameObject _imageContainer;
    [SerializeField] private RectTransform _verticalLayout;
    [SerializeField] private GameObject _yesNoButtonsContainer;
    [SerializeField] private GameObject _hintButtonsContainer;

    [SerializeField] private GameObject _firstHintButton;
    [SerializeField] private GameObject _secondHintButton;
    [SerializeField] private GameObject _anwserButton;
    [SerializeField] private GameObject _messagePopup;

    [Header("Data")]

    private bool _showImage = true;
    private bool _showMessage = true;
    private bool _showHintButton = true;
    private bool _showYesNoButtons = true;

    private Hint _currentHint;
    private string _currentHintName;


    [SerializeField] public UnityEvent<bool> OnYesNoValidate;
    //[SerializeField] public UnityEvent<int> OnHintValidate;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Color _color;
    [SerializeField] private string _message;

    [SerializeField] private LocalizedString _hintConfirmString;
    [SerializeField] private LocalizedString _hintSolutionString;
    [SerializeField] private LocalizedString _hintTitleString;

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _normalColor;

    [SerializeField] private GameObject _CardToDiscardContainer;
    [SerializeField] private List<GameObject> _CardToDiscards;
    [SerializeField] private GameObject _CardToTakeContainer;
    [SerializeField] private List<GameObject> _CardToTakes;


    private Image _image;

    private PopupViewController _popupViewController;





    public string CurrentHintName
    {
        get => _currentHintName;
        set => _currentHintName = value;
    }

    public Hint CurrentHint
    {
        get => _currentHint;
        set => _currentHint = value;
    }

    public void OpenPopupMessage(float delay = 0f, Action action = null)
    {
        if (delay == 0f)
        {
            _popupViewController.openPopup();
            action?.Invoke();
            return;
        }

        DOTween.Sequence()
            .SetDelay(delay)
            .OnComplete(() =>
            {
                _popupViewController.openPopup();
                action?.Invoke();
            });
    }

    public void SetupPopupMessage(UnlockEvent unlockEvent)
    {
        string message = unlockEvent.localizedString.GetLocalizedString();
        SetMessage(message);
        ShowMessage(message != "");
        ShowImage(false);
        ShowYesNoButtons(false);
        ShowHintButtons(false, false, false);
        ShowCardToTake(new List<string>(unlockEvent._cardToTake));
        ShowDiscardCard(new List<string>(unlockEvent._cardToDiscard));
    }

    public void SetupPopupMessage(string message, bool showImage, bool showYesNoButtons, bool showHintButtons, bool showSecondHint = false, bool showAnwser = false)
    {
        SetMessage(message);
        ShowMessage(message != "");
        ShowImage(showImage);
        ShowYesNoButtons(showYesNoButtons);
        ShowHintButtons(showHintButtons, showSecondHint, showAnwser);
        ShowCardToTake();
        ShowDiscardCard();
    }

    public void ResetPopupMessage()
    {
        _messageText.gameObject.SetActive(false);
        _imageContainer.SetActive(false);
        _yesNoButtonsContainer.SetActive(false);
        _hintButtonsContainer.SetActive(false);
    }

    public void UpdateHintPopupMessage(Hint hint, string hintName, string message)
    {
        (_hintTitleString["hintNumber"] as IntVariable).Value = int.Parse(hintName);
        SetupPopupMessage(
            "<b>" + _hintTitleString.GetLocalizedString() + "</b>\n" + message,
            false,
            false,
            hint != null,
            hint == null ? false : hint.HasMoreHint,
            hint == null ? false : hint.HasSolution
        );

    }

    public void OnHint(int value)
    {
        if (value == 1)
        {
            UpdateHintPopupMessage(_currentHint, CurrentHintName, _currentHint.hintMessage1.GetLocalizedString());

            _firstHintButton.GetComponent<Image>().color = _selectedColor;
            _secondHintButton.GetComponent<Image>().color = _normalColor;
            _anwserButton.GetComponent<Image>().color = _normalColor;

            return;
        }

        if (value == 2)
        {
            PopupMessageController popupMessageController = _messagePopup.GetComponent<PopupMessageController>();
            popupMessageController.SetupPopupMessage(_hintConfirmString.GetLocalizedString(), false, true, false);

            popupMessageController.OnYesNoValidate.RemoveAllListeners();

            popupMessageController.OnYesNoValidate.AddListener((bool value) =>
            {
                popupMessageController.OnYesNoValidate.RemoveAllListeners();
                if (value)
                {
                    UpdateHintPopupMessage(_currentHint, _currentHintName, _currentHint.hintMessage2.GetLocalizedString());
                    _firstHintButton.GetComponent<Image>().color = _normalColor;
                    _secondHintButton.GetComponent<Image>().color = _selectedColor;
                    _anwserButton.GetComponent<Image>().color = _normalColor;
                }
                _messagePopup.GetComponent<PopupViewController>().closePopup();
            });

            _messagePopup.GetComponent<PopupViewController>().openPopup();
            return;

        }

        if (value == 3)
        {
            PopupMessageController popupMessageController = _messagePopup.GetComponent<PopupMessageController>();
            popupMessageController.SetupPopupMessage(_hintConfirmString.GetLocalizedString(), false, true, false);

            popupMessageController.OnYesNoValidate.RemoveAllListeners();

            popupMessageController.OnYesNoValidate.AddListener((bool value) =>
            {
                popupMessageController.OnYesNoValidate.RemoveAllListeners();
                if (value)
                {
                    UpdateHintPopupMessage(_currentHint, _currentHintName, _currentHint.hintMessageAnswer.GetLocalizedString());
                    _firstHintButton.GetComponent<Image>().color = _normalColor;
                    _secondHintButton.GetComponent<Image>().color = _normalColor;
                    _anwserButton.GetComponent<Image>().color = _selectedColor;
                }
                _messagePopup.GetComponent<PopupViewController>().closePopup();
            });

            _messagePopup.GetComponent<PopupViewController>().openPopup();
            return;
        }
    }

    public void OnYesOrNo(bool value)
    {
        OnYesNoValidate.Invoke(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        _image = _imageContainer.GetComponentInChildren<Image>();
        _image.sprite = _sprite;
        _image.color = _color;
        _messageText.text = _message;
        _imageContainer.SetActive(_showImage);

        _hintButtonsContainer.SetActive(_showHintButton);
        _yesNoButtonsContainer.SetActive(_showYesNoButtons);
        _messageText.gameObject.SetActive(_showMessage);

        _firstHintButton.GetComponent<Image>().color = _selectedColor;
        _secondHintButton.GetComponent<Image>().color = _normalColor;
        _anwserButton.GetComponent<Image>().color = _normalColor;

        _popupViewController = GetComponent<PopupViewController>();
        _popupViewController.OnPopupClose.AddListener(ResetPopupMessage);
    }

    public void ShowYesNoButtons(bool show)
    {
        _showYesNoButtons = show;
        _yesNoButtonsContainer.SetActive(show);
    }

    public void ShowDiscardCard(List<string> cards = null)
    {
        if(cards == null)
        {
            _CardToDiscardContainer.SetActive(false);
            return;
        }

        _CardToDiscardContainer.SetActive(cards.Count > 0);

        for (int i = 0; i < _CardToDiscards.Count; i++)
        {
            _CardToDiscards[i].SetActive(i < cards.Count);

            if (i >= cards.Count)
                continue;

            _CardToDiscards[i].GetComponentInChildren<TextMeshProUGUI>().text = cards[i];
        }
    }

    public void ShowCardToTake(List<string> cards = null)
    {
        if(cards == null)
        {
            _CardToTakeContainer.SetActive(false);
            return;
        }

        _CardToTakeContainer.SetActive(cards.Count > 0);

        for (int i = 0; i < _CardToTakes.Count; i++)
        {
            _CardToTakes[i].SetActive(i < cards.Count);

            if (i >= cards.Count)
                continue;

            _CardToTakes[i].GetComponentInChildren<TextMeshProUGUI>().text = cards[i];
        }
    }

    public void ShowHintButtons(bool show, bool showSecondHint, bool showAnwser)
    {
        _showHintButton = show;
        _hintButtonsContainer.SetActive(show);
        _secondHintButton.gameObject.SetActive(showSecondHint);
        _anwserButton.gameObject.SetActive(showAnwser);

        _anwserButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = !showSecondHint && showAnwser ? "2" : "3";
    }

    public void ShowImage(bool show)
    {
        _showImage = show;
        _imageContainer.SetActive(show);
    }

    public void SetImage(Sprite sprite, Color color)
    {
        _image.sprite = sprite;
        _image.color = color;

        _color = color;
        _sprite = sprite;
    }

    public void ShowMessage(bool show)
    {
        _showMessage = show;
        _messageText.gameObject.SetActive(show);
    }

    public void SetMessage(string message)
    {
        _messageText.text = message;
        _message = message;
    }
}
