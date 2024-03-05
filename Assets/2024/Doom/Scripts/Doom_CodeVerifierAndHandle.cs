using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doom_CodeVerifierAndHandle : MonoBehaviour
{
    public Doom_ChangeNumberButton[] codeButtons; // Tableau contenant les boutons du code
    public GameObject handle; // Référence vers l'objet de la manivelle

    // Méthode appelée pour vérifier le code
    public void VerifyCode()
    {
        // Vérifier si le code est correct (6442)
        if (codeButtons[0].GetCurrentNumber() == 6 &&
            codeButtons[1].GetCurrentNumber() == 4 &&
            codeButtons[2].GetCurrentNumber() == 4 &&
            codeButtons[3].GetCurrentNumber() == 2)
        {
            // Activer l'interaction de la manivelle
            handle.GetComponent<Doom_CrankInteraction>().enabled = true;
            // Afficher un message dans la console pour indiquer que le code a été reconnu
            Debug.Log("Code correctement reconnu !");
        }
    }

    // Méthode pour vérifier si le code est correct
    public bool IsCodeCorrect()
    {
        // Vérifier si le code est correct (6442)
        return codeButtons[0].GetCurrentNumber() == 6 &&
               codeButtons[1].GetCurrentNumber() == 4 &&
               codeButtons[2].GetCurrentNumber() == 4 &&
               codeButtons[3].GetCurrentNumber() == 2;
    }
}
