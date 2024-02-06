using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Doom_PandemoniumMenu : MonoBehaviour
{
    [SerializeField] private ButtonController _pandemoniumButton;
    [SerializeField] private bool _isTimerLaunched, _isPandemoniumActivated;
    [SerializeField] private int _doorTowerTimer = 60, _doorLaboTimer = 60, _timerPandemoniumActive = 60;
    [SerializeField] private float _timeScaleDifficulty = 1.5f;
    private float _timer, _timerActive;
    void Start(){
        UnlockGameManager.Instance.OnUnlockEvent.AddListener(OnUnlockEvent);
    }
    public void OnUnlockEvent((string key, UnlockEvent UnityEvent) unlockEnventEntry){
        if (unlockEnventEntry.key == "2" || unlockEnventEntry.key == "3")
        {
            _isTimerLaunched = true;
            _timer = _doorTowerTimer;
            _timerActive = _timerPandemoniumActive;
        }

        if (unlockEnventEntry.key == "99")
        {
            PandemoniumDeactivate();
        }
    }
    
    private void PandemoniumActivate()
    {
        if (_isPandemoniumActivated) return;
        UnlockGameManager.Instance.TriggerEvent("98");
        _pandemoniumButton.Activate();
        UnlockGameManager.Instance.TimeScale = _timeScaleDifficulty;
        _isPandemoniumActivated = true;
    }
    private void PandemoniumDeactivate()
    {
        _isTimerLaunched = false;
        _pandemoniumButton.Deactivate();
        UnlockGameManager.Instance.TimeScale = 1;
    }
    
    
    void Update()
    {
        if (_isTimerLaunched)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timerActive -= Time.deltaTime;
                PandemoniumActivate();
                if (_timerActive <= 0)
                {
                    PandemoniumDeactivate();
                }
            }
        }
    }
}
