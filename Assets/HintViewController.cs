using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class HintViewController : MonoBehaviour
{
    [SerializeField] private GameObject _messagePopup;
    [SerializeField] private LocalizedString _wrongHintString;


    [SerializeField] private UnityEvent _onHint;

    public void OnHintValidate(string hintName)
    {

        PopupMessageController popupMessageController = _messagePopup.GetComponent<PopupMessageController>();

        Hint hint = UnlockGameManager.Instance.getHint(hintName);

        popupMessageController.CurrentHint = hint;
        popupMessageController.CurrentHintName = hintName;

        if (hint != null)
        {
            string message = hint.hintMessage1.GetLocalizedString();
            popupMessageController.OnHint(1);

            _onHint.Invoke();

            DOTween.Sequence()
                .SetDelay(0.5f)
                .OnComplete(() =>
                {
                    _messagePopup.GetComponent<PopupViewController>().openPopup();
                });

            return;
        }

        if (hintName == "")
            return;


        string message2 = _wrongHintString.GetLocalizedString();

        popupMessageController.UpdateHintPopupMessage(hint, hintName, message2);


        _onHint.Invoke();

        DOTween.Sequence()
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                _messagePopup.GetComponent<PopupViewController>().openPopup();
            });
    }
}
