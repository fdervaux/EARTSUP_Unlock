using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Unlock_Doom_SarcophageManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Le code du boulier fonctionne avec des �tats diff�rents pour chaque branches du boulier," +
                              " l'�tat de chaques branches de gauche � droite correspondes au valeurs suivantes : 0-1-2, " +
                              " Donc si un code comme 0022, le boulier aura les deux premi�res boules tout � gauches et les deux derni�res tout � droite, " +
                              "  |||| UNIQUEMENT LES CHIFFRES 0-1-2 ||||")] private string codeToFind;
    [SerializeField] private string codeFound;
    [SerializeField] List<int> branches = new List<int>(4);


    public void ChangeState(int brancheID)
    {
        branches[brancheID]++;
        if(branches[brancheID] > 2)
        {
            branches[brancheID] = 0;
        }
        ActualizeCodeFound();
    }

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
        }
        else
        {
            print("NOOOOOO");
        }
    }
}
