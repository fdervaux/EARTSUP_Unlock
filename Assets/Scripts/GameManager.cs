using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class UserSettingsManager
{
    private bool _isMusucOn = true;
    private bool _isChronometerOn = true;
    private bool _isHiddenObjectsOn = true;

    public bool IsMusucOn { get => _isMusucOn; }
    public bool IsChronometerOn { get => _isChronometerOn; }
    public bool IsHiddenObjectsOn { get => _isHiddenObjectsOn; }

    public bool SetMusic(bool isOn)
    {
        _isMusucOn = isOn;
        PlayerPrefs.SetInt("Music", isOn ? 1 : 0);
        return _isMusucOn;
    }

    public bool SetChronometer(bool isOn)
    {
        _isChronometerOn = isOn;
        PlayerPrefs.SetInt("Chronometer", isOn ? 1 : 0);
        return _isChronometerOn;
    }

    public bool SetHiddenObjects(bool isOn)
    {
        _isHiddenObjectsOn = isOn;
        PlayerPrefs.SetInt("HiddenObjects", isOn ? 1 : 0);
        return _isHiddenObjectsOn;
    }

    public void Load()
    {
        _isMusucOn = PlayerPrefs.GetInt("Music", 1) == 1;
        _isChronometerOn = PlayerPrefs.GetInt("Chronometer", 1) == 1;
        _isHiddenObjectsOn = PlayerPrefs.GetInt("HiddenObjects", 1) == 1;
    }

}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UserSettingsManager _userSettingsManager;
    [SerializeField] private TransitionSceneManager _transitionSceneManager;

    public UserSettingsManager UserSettingsManager { get => _userSettingsManager; }
    public TransitionSceneManager TransitionSceneManager { get => _transitionSceneManager; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        _userSettingsManager.Load();
        _transitionSceneManager.Init();

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}