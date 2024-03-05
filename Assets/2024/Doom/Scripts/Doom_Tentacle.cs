using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Doom_Tentacle : MonoBehaviour
{
    // Événement déclenché lorsque le joueur entre en collision avec la case
    public UnityEvent onPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si le joueur entre en collision avec la case
        if (other.CompareTag("Player"))
        {
            // Déclenche l'événement
            onPlayerEnter.Invoke();
        }
    }
}
