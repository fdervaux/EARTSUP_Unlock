using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class HintViewController : MonoBehaviour
{
    [SerializeField] private PopupMessageController _messagePopupController;
    [SerializeField] private LocalizedString _wrongHintString;
    [SerializeField] private LocalizedString _hintConfirmString;
    [SerializeField] private LocalizedString _hintSolutionString;
    

    public void OnHintValidate(string hintName)
    {
        Hint hint = UnlockGameManager.Instance.getHint(hintName);

        _messagePopupController.CurrentHint = hint;
        _messagePopupController.CurrentHintName = hintName;

        if (hint != null)
        {
            _messagePopupController.OnHint(1);
            GetComponent<PopupViewController>().closePopup();
            _messagePopupController.OpenPopupMessage(0.3f);
            return;
        }

        if (hintName == "")
            return;


        string message2 = _wrongHintString.GetLocalizedString();

        _messagePopupController.UpdateHintPopupMessage(hint, hintName, message2);
        GetComponent<PopupViewController>().closePopup();
        _messagePopupController.OpenPopupMessage(0.3f);
    }
}
