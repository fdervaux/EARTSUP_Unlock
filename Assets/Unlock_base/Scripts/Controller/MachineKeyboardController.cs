using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class MachineKeyboardController : MonoBehaviour
{

    [SerializeField] private LocalizedString _noMachine;
    [SerializeField] private PopupMessageController _messagePopupController;

    public void OnCodeValidate(string stringCode)
    {
        if(stringCode.Length == 0)
        {
            return;
        }

        Machine machine = UnlockGameManager.Instance.GetMachine(stringCode);

        if (machine != null)
        {   
            GetComponent<PopupViewController>().closePopup();
            
            var xrManagerSettings = UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager;
            //xrManagerSettings.DeinitializeLoader();
            //xrManagerSettings.InitializeLoader();
            LoaderUtility.Deinitialize();
            LoaderUtility.Initialize();
            GameManager.Instance.TransitionSceneManager.LoadScene(machine.Name, LoadSceneMode.Additive, () =>
            {
                UnlockGameManager.Instance.HideMenuInstant();
            });

            UnlockGameManager.Instance.CurrentMachine = machine;
            //trigger new Machine
            return;
        }

        _messagePopupController.SetupPopupMessage("<b> " + _noMachine.GetLocalizedString() + "</b>", false, false, false);
        GetComponent<PopupViewController>().closePopup();
        _messagePopupController.OpenPopupMessage(0.3f);
    }
}
