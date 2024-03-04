using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScannerEvent : MonoBehaviour
{

    public string TriggerEvent;

    void Start()
    {
        StartCoroutine(ScannerWaiting());
    }

    IEnumerator ScannerWaiting()
    {
        yield return new WaitForSeconds(2);
        UnlockGameManager.Instance.TriggerEvent("EVENT_NAME");
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("MachineScanner"); });
    }
}

