using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MachineController : MonoBehaviour
{
    [SerializeField] private GameObject _mainCanvas;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;


    public void OnPenaltyButton()
    {
        UnlockGameManager.Instance.TriggerPenalty();
    }

    public void OnValidateButton()
    {
        Close();   
        UnlockGameManager.Instance.TriggerEvent("1");
    }

    public void OnCloseButton()
    {
        Close();
    }

    private void Close()
    {
        _rectTransform.DOAnchorPos(new Vector2(_rectTransform.rect.width, 0), 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            SceneManager.UnloadSceneAsync("MachineTuto");
        });
    }


    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = _mainCanvas.GetComponent<CanvasGroup>();
        _rectTransform = _mainCanvas.GetComponent<RectTransform>();

        _canvasGroup.alpha = 1;

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        _rectTransform.anchoredPosition = new Vector2(_rectTransform.rect.width, 0);

        _rectTransform.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.InBack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
