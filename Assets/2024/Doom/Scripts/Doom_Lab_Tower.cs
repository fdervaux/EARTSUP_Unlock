using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doom_Lab_Tower : MonoBehaviour
{
    [SerializeField] private GameObject _digicodeCanvas;
    [SerializeField] private TextMeshProUGUI _digicodeText;
    private string _finalAnswer = "8145";
    private string _answerPlayer = "";
    private int _digicodeCounter = 0;

    private void Start(){
        _digicodeText.text = "";
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
                Debug.Log("Code correctement reconnu !");
                UnlockGameManager.Instance.TriggerEvent("6");
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
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("_Unlock_Doom_Salletentacules"); });
    }
}
