using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CraftButton : MonoBehaviour
{
    [SerializeField] private SubnauticaMachineCraft _machineCraft;
    [SerializeField] private Craft _craft;
    [SerializeField] private string _caftIndex;

    [ContextMenu("Start fct")]
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = _craft.itemName;
    }

    public void OnClic()
    {
        // _machineCraft.SelectCraft(_craft);
        if(_caftIndex == "")
            return;
        UnlockGameManager.Instance.TriggerEvent(_caftIndex);
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("Subnautica_Labyrinth"); });
    }
}
