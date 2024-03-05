using UnityEngine;
using UnityEngine.SceneManagement;

public class Doom_XyolcraneCodeManager : MonoBehaviour
{
    public string codeToFind;
    public string codeFound;

    public float timerBeforeCodeRestart;
    public float elapsedTime;

    public void CheckCode()
    {
        if (codeFound == codeToFind)
        {
            Close();
        }
    }

    public void AddNoteForCode(string noteAdded)
    {
        elapsedTime = 0;

        codeFound += noteAdded;

        CheckCode();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timerBeforeCodeRestart)
        {
            codeFound = "";
        }
    }

    public void Close()
    {
        UnlockGameManager.Instance.TriggerEvent("5");
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("DoomMenu"); });
    }
}
