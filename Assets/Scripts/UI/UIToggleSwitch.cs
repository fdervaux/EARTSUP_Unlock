using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleSwitch : MonoBehaviour
{
    private Toggle _toggle;

    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    private void OnValidate()
    {
        if (_offSprite == null)
            return;

        if (_onSprite == null)
            return;

        _toggle = GetComponent<Toggle>();

        _toggle.image.sprite = _toggle.isOn ? _onSprite : _offSprite;
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(_toggle.isOn);
    }

    private void OnToggleValueChanged(bool state)
    {
        _toggle.image.sprite = state ? _onSprite : _offSprite;
    }
}
