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
        GameManager.Instance.SoundManager.SetMusicMute(!_isMusucOn);
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
        SetMusic(PlayerPrefs.GetInt("Music", 1) == 1);

        _isChronometerOn = PlayerPrefs.GetInt("Chronometer", 1) == 1;
        _isHiddenObjectsOn = PlayerPrefs.GetInt("HiddenObjects", 1) == 1;
    }

}

[Serializable]
public class SoundManager
{

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    public AudioSource MusicSource { get => _musicSource; }
    public AudioSource SfxSource { get => _sfxSource; }

    [Header("SFX sounds")]
    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _penalty;

    public void playButtonClick()
    {
        _sfxSource.PlayOneShot(_buttonClick);
    }

    public void playPenalty()
    {
        _sfxSource.PlayOneShot(_penalty);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        _musicSource.Pause();
    }

    public void PlayMusic()
    {
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void StopSfx()
    {
        _sfxSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = volume;
    }

    public void SetMusicMute(bool isMute)
    {
        _musicSource.mute = isMute;
    }

    public void SetSfxMute(bool isMute)
    {
        _sfxSource.mute = isMute;
    }

}

[DefaultExecutionOrder(-2)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UserSettingsManager _userSettingsManager;
    [SerializeField] private TransitionSceneManager _transitionSceneManager;
    [SerializeField] private SoundManager _soundManager;

    public UserSettingsManager UserSettingsManager { get => _userSettingsManager; }
    public TransitionSceneManager TransitionSceneManager { get => _transitionSceneManager; }
    public SoundManager SoundManager { get => _soundManager; }

    private void Start()
    {
        _userSettingsManager.Load();
        _transitionSceneManager.Init();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null && Instance != this)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}