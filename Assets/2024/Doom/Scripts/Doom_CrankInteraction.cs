using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doom_CrankInteraction : MonoBehaviour
{
    public float rotationSpeed = 360f; // Vitesse de rotation par seconde (360 degrés par seconde par défaut)
    public float maxRotationDegree = 360f; // Degré de rotation maximal autorisé
    private bool isRotating = false; // Indique si la manivelle est en train de tourner
    private bool rotationCompleted = false; // Indique si la rotation complète a été effectuée
    private Doom_CodeVerifierAndHandle codeVerifier; // Référence au script de vérification du code

    // Méthode appelée lorsque le joueur clique sur la manivelle
    public void OnMouseDown()
    {
        // Vérifier si la rotation est terminée et si l'interaction est autorisée
        if (!rotationCompleted && CanInteract())
        {
            // Activer la rotation de la manivelle
            isRotating = true;
            // Réinitialiser la rotation complétée
            rotationCompleted = false;
            // Afficher un message de débogage pour indiquer que la manivelle tourne
            Debug.Log("La manivelle tourne");
        }
    }

    // Méthode appelée à chaque frame
    private void Update()
    {
        // Si la rotation est activée
        if (isRotating)
        {
            // Faire tourner la manivelle
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            // Vérifier si la rotation a atteint le degré maximal autorisé
            if (Mathf.Abs(transform.localEulerAngles.z) >= maxRotationDegree)
            {
                // Limiter la rotation au degré maximal autorisé
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, maxRotationDegree);
                // Désactiver la rotation après avoir atteint le degré maximal
                isRotating = false;
                // Indiquer que la rotation est complète
                rotationCompleted = true;
                // Désactiver le composant CrankInteraction pour arrêter la rotation
                enabled = false;
            }
        }
    }

    // Méthode pour vérifier si l'interaction avec la manivelle est autorisée
    private bool CanInteract()
    {
        // Vérifier si le code a été correctement saisi
        if (codeVerifier == null)
        {
            codeVerifier = FindObjectOfType<Doom_CodeVerifierAndHandle>();
        }

        if (codeVerifier != null)
        {
            return codeVerifier.IsCodeCorrect();
        }

        return false;
    }
}
