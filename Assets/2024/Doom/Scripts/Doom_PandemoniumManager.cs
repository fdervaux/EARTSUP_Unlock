using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Doom_PandemoniumManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabDemonList = new List<GameObject>();

    private void Awake(){
        int randomDemon = Random.Range(0,_prefabDemonList.Count);
        InstantiateDemon(randomDemon);
    }

    private void InstantiateDemon(int indexDemon){
        Instantiate(_prefabDemonList[indexDemon], this.transform.position, quaternion.identity, this.transform);
    }

    public void Close()
    {
        UnlockGameManager.Instance.TriggerEvent("99");
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("_Unlock_Doom_DoomMenu"); });
    }
}
