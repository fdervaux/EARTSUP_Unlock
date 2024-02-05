using UnityEngine;
using UnityEngine.SceneManagement;

public class ARMachineController : MonoBehaviour
{
    public void OnValidateButton()
    {
        Close();
        UnlockGameManager.Instance.TriggerEvent("1");
    }

    public void OnCloseButton()
    {
        Close();
    }

    private void Close()
    {
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("ARMachine"); });
    }
}
