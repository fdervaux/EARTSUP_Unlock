using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.SocialPlatforms;


[DefaultExecutionOrder(-1)]
public class UnlockGameManager : MonoBehaviour
{

    [SerializeField] private Canvas _mainMenu;
    [SerializeField] private ButtonController _playPauseButton;
    [SerializeField] private ButtonController _penaltyButton;
    [SerializeField] private ButtonController _clueButton;
    [SerializeField] private ButtonController _codeButton;
    [SerializeField] private ButtonController _hiddenObjectButton;
    [SerializeField] private GameObject _hiddenObjectsAutoText;
    [SerializeField] private ButtonController _machineButton;
    [SerializeField] private ButtonController _reviewClueButton;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _noTimePanel;
    [SerializeField] private RectTransform _timerPanel; // 30 seconds
    [SerializeField] private float _startTimeTimer = 600; // 10 minutes
    [SerializeField] private float _penaltyTime = 60; // 60 seconds
    [SerializeField] private Color _penaltyTextColor = Color.red;
    [SerializeField] private Color _normalTextColor = Color.black;
    [SerializeField] private AudioClip _music;
    [SerializeField] private UnlockGameData _unlockGameData;
    [SerializeField] private PopupMessageController _popupMessageController;
    [SerializeField] private LocalizedString _quitGameString;
    [SerializeField] private HintSelectorController _hintSelectorController;
    [SerializeField] private LocalizedString _penaltyString;
    [SerializeField] private List<PopupViewController> _popupsViewController;
    [SerializeField] private UnityEvent<(string key, UnlockEvent UnityEvent)> _onUnlockEvent;

    [SerializeField] private ScoreViewController _scoreViewController;
 

    public static UnlockGameManager Instance;
    private CanvasGroup _mainMenuCanvasGroup;
    private List<UnlockSaveHintItem> _unlockedHints = new List<UnlockSaveHintItem>();
    public List<UnlockSaveHintItem> UnlockedHints => _unlockedHints;
    private List<(float timeStart, UnlockSaveHintItem hintSelector)> _beingUnlockHiddenObjects = new List<(float time, UnlockSaveHintItem)>();
    private List<UnlockSaveHintItem> _unlockedHiddenObjects = new List<UnlockSaveHintItem>();
    private List<UnlockSaveHintItem> _hiddenObjects = new List<UnlockSaveHintItem>();
    private Machine _currentMachine;
    private int _penaltyCount = 0;
    private float _timeScale = 1;
    private bool _isPlaying = false;
    private float _timeLeft = 0;
    private float _hintCount = 0;
    

    public UnityEvent<(string key, UnlockEvent UnityEvent)> OnUnlockEvent { get => _onUnlockEvent; }

    public float StartTimeTimer
    {
        get => _startTimeTimer;
    }

    public Machine CurrentMachine
    {
        get => _currentMachine;
        set => _currentMachine = value;
    }

    public bool IsPlaying
    {
        get => _isPlaying;
    }

    public int PenaltyCount
    {
        get => _penaltyCount;
    }

    public int HintCount
    {
        get => _unlockedHints.Count;
    }

