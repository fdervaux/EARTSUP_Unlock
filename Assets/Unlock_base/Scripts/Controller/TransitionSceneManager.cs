using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class TransitionSceneManager
{

    public GameObject _loadingScreen;

    private SliderController _slider;

    public float _minTime = 2f;
    private float _currentProgressTime = 0f;
    private CanvasGroup _canvasGroup;

    public void Init()
    {
        _slider = _loadingScreen.GetComponentInChildren<SliderController>();
        _canvasGroup = _loadingScreen.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;

        
    }

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Action OnPreviousSceneHidden = null)
    {
        InitLoader(OnPreviousSceneHidden);
        GameManager.Instance.StartCoroutine(loadAsync(sceneName, loadSceneMode));
    }

    public void LoadScene(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        InitLoader();
        GameManager.Instance.StartCoroutine(loadAsync(sceneIndex, loadSceneMode));
    }

    private void InitLoader(Action OnPreviousSceneHidden = null)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _slider.SetProgress(0);
        _canvasGroup.DOFade(1, 0.2f).SetEase(Ease.Linear).OnComplete(() => OnPreviousSceneHidden?.Invoke());
    }

    private IEnumerator loadAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        yield return new WaitForSeconds(0.3f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        asyncOperation.allowSceneActivation = false;
        yield return LoadAsync(asyncOperation);
    }

    private IEnumerator loadAsync(int sceneIndex, LoadSceneMode loadSceneMode)
    {
        yield return new WaitForSeconds(0.3f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        asyncOperation.allowSceneActivation = false;
        yield return LoadAsync(asyncOperation);
    }

    private IEnumerator  LoadAsync(AsyncOperation asyncOperation)
    {
        _currentProgressTime = _minTime;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        

        //When the load is still in progress, output the Text and progress bar
        while (_currentProgressTime > 0 || !asyncOperation.isDone)
        {
            //Output the current progress
            _slider.SetProgress(Mathf.Min(1 - _currentProgressTime / _minTime, asyncOperation.progress));

            _currentProgressTime -= Time.unscaledDeltaTime;

            if (_currentProgressTime > 0)
            {
                yield return null;
            }

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }


        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        _canvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear);
    }
}
