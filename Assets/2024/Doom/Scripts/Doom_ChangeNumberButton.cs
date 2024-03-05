using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doom_ChangeNumberButton : MonoBehaviour
{
   private int currentNumber = 0; // Chiffre actuel du bouton
    private Text buttonText; // Composant Text du bouton

    void Start()
    {
        // Récupérer le composant Text du bouton
        buttonText = GetComponentInChildren<Text>();
        // Mettre le texte du bouton au chiffre actuel (0 au début)
        buttonText.text = currentNumber.ToString();
    }

    // Méthode appelée lorsque le bouton est cliqué
    public void OnButtonClick()
    {
        // Incrémenter le chiffre actuel
        currentNumber++;
        // Si on dépasse 9, revenir à 0
        if (currentNumber > 9)
        {
            currentNumber = 0;
        }
        // Mettre à jour le texte du bouton avec le nouveau chiffre
        buttonText.text = currentNumber.ToString();
    }

    // Méthode pour récupérer le chiffre actuel du bouton
    public int GetCurrentNumber()
    {
        return currentNumber;
    }

    // Méthode pour réinitialiser le chiffre affiché sur le bouton à zéro
    public void ResetNumber()
    {
        currentNumber = 0;
        buttonText.text = currentNumber.ToString();
    }
}
