using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Doom_PandemoniumMenu : MonoBehaviour
{
    void Start(){
        UnlockGameManager.Instance.OnUnlockEvent.AddListener(OnUnlockEvent);
    }

    public void OnUnlockEvent((string key, UnlockEvent UnityEvent) unlockEnventEntry){
        //Verif After Event
        print(unlockEnventEntry.key);
        
    }
}
