using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftButton : MonoBehaviour
{
    [SerializeField] private SubnauticaMachineCraft _machineCraft;
    [SerializeField] private Craft _craft;

    [ContextMenu("Start fct")]
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = _craft.itemName;
    }

    public void OnClic()
    {
        _machineCraft.SelectCraft(_craft);
    }
}
