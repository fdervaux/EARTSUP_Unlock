using UnityEngine;

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
            print("You Find It !!!!");
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
}
