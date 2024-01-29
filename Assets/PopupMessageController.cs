using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
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

    private Image _image;

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

    public void UpdateHintPopupMessage( Hint hint, string hintName, string message)
    {
        (_hintTitleString["hintNumber"] as IntVariable).Value = int.Parse(hintName);
        SetMessage("<b>" + _hintTitleString.GetLocalizedString() + "</b>\n" + message);
        ShowMessage(true);
        ShowImage(false);
        ShowYesNoButtons(false);

        if(hint == null)
        {
            ShowHintButtons(false);
            return;
        }

        ShowHintButtons(true, hint.HasMoreHint, hint.HasSolution);
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
            popupMessageController.SetMessage(_hintConfirmString.GetLocalizedString());
            popupMessageController.ShowMessage(true);
            popupMessageController.ShowImage(false);
            popupMessageController.ShowYesNoButtons(true);
            popupMessageController.ShowHintButtons(false);

            popupMessageController.OnYesNoValidate.AddListener((bool value) => {
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
            popupMessageController.SetMessage(_hintSolutionString.GetLocalizedString());
            popupMessageController.ShowMessage(true);
            popupMessageController.ShowImage(false);
            popupMessageController.ShowYesNoButtons(true);
            popupMessageController.ShowHintButtons(false);

            popupMessageController.OnYesNoValidate.AddListener((bool value) => {
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
    }

    public void ShowYesNoButtons(bool show)
    {
        _showYesNoButtons = show;
        _yesNoButtonsContainer.SetActive(show);
    }

    public void ShowHintButtons(bool show, bool showSecondHint = false, bool showAnwser = false)
    {
        _showHintButton = show;
        _hintButtonsContainer.SetActive(show);
        _secondHintButton.gameObject.SetActive(showSecondHint);
        _anwserButton.gameObject.SetActive(showAnwser);

        _anwserButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = !showSecondHint && showAnwser ? "2": "3";
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
