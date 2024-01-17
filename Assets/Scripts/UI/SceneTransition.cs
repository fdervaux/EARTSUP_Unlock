using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private int _sceneIndex = 0;

    public void LoadScene()
    {
        GameManager.Instance.TransitionSceneManager.LoadScene(_sceneIndex);
    }
}