    public float TimeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = value;
            //Time.timeScale = _timeScale;
        }
    }

    public float PenaltyTime
    {
        get => _penaltyTime;
        set
        {
            _penaltyTime = value;
        }
    }


    public void ShowMenu(Action onComplete = null)
    {
        _mainMenuCanvasGroup.DOFade(1, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
        _mainMenuCanvasGroup.blocksRaycasts = true;
        _mainMenuCanvasGroup.interactable = true;
    }

    public void HideMenu()
    {
        _mainMenuCanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Linear);
        _mainMenuCanvasGroup.blocksRaycasts = false;
        _mainMenuCanvasGroup.interactable = false;
    }

    public void ShowMenuInstant()
    {
        _mainMenuCanvasGroup.alpha = 1;
        _mainMenuCanvasGroup.blocksRaycasts = true;
        _mainMenuCanvasGroup.interactable = true;
    }

    public void HideMenuInstant()
    {
        _mainMenuCanvasGroup.alpha = 0;
        _mainMenuCanvasGroup.blocksRaycasts = false;
        _mainMenuCanvasGroup.interactable = false;
    }

    public void ManageHiddenObjectOnEvent(string EventIndex)
    {

        // add event to being unlock hidden objects if event is a preceding unlock event 
        for (int i = 0; i < _hiddenObjects.Count; i++)
        {
            HiddenObject hiddenObject = _hiddenObjects[i].unlockSaveHint as HiddenObject;
            if (hiddenObject.precedingUnlockEventID == EventIndex)
            {
                _beingUnlockHiddenObjects.Add((_timeLeft, _hiddenObjects[i]));
                Debug.Log("Add being unlock hidden object: " + _hiddenObjects[i].name);
                _hiddenObjects.RemoveAt(i);
                i--;
            }
        }

        if (EventIndex == "")
            return;


        // remove event from all hidden objects List if event is a canceling unlock event
        for (int i = 0; i < _unlockedHiddenObjects.Count; i++)
        {
            HiddenObject hiddenObject = _unlockedHiddenObjects[i].unlockSaveHint as HiddenObject;
            if (hiddenObject.cancelingUnlockEventID == EventIndex)
            {
                _unlockedHiddenObjects.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < _hiddenObjects.Count; i++)
        {
            HiddenObject hiddenObject = _hiddenObjects[i].unlockSaveHint as HiddenObject;
            if (hiddenObject.cancelingUnlockEventID == EventIndex)
            {
                _hiddenObjects.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < _beingUnlockHiddenObjects.Count; i++)
        {
            HiddenObject hiddenObject = _beingUnlockHiddenObjects[i].hintSelector.unlockSaveHint as HiddenObject;
            if (hiddenObject.cancelingUnlockEventID == EventIndex)
            {
                _beingUnlockHiddenObjects.RemoveAt(i);
                i--;
            }
        }
    }


    public void TriggerEvent(string EventIndex)
    {
        UnlockEvent unlockEvent;
        _unlockGameData.UnlockEvents.TryGetValue(EventIndex, out unlockEvent);

        if (unlockEvent == null)
            return;

        _onUnlockEvent.Invoke((EventIndex, unlockEvent));
        ManageHiddenObjectOnEvent(EventIndex);

        if (unlockEvent._isEndEvent)
        {
            _popupMessageController.SetupPopupMessage(unlockEvent.localizedString.GetLocalizedString(), false, false, false);
            _popupMessageController.OpenPopupMessage();

            TimeScale = 0;

            PopupViewController popupViewController = _popupMessageController.GetComponent<PopupViewController>();

            popupViewController.OnPopupClose.AddListener(() =>
            {
                GameManager.Instance.SoundManager.StopMusic();
                _scoreViewController.ShowView();

                popupViewController.OnPopupClose.RemoveAllListeners();
            });
            return;
        }

        if (unlockEvent._isPenaltyEvent)
        {
            (_penaltyString["minutes"] as IntVariable).Value = GetPenaltyTimeInMinute();


            _popupMessageController.SetupPopupMessage("<b> " + unlockEvent.localizedString.GetLocalizedString() + "</b>" + "\n" + _penaltyString.GetLocalizedString(), true, false, false);
            _popupMessageController.OpenPopupMessage();

            TriggerPenalty();
            return;
        }

        _popupMessageController.SetupPopupMessage(unlockEvent);
        _popupMessageController.OpenPopupMessage();

    }

    public void OpenPopupMessage(float delay = 0f, Action action = null)
    {
        if (delay == 0f)
        {
            _popupMessageController.transform.GetComponent<PopupViewController>().openPopup();
            return;
        }

        DOTween.Sequence()
            .SetDelay(delay)
            .OnComplete(() =>
            {
                _popupMessageController.transform.GetComponent<PopupViewController>().openPopup();
                action?.Invoke();
            });
    }



    public void OnCloseButton()
    {
        _popupMessageController.SetupPopupMessage(_quitGameString.GetLocalizedString(), false, true, false);

        _popupMessageController.OnYesNoValidate.RemoveAllListeners();

        _popupMessageController.OnYesNoValidate.AddListener((bool value) =>
        {
            _popupMessageController.OnYesNoValidate.RemoveAllListeners();
            if (value)
            {
                GameManager.Instance.TransitionSceneManager.LoadScene(1);
                GameManager.Instance.SoundManager.StopMusic();
            }
            else
            {
                _popupMessageController.transform.GetComponent<PopupViewController>().closePopup();
            }
        });

        OpenPopupMessage();
    }

    public int GetPenaltyTimeInMinute()
    {
        return (int)_penaltyTime / 60;
    }

    public Machine GetMachine(string machineName)
    {
        Machine machine;
        _unlockGameData.Machines.TryGetValue(machineName, out machine);

        return machine;
    }

    public Code GetCode(string codeName)
    {
        Code code;
        _unlockGameData.Codes.TryGetValue(codeName, out code);

        return code;
    }


    public Hint getHint(string hintName)
    {
        Hint hint;
        _unlockGameData.Hints.TryGetValue(hintName, out hint);

        if (hint == null)
            return null;

        UnlockSaveHintItem unlockHint = _unlockedHints.Find((x) => x.name == hintName);

        if (hint != null && unlockHint.unlockSaveHint == null)
        {
            UnlockSaveHintItem item = new UnlockSaveHintItem
            {
                name = hintName,
                unlockSaveHint = hint
            };

            _unlockedHints.Add(item);
            _hintSelectorController.SetupHints();
        }

        return hint;
    }

    public void SetTime(float time)
    {
        _timeLeft = time;
        UpdateTimerText();
    }

    public float GetTimeLeft()
    {
        return _timeLeft;
    }

    public float GetTime()
    {
        return _timeLeft - _penaltyCount * _penaltyTime;
    }

    private void UpdateTime(float deltaTime)
    {
        if (!GameManager.Instance.UserSettingsManager.IsChronometerOn)
        {
            _noTimePanel.SetActive(true);
            _timerText.gameObject.SetActive(false);
            return;
        }

        _noTimePanel.SetActive(false);
        _timerText.gameObject.SetActive(true);

        if (_isPlaying)
        {
            _timeLeft -= deltaTime * TimeScale;

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        float time = GetTime();

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        if (minutes <= 0 && seconds <= 0)
        {
            minutes *= -1;
            seconds *= -1;
            _timerText.color = _penaltyTextColor;
        }

        _timerText.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainMenuCanvasGroup = _mainMenu.GetComponent<CanvasGroup>();

        _playPauseButton.Activate();
        _penaltyButton.Deactivate();
        _clueButton.Deactivate();
        _codeButton?.Deactivate();
        _hiddenObjectButton.Deactivate();
        _machineButton.Deactivate();
        _reviewClueButton.Deactivate();

        _playPauseButton.OnClick.AddListener(PlayPauseButtonClick);
        _penaltyButton.OnClick.AddListener(PenaltyButtonClick);
        _hiddenObjectButton.OnClick.AddListener(HiddenObjectButtonClick);

        SetTime(_startTimeTimer);

        _timerText.color = _normalTextColor;

        GameManager.Instance.SoundManager.PlayMusic(_music);
        GameManager.Instance.SoundManager.PauseMusic();

        foreach (var hiddenObject in _unlockGameData.HiddenObjects)
        {
            UnlockSaveHintItem itemSelectorItem = new UnlockSaveHintItem
            {
                name = hiddenObject.Key,
                unlockSaveHint = hiddenObject.Value
            };
            Debug.Log("add hidden object: " + hiddenObject.Key + " " + hiddenObject.Value);

            _hiddenObjects.Add(itemSelectorItem);
        }

        ManageHiddenObjectOnEvent("");
    }

    public void HiddenObjectButtonClick()
    {
        if (_unlockedHiddenObjects.Count > 0)
        {
            TriggerHiddenObject(_unlockedHiddenObjects[0]);
            _unlockedHiddenObjects.RemoveAt(0);
        }
    }

    void PenaltyButtonClick()
    {
        TriggerPenalty();
    }

    public void TriggerPenalty()
    {
        _penaltyCount++;

        float time = GetTime();

        if (time > 0)
            _timerText.DOColor(_normalTextColor, 1f).From(_penaltyTextColor).SetEase(Ease.OutCubic);

#if UNITY_ANDROID
        Vibration.VibrateAndroid(500);
#endif

        _timerPanel.DOShakeAnchorPos(0.5f, 20, 100, 90, false, true).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _timerPanel.anchoredPosition = Vector2.zero;
        });

        GameManager.Instance.SoundManager.playPenalty();
    }

    private void PlayPauseButtonClick()
    {
        _isPlaying = !_isPlaying;
        _playPauseButton.SwitchImage();

        if (_isPlaying)
        {
            GameManager.Instance.SoundManager.PlayMusic();
        }
        else
        {
            GameManager.Instance.SoundManager.PauseMusic();
        }
    }

    public void UpdateButtonState()
    {
        bool isHiddenObjectsAutoOn = GameManager.Instance.UserSettingsManager.IsHiddenObjectsAutoOn;

        if (_hiddenObjectsAutoText.activeSelf != isHiddenObjectsAutoOn)
            _hiddenObjectsAutoText.SetActive(isHiddenObjectsAutoOn);

        if (_isPlaying)
        {
            _penaltyButton.Activate();
            _clueButton.Activate();
            _codeButton?.Activate();
            _machineButton.Activate();

            if (!GameManager.Instance.UserSettingsManager.IsHiddenObjectsAutoOn &&
               _unlockedHiddenObjects.Count > 0)
            {
                _hiddenObjectButton.Activate();
            }
            else
            {
                _hiddenObjectButton.Deactivate();
            }


            if (_unlockedHints.Count > 0)
            {
                _reviewClueButton.Activate();
            }
            else
            {
                _reviewClueButton.Deactivate();
            }
        }

        else
        {
            _penaltyButton.Deactivate();
            _clueButton.Deactivate();
            _codeButton?.Deactivate();
            _machineButton.Deactivate();
            _reviewClueButton.Deactivate();
            _hiddenObjectButton.Deactivate();
        }
    }

    public void Back()
    {
        foreach (PopupViewController popupViewController in _popupsViewController)
        {
            if (popupViewController.IsOpened)
            {
                popupViewController.closePopup();
                return;
            }
        }

        OnCloseButton();
    }

    private void ManageBeingUnlockHiddenObjects()
    {
        for (int i = 0; i < _beingUnlockHiddenObjects.Count; i++)
        {
            UnlockSaveHintItem hintSelectorItem = _beingUnlockHiddenObjects[i].hintSelector;
            HiddenObject hiddenObject = hintSelectorItem.unlockSaveHint as HiddenObject;
            float timeStart = _beingUnlockHiddenObjects[i].timeStart;

            if (_timeLeft <= timeStart - hiddenObject.Time)
            {
                _unlockedHiddenObjects.Add(hintSelectorItem);
                _beingUnlockHiddenObjects.RemoveAt(i);
                i--;
            }
        }
    }

    private void ManageUnlockedHiddenObjects()
    {
        if (GameManager.Instance.UserSettingsManager.IsHiddenObjectsAutoOn == false)
            return;

        if (CurrentMachine != null)
            return;

        foreach (PopupViewController popupViewController in _popupsViewController)
        {
            if (popupViewController.IsOpened)
                return;
        }

        for (int i = 0; i < _unlockedHiddenObjects.Count; i++)
        {
            UnlockSaveHintItem hintSelectorItem = _unlockedHiddenObjects[i];

            TriggerHiddenObject(hintSelectorItem);
            _unlockedHiddenObjects.RemoveAt(i);
            i--;
        }
    }

    private void TriggerHiddenObject(UnlockSaveHintItem hintSelectorItem)
    {
        _unlockedHints.Add(hintSelectorItem);
        _hintSelectorController.SetupHints();
        HiddenObject hiddenObject = hintSelectorItem.unlockSaveHint as HiddenObject;
        _popupMessageController.SetupPopupMessage(hiddenObject.message.GetLocalizedString(), false, false, false);
        _popupMessageController.OpenPopupMessage();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime(Time.deltaTime);
        ManageBeingUnlockHiddenObjects();
        ManageUnlockedHiddenObjects();
        UpdateButtonState();
    }
}
