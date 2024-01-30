using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public float _TimerTime = 10; // 10 secondes
    public float _triggerTime = 0;
    public bool _isTimerRunning = false;

    // Start is called before the first frame update
    void Start()
    {

        _triggerTime = UnlockGameManager.Instance.TimeLeft - _TimerTime;
        _isTimerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(UnlockGameManager.Instance.TimeLeft < _triggerTime && _isTimerRunning)
        {
            UnlockGameManager.Instance.TriggerEvent("1"); 
            _isTimerRunning = false;
        }
        
    }
}
