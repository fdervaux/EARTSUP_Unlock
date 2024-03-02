using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTimeText;
    [SerializeField] private TextMeshProUGUI _penaltyNumberText;
    [SerializeField] private TextMeshProUGUI _hintNumberText;

    private CanvasGroup _canvasGroup;

    [SerializeField] private List<Image> _stars = new List<Image>();

    [SerializeField] private Sprite _spriteStarEmpty;
    [SerializeField] private Sprite _spriteStarFull;
    [SerializeField] private Sprite _spriteStarHalf;

    [Header("Score computation")]

    [SerializeField] private float _scoreTimeCoefficient = 2f;
    [SerializeField] private float _penaltyCoefficient = 1f;
    [SerializeField] private float _hintCoefficient = 1f;

    [SerializeField] private float _maxPenalty = 5;
    [SerializeField] private float _maxHint = 5;
    [SerializeField] private float _timeSuccesFactor = 0.7f;



    private float _normalzedScore;

    public void ShowView()
    {
        Debug.Log("ShowView");
        UpdateView();
        ComputeScore();
        UpdateStarView();
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        _canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear).From(0);
    }

    public void HideView()
    {
        _canvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear).From(1);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private float remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void ComputeScore()
    {
        float totalTime = UnlockGameManager.Instance.StartTimeTimer - UnlockGameManager.Instance.GetTime();
        int penaltyCount = UnlockGameManager.Instance.PenaltyCount;
        int hintCount = UnlockGameManager.Instance.HintCount;

        float startTime = UnlockGameManager.Instance.StartTimeTimer;

        float timescore = Mathf.Clamp01(remap(totalTime, _timeSuccesFactor * startTime, startTime, 1, 0));
        float penaltyScore = Mathf.Clamp01(remap(penaltyCount, 0, _maxPenalty, 1, 0));
        float hintScore = Mathf.Clamp01(remap(hintCount, 0, _maxHint, 1, 0));

        float score = _scoreTimeCoefficient * timescore + _penaltyCoefficient * penaltyScore + _hintCoefficient * hintScore;
        _normalzedScore = score / (_scoreTimeCoefficient + _penaltyCoefficient + _hintCoefficient);
    }

    private void UpdateStarView()
    {
        for (int i = 0; i < _stars.Count; i++)
        {
            float starScore = _normalzedScore * 5f - i;
            if (starScore >= 1)
            {
                _stars[i].sprite = _spriteStarFull;
            }
            else if (starScore >= 0.5f)
            {
                _stars[i].sprite = _spriteStarHalf;
            }
            else
            {
                _stars[i].sprite = _spriteStarEmpty;
            }
        }
    }

    private void UpdateView()
    {
        float time = UnlockGameManager.Instance.GetTime();

        float totalTime = UnlockGameManager.Instance.StartTimeTimer - time;

        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);

        _scoreTimeText.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));

        _penaltyNumberText.text = UnlockGameManager.Instance.PenaltyCount.ToString();
        _hintNumberText.text = UnlockGameManager.Instance.HintCount.ToString();
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

}
