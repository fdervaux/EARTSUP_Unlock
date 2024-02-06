using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doom_SarcophageManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Le code du boulier fonctionne avec des états différents pour chaque branches du boulier," +
                              " l'état de chaques branches de gauche à droite correspondes au valeurs suivantes : 0-1-2, " +
                              " Donc si un code comme 0022, le boulier aura les deux premières boules tout à gauches et les deux dernières tout à droite, " +
                              "  |||| UNIQUEMENT LES CHIFFRES 0-1-2 ||||")] private string codeToFind;
    [SerializeField] private string codeFound;
    [SerializeField] List<int> branches = new List<int>(4);
    [SerializeField] Doom_SarcophageVisual sarcophageVisual;

    private void Start()
    {
        sarcophageVisual = GetComponent<Doom_SarcophageVisual>();
        sarcophageVisual.ActualizeBranchVisual(0, branches[0]);
        sarcophageVisual.ActualizeBranchVisual(1, branches[1]);
        sarcophageVisual.ActualizeBranchVisual(2, branches[2]);
        sarcophageVisual.ActualizeBranchVisual(3, branches[3]);
    }

    //public void ChangeState(int brancheID)
    //{
    //    branches[brancheID]++;
    //    if(branches[brancheID] > 2)
    //    {
    //        branches[brancheID] = 0;
    //    }
    //    ActualizeCodeFound();
    //    sarcophageVisual.ActualizeBranchVisual(brancheID, branches[brancheID]);
    //}

    public void ActualizeCodeFound()
    {
        codeFound = "";
        for (int i = 0; i < branches.Count; i++)
        {
            codeFound += branches[i].ToString();
        }
        CheckIfCodeGood();
    }

    public void CheckIfCodeGood()
    {
        if (codeFound == codeToFind)
        {
            print("YESSSSS");
            Close();
        }
        else
        {
            print("NOOOOOO");
        }
    }

    public void changeStateBranch0(int stateID)
    {
        branches[0] = stateID;
        ActualizeCodeFound();
        sarcophageVisual.ActualizeBranchVisual(0, stateID);
    }
    public void changeStateBranch1(int stateID)
    {
        branches[1] = stateID;
        ActualizeCodeFound();
        sarcophageVisual.ActualizeBranchVisual(1, stateID);
    }
    public void changeStateBranch2(int stateID)
    {
        branches[2] = stateID;
        ActualizeCodeFound();
        sarcophageVisual.ActualizeBranchVisual(2, stateID);
    }
    public void changeStateBranch3(int stateID)
    {
        branches[3] = stateID;
        ActualizeCodeFound();
        sarcophageVisual.ActualizeBranchVisual(3, stateID);
    }

    public void Close()
    {
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("Doom_Menu"); });
    }
}
