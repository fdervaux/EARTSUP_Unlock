using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UnlockGameManager : MonoBehaviour
{
    [SerializeField] private ButtonController _playPauseButton;
    [SerializeField] private ButtonController _penaltyButton;
    [SerializeField] private ButtonController _clueButton;
    [SerializeField] private ButtonController _codeButton;
    [SerializeField] private ButtonController _hiddenObjectButton;
    [SerializeField] private ButtonController _machineButton;
    [SerializeField] private ButtonController _reviewClueButton;

    [Header("Timer")]

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private RectTransform _timerPanel; // 30 seconds
    [SerializeField] private float _startTimeTimer = 600; // 10 minutes
    [SerializeField] private float _penaltyTime = 60; // 60 seconds
    [SerializeField] private Color _penaltyTextColor = Color.red;
    [SerializeField] private Color _normalTextColor = Color.black;



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

    private void SetTime(float time)
    {
        TimeLeft = time;
        UpdateTimerText();
    }


    private void UpdateTime(float deltaTime)
    {
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
    }

    void PenaltyButtonClick()
    {
        SetTime(TimeLeft - _penaltyTime);

        _timerText.DOColor(_normalTextColor, 2f).From(_penaltyTextColor).SetEase(Ease.OutCubic);
        _timerPanel.DOShakeAnchorPos(0.5f, 20, 100, 90, false, true).SetEase(Ease.OutCubic);
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
        }

        else
        {
            _playPauseButton.SwitchImage();
            _penaltyButton.Deactivate();
            _clueButton.Deactivate();
            _codeButton.Deactivate();
            _machineButton.Deactivate();
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime(Time.deltaTime);
    }
}
