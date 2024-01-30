using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class MachineKeyboardController : MonoBehaviour
{

    [SerializeField] private LocalizedString _noMachine;
    [SerializeField] private PopupMessageController _messagePopupController;

    public void OnCodeValidate(string stringCode)
    {
        //PopupMessageController popupMessageController = _messagePopup.GetComponent<PopupMessageController>();

        Machine machine = UnlockGameManager.Instance.GetMachine(stringCode);

        if (machine != null)
        {   
            GetComponent<PopupViewController>().closePopup();
            SceneManager.LoadScene(machine.Name,LoadSceneMode.Additive);
            //trigger new Machine
            return;
        }

        _messagePopupController.SetupPopupMessage("<b> " + _noMachine.GetLocalizedString() + "</b>", false, false, false);
        GetComponent<PopupViewController>().closePopup();
        _messagePopupController.OpenPopupMessage(0.3f, () =>
        {
            UnlockGameManager.Instance.TriggerPenalty();
        });
    }
}
