using UnityEngine;
using UnityEngine.UI;

public class PopupOptionController : MonoBehaviour
{
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _chronometerToggle;
    [SerializeField] private Toggle _hiddenObjectsToggle;

    // Start is called before the first frame update
    void Start()
    {
        _musicToggle.isOn = GameManager.Instance.UserSettingsManager.IsMusucOn;
        _chronometerToggle.isOn = GameManager.Instance.UserSettingsManager.IsChronometerOn;
        _hiddenObjectsToggle.isOn = GameManager.Instance.UserSettingsManager.IsHiddenObjectsOn;

        _musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        _chronometerToggle.onValueChanged.AddListener(OnChronometerToggleChanged);
        _hiddenObjectsToggle.onValueChanged.AddListener(OnHiddenObjectsToggleChanged);
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        GameManager.Instance.UserSettingsManager.SetMusic(isOn);
    }

    public void OnChronometerToggleChanged(bool isOn)
    {
        GameManager.Instance.UserSettingsManager.SetChronometer(isOn);
    }

    public void OnHiddenObjectsToggleChanged(bool isOn)
    {
        GameManager.Instance.UserSettingsManager.SetHiddenObjects(isOn);
    }
}
