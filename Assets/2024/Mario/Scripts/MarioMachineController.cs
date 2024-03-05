using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioMachineController : MonoBehaviour
{
    [SerializeField] private string _eventToTrigger;
    [SerializeField] private string _sceneToUnload;

    public void OnPenaltyButton()
    {
        UnlockGameManager.Instance.TriggerPenalty();
    }

    public void OnValidateButton()
    {
        Close();
        UnlockGameManager.Instance.TriggerEvent(_eventToTrigger);
    }

    public void OnCloseButton()
    {
        Close();
    }

    private void Close()
    {
        UnlockGameManager.Instance.CurrentMachine = null;
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("MachineTuto"); });
    }
}
