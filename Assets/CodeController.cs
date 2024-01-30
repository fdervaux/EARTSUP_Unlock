using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class CodeController : MonoBehaviour
{
    [SerializeField] private PopupMessageController _messagePopupController;
    [SerializeField] private LocalizedString _notLongEnoughtString;
    [SerializeField] private LocalizedString _wrongCodeString;
    [SerializeField] private LocalizedString _penaltyString;
    


    public void OnCodeValidate(string stringCode)
    {
        //PopupMessageController popupMessageController = _messagePopup.GetComponent<PopupMessageController>();

        if (stringCode.Length != 4)
        {
            _messagePopupController.SetupPopupMessage(_notLongEnoughtString.GetLocalizedString(), false, false, false);
            _messagePopupController.OpenPopupMessage();
            return;
        }


        Code code = UnlockGameManager.Instance.GetCode(stringCode);

        if (code != null)
        {

            DOTween.Sequence().SetDelay(0.3f).onComplete += () => UnlockGameManager.Instance.TriggerEvent(code.eventName);
            GetComponent<PopupViewController>().closePopup();

            return;
        }


        (_penaltyString["minutes"] as IntVariable).Value = UnlockGameManager.Instance.GetPenaltyTimeInMinute();
        _messagePopupController.SetupPopupMessage("<b> " + _wrongCodeString.GetLocalizedString() + "</b>" + "\n" + _penaltyString.GetLocalizedString(), true, false, false);
        GetComponent<PopupViewController>().closePopup();
        _messagePopupController.OpenPopupMessage(0.3f, () =>
        {
            UnlockGameManager.Instance.TriggerPenalty();
        });
    }
}
