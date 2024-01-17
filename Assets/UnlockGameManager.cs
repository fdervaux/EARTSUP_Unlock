using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGameManager : MonoBehaviour
{
    [SerializeField] private ButtonController _playPauseButton;
    [SerializeField] private ButtonController _penaltyButton;
    [SerializeField] private ButtonController _clueButton;
    [SerializeField] private ButtonController _codeButton;
    [SerializeField] private ButtonController _hiddenObjectButton;
    [SerializeField] private ButtonController _machineButton;
    [SerializeField] private ButtonController _reviewClueButton;


    


    // Start is called before the first frame update
    void Start()
    {
        _playPauseButton.Activate();
        _penaltyButton.Deactivate();
        _clueButton.Deactivate();
        _codeButton.Deactivate();
        _hiddenObjectButton.Deactivate();
        _machineButton.Deactivate();
        _reviewClueButton.Deactivate();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
