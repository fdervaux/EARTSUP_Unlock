using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class CancelHandler : MonoBehaviour
{
    private InputSystemUIInputModule _inputSystemUIInputModule;

    [SerializeField] private UnityEvent _OnCancel;

    public void OnEnable()
    {
        _inputSystemUIInputModule = GameManager.Instance.GetComponentInChildren<InputSystemUIInputModule>();
        _inputSystemUIInputModule.cancel.action.performed += _ => Cancel();
    }

    public void OnDisable()
    {
        _inputSystemUIInputModule.cancel.action.performed -= _ => Cancel();
    }

    public void Cancel()
    {
        _OnCancel.Invoke();
    }

    
}
