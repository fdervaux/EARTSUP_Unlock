using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownFlagsItem : MonoBehaviour, IPointerClickHandler
{
    private bool isOn = false;
    private DropDownFlagsItemData data;
    private int index = 0;

    [SerializeField] private Sprite _checkMarkSprite;
    [SerializeField] private Image _checkMarkImage;

    public void setOn(bool isOn)
    {
        this.isOn = isOn;
        _checkMarkImage.sprite = isOn ? _checkMarkSprite : null;
        _checkMarkImage.color = isOn ? Color.white : Color.clear;
    }

    public void Init(DropDownFlagsItemData data, int index , bool isOn = false)
    {
        this.data = data;
        this.index = index;
        GetComponent<Image>().sprite = data.sprite;

        setOn(isOn);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isOn = !isOn;

        DropDownFlags dropdownFlags = GetComponentInParent<DropDownFlags>();
        dropdownFlags.Select(index);

        setOn(isOn);
    }
}
