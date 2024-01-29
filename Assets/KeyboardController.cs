using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{

    [SerializeField] private GameObject _keyboardButtonPrefab;

    [SerializeField] private GameObject _keyboardContainer;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _clearColor;
    [SerializeField] private Color _okColor;

    [SerializeField] private TextMeshProUGUI  _codeText;

    [SerializeField] private UnityEvent<string> _onOk;

    public void OnClickNumber(int i)
    {
        if (_codeText.text.Length >= 4)
        {
            return;
        }   
        _codeText.text += i.ToString();
    }


    public void OnClickClear()
    {
        _codeText.text = "";
    }

    public void OnClickOk()
    {
        _onOk.Invoke(_codeText.text);
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < 10; ++i)
        {
            ButtonController button = InstantiateButton(i.ToString(), _defaultColor);
            int index = i;
            button.OnClick.AddListener(() => OnClickNumber(index));

        }

        ButtonController clearButton = InstantiateButton("C", _clearColor);
        clearButton.OnClick.AddListener(OnClickClear);

        ButtonController zeroButton = InstantiateButton("0", _defaultColor);
        zeroButton.OnClick.AddListener(() => OnClickNumber(0));

        ButtonController okButton = InstantiateButton("OK", _okColor);
        okButton.OnClick.AddListener(OnClickOk);
    }

    private ButtonController InstantiateButton(string key, Color color)
    {
        GameObject buttonGameobject = Instantiate(_keyboardButtonPrefab, _keyboardContainer.transform);

        TextMeshProUGUI text = buttonGameobject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = key;
        text.color = color;

        return buttonGameobject.GetComponent<ButtonController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
