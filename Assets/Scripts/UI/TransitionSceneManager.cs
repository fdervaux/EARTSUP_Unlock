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

    public void LoadScene(int sceneIndex)
    {
        //Start loading the Scene asynchronously and output the progress bar


        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _slider.SetProgress(0);
        _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.Linear);

        GameManager.Instance.StartCoroutine(LoadSceneAsync(sceneIndex));
    }




    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return new WaitForSeconds(0.3f);


        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        _currentProgressTime = _minTime;

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
