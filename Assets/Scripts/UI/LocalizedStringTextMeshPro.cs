using TMPro;
using UnityEngine;
using UnityEngine.Localization;

[ExecuteAlways]
public class LocalizedStringTextMeshPro : MonoBehaviour
{
    [SerializeField] private LocalizedString _localizedString;

    private TextMeshProUGUI _localizedText;

    public string LocalizedString
    {
        set
        {
            _localizedString.TableEntryReference = value;
        }
    }

    void OnEnable()
    {
        _localizedText = GetComponent<TextMeshProUGUI>();
        _localizedString.Arguments = new[] { this };
        _localizedString.StringChanged += UpdateString;
    }

    void OnDisable()
    {
        _localizedString.StringChanged -= UpdateString;
    }

    void UpdateString(string text)
    {
        _localizedText.text = text;
    }
}
