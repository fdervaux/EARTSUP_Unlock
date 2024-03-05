using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectorItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameText;
    private string _levelSceneName;
    private UnlockGame _game = null;

    private UnityEvent _onClick = new UnityEvent();
    public UnityEvent OnClick { get => _onClick;}

    public void SetLevelSceneName(string sceneName)
    {
        _levelSceneName = sceneName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!eventData.dragging)
            OnClick.Invoke();

        if(_levelSceneName == null || _levelSceneName == "")
            return;

        GameManager.Instance.TransitionSceneManager.LoadScene(_levelSceneName);
    }

    public void SetUnlockGame(UnlockGame game)
    {
        _game = game;
        _image.sprite = game.image;
        _nameText.text = game.gameName;
    }

}
