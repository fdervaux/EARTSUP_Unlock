using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTower : MonoBehaviour
{
    [SerializeField] private GameObject _digicodeCanvas;
    [SerializeField] private TextMeshProUGUI _digicodeText;
    private string _finalAnswer = "1234";
    private string _answerPlayer = "";
    private int _digicodeCounter = 0, _doorDestroyingCounter = 0;

    private void Start(){
        _digicodeText.text = "";
    }

    public void HandleDoor(){
        //Popup, la porte est bloqué
        print("DoorIsLocked");
        UnlockGameManager.Instance.TriggerEvent("1");
    }
    public void DestroyDoor(){
        //Appuyer plusieurs fois sur la porte ce qui la fait exploser
        _doorDestroyingCounter++;
        //PlaySFX toc toc
        if (_doorDestroyingCounter >= 7){
            UnlockGameManager.Instance.TriggerEvent("2");
            Close();
        }
    }
    public void OpenDigicodeCanva()
    {
        print("OpenCanvasDigicode");
        _digicodeCanvas.SetActive(true);
    }
    public void CloseDigicodeCanva()
    {
        print("CloseCanvasDigicode");
        _digicodeCanvas.SetActive(false);
    }

    public void DigicodeButton(string numberDigicode)
    {
        _answerPlayer += numberDigicode;
        _digicodeText.text = _answerPlayer; 
        _digicodeCounter++;
        if (_digicodeCounter >= 4)
        {
            if (_answerPlayer == _finalAnswer){
                _digicodeCanvas.SetActive(false);
                UnlockGameManager.Instance.TriggerEvent("3");
                Close();
            }
            else{
                //Play SFX Wrong
            }
            _answerPlayer = "";
            _digicodeText.text = _answerPlayer;
            _digicodeCounter = 0;
        }
    }

    public void Close(){
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("DoomMenu"); });
    }
}
