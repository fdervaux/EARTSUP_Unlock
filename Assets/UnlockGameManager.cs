using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class UnlockGameManager : MonoBehaviour
{
    public static UnlockGameManager Instance;

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

    public void OnCloseButton()
    {
        _popupMessageController.SetMessage(_quitGameString.GetLocalizedString());
        _popupMessageController.ShowMessage(true);
        _popupMessageController.ShowImage(false);
        _popupMessageController.ShowYesNoButtons(true);
        _popupMessageController.ShowHintButtons(false);

        _popupMessageController.OnYesNoValidate.AddListener((bool value) => {
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

        _popupMessageController.transform.GetComponent<PopupViewController>().openPopup();

    }


    public int GetPenaltyTimeInMinute()
    {
        return (int)_penaltyTime / 60;
    }


    private bool _isPlaying = false;

    private float _timeLeft = 0;

    public float TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = Mathf.Max(value, 0);
        }
    }


    public Hint getHint(string hintName)
    {
        Hint hint;
        _unlockGameData.Hints.TryGetValue(hintName, out hint);
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
            TimeLeft -= deltaTime;

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
        _playPauseButton.Activate();
        _penaltyButton.Deactivate();
        _clueButton.Deactivate();
        _codeButton.Deactivate();
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

        if (_isPlaying)
        {
            _playPauseButton.SwitchImage();
            _penaltyButton.Activate();
            _clueButton.Activate();
            _codeButton.Activate();
            _machineButton.Activate();
            GameManager.Instance.SoundManager.PlayMusic();
        }

        else
        {
            _playPauseButton.SwitchImage();
            _penaltyButton.Deactivate();
            _clueButton.Deactivate();
            _codeButton.Deactivate();
            _machineButton.Deactivate();
            GameManager.Instance.SoundManager.PauseMusic();
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime(Time.deltaTime);
    }
}
