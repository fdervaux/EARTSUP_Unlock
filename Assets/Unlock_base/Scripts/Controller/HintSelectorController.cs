using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintSelectorController : MonoBehaviour
{

    [SerializeField] private GameObject _hintButtonPrefab;
    [SerializeField] private PopupMessageController _messagePopupController;

    [SerializeField] private GameObject _hintContainer;

    public void OnClickHint(UnlockSaveHintItem hintSelector)
    {
        if (hintSelector.unlockSaveHint == null)
            return;


        if (hintSelector.unlockSaveHint is HiddenObject hiddenObject)
        {
            _messagePopupController.SetupPopupMessage(hiddenObject.message.GetLocalizedString(), false, false, false);
        }

        if (hintSelector.unlockSaveHint is Hint hint)
        {
            _messagePopupController.CurrentHint = hint;
            _messagePopupController.CurrentHintName = hintSelector.name;

            _messagePopupController.OnHint(1);
        }

        GetComponent<PopupViewController>().closePopup();
        _messagePopupController.OpenPopupMessage(0.3f);
    }

    public void SetupHints()
    {
        foreach (Transform child in _hintContainer.transform)
        {
            Destroy(child.gameObject);
        }

        //print only last 12 hints unlocked
        for (int i = Mathf.Max(UnlockGameManager.Instance.UnlockedHints.Count - 12, 0); i < UnlockGameManager.Instance.UnlockedHints.Count; ++i)
        {
            UnlockSaveHintItem item = UnlockGameManager.Instance.UnlockedHints[i];
            ButtonController button = InstantiateButton(item.name);
            button.OnClick.AddListener(() => OnClickHint(item));
            button.Activate();
        }

        for (int i = UnlockGameManager.Instance.UnlockedHints.Count; i < 12; ++i)
        {
            InstantiateButton("?");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupHints();
    }

    private ButtonController InstantiateButton(string key)
    {
        GameObject buttonGameobject = Instantiate(_hintButtonPrefab, _hintContainer.transform);

        TextMeshProUGUI text = buttonGameobject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = key;

        return buttonGameobject.GetComponent<ButtonController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
