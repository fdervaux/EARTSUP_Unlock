using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryCondition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("SubnauticaVibrateMachine"); });
            UnlockGameManager.Instance.TriggerEvent("1_SubnauticaEndMachineSonar");
        }
    }
}
