using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class QuitGameController : MonoBehaviour
{
    [SerializeField] private PopupMessageController _popupMessageController;
    [SerializeField] private LocalizedString _quitGameString;

    public void Back()
    {
        PopupViewController popupViewController = _popupMessageController.transform.GetComponent<PopupViewController>();

        if(popupViewController.IsOpened)
        {
            popupViewController.closePopup();
            return;
        } 

        _popupMessageController.OnYesNoValidate.RemoveAllListeners();

        _popupMessageController.SetupPopupMessage(_quitGameString.GetLocalizedString(), false, true, false);

        _popupMessageController.OnYesNoValidate.AddListener((bool value) =>
        {
            _popupMessageController.OnYesNoValidate.RemoveAllListeners();
            if (value)
            {
                GameManager.Instance.QuitGame();
                
            }
            else
            {
                _popupMessageController.transform.GetComponent<PopupViewController>().closePopup();
            }
        });

        _popupMessageController.transform.GetComponent<PopupViewController>().openPopup();
    }
}
