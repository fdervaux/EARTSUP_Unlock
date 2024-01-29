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
    [SerializeField] private GameObject _messagePopup;
    [SerializeField] private LocalizedString _notLongEnoughtString;    
    [SerializeField] private LocalizedString _wrongCodeString;
    [SerializeField] private LocalizedString _penaltyString;

    [SerializeField] private UnityEvent _onWrongCode;
    

    public void OnCodeValidate(string code)
    {
        PopupMessageController popupMessageController = _messagePopup.GetComponent<PopupMessageController>();

        if (code.Length != 4)
        {

            popupMessageController.SetMessage(_notLongEnoughtString.GetLocalizedString());
            popupMessageController.ShowMessage(true);
            popupMessageController.ShowImage(false);
            popupMessageController.ShowYesNoButtons(false);
            popupMessageController.ShowHintButtons(false);
            _messagePopup.GetComponent<PopupViewController>().openPopup();
            return;
        }

        (_penaltyString["minutes"] as IntVariable).Value = UnlockGameManager.Instance.GetPenaltyTimeInMinute();
        popupMessageController.SetMessage("<b> " + _wrongCodeString.GetLocalizedString() + "</b>" + "\n" + _penaltyString.GetLocalizedString());
        popupMessageController.ShowMessage(true);
        popupMessageController.ShowImage(true);
        popupMessageController.ShowYesNoButtons(false);
        popupMessageController.ShowHintButtons(false);

        
        _onWrongCode.Invoke();

        DOTween.Sequence()
            .SetDelay(0.3f)
            .OnComplete(() => 
            {
                _messagePopup.GetComponent<PopupViewController>().openPopup();
                UnlockGameManager.Instance.TriggerPenalty();
            });

        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
