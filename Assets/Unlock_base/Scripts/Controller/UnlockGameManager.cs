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

public struct HintSelectorItem
{
    public string name;
    public Hint hint;
    public HiddenObject hiddenObject;
}


[DefaultExecutionOrder(-1)]
public class UnlockGameManager : MonoBehaviour
{
    public static UnlockGameManager Instance;

    [SerializeField] private Canvas _mainMenu;

    [SerializeField] private ButtonController _playPauseButton;
    [SerializeField] private ButtonController _penaltyButton;
    [SerializeField] private ButtonController _clueButton;
    [SerializeField] private ButtonController _codeButton;
    [SerializeField] private ButtonController _hiddenObjectButton;
    [SerializeField] private ButtonController _machineButton;
    [SerializeField] private ButtonController _reviewClueButton;

    [Header("Timer")]

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


    private CanvasGroup _mainMenuCanvasGroup;
    private List<HintSelectorItem> _unlockedHints = new List<HintSelectorItem>();
    public List<HintSelectorItem> UnlockedHints => _unlockedHints;

    private float _timeScale = 1;

    public float TimeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = value;
            Time.timeScale = _timeScale;
        }
    }

    public float StartTimeTimer
    {
        get => _startTimeTimer;
        set
        {
            _startTimeTimer = value;
            SetTime(_startTimeTimer);
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


    [SerializeField] private UnityEvent<(string key,UnlockEvent UnityEvent)> _onUnlockEvent;
    public UnityEvent<(string key,UnlockEvent UnityEvent)> OnUnlockEvent { get => _onUnlockEvent; }


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


    public void TriggerEvent(string eventName)
    {
        UnlockEvent unlockEvent;
        _unlockGameData.UnlockEvents.TryGetValue(eventName, out unlockEvent);

        if (unlockEvent == null)
            return;

        _onUnlockEvent.Invoke((eventName,unlockEvent));
    
        if(unlockEvent._isPenaltyEvent)
        {
            (_penaltyString["minutes"] as IntVariable).Value = GetPenaltyTimeInMinute();
            _popupMessageController.SetupPopupMessage("<b> " + unlockEvent.localizedString.GetLocalizedString() + "</b>" + "\n" + _penaltyString.GetLocalizedString(), true, false, false);
            _popupMessageController.OpenPopupMessage();

            TriggerPenalty();
            return;
        }

        _popupMessageController.SetupPopupMessage(unlockEvent.localizedString.GetLocalizedString(), false, false, false);
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

        _popupMessageController.OnYesNoValidate.AddListener((bool value) =>
        {
            _popupMessageController.OnYesNoValidate.RemoveAllListeners();
            if (value)
            {
                GameManager.Instance.TransitionSceneManager.LoadScene(0);
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


    private bool _isPlaying = false;

    public bool IsPlaying
    {
        get => _isPlaying;
    }

    private float _timeLeft = 0;

    public float TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = Mathf.Max(value, 0);
        }
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

        HintSelectorItem unlockHint = _unlockedHints.Find((x) => x.name == hintName);

        if (hint != null && unlockHint.hint == null)
        {
            HintSelectorItem item = new HintSelectorItem
            {
                name = hintName,
                hint = hint,
                hiddenObject = null
            };

            _unlockedHints.Add(item);
            _hintSelectorController.SetupHints();
        }

        return hint;
    }

    private void SetTime(float time)
    {
        TimeLeft = time;
        UpdateTimerText();
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
            TimeLeft -= deltaTime * TimeScale;

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(TimeLeft / 60);
        int seconds = Mathf.FloorToInt(TimeLeft % 60);

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

        SetTime(_startTimeTimer);

        _timerText.color = _normalTextColor;

        GameManager.Instance.SoundManager.PlayMusic(_music);
        GameManager.Instance.SoundManager.PauseMusic();
    }

    void PenaltyButtonClick()
    {
        TriggerPenalty();
    }

    public void TriggerPenalty()
    {
        SetTime(TimeLeft - _penaltyTime);

        _timerText.DOColor(_normalTextColor, 2f).From(_penaltyTextColor).SetEase(Ease.OutCubic);
        _timerPanel.DOShakeAnchorPos(0.5f, 20, 100, 90, false, true).SetEase(Ease.OutCubic);

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
        if (_isPlaying)
        {
            _penaltyButton.Activate();
            _clueButton.Activate();
            _codeButton?.Activate();
            _machineButton.Activate();


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
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime(Time.deltaTime);
        UpdateButtonState();
    }
}
