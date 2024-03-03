using UnityEngine;
using UnityEngine.SceneManagement;

public class MachineController : MonoBehaviour
{

    public void OnPenaltyButton()
    {
        UnlockGameManager.Instance.TriggerPenalty();
    }

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
        UnlockGameManager.Instance.CurrentMachine = null;
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("MachineTuto"); });
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
