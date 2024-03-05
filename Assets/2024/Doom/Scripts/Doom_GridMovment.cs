using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doom_GridMovment : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement du personnage
    public Vector3 startPosition; // Position initiale du personnage
    private Vector3 targetPosition; // Position cible du déplacement

    void Start()
    {
        // Définir la position initiale du personnage
        transform.position = startPosition;
        targetPosition = startPosition;
    }

    void Update()
    {
        // Vérifie si le joueur a cliqué sur une case adjacente
        if (Input.GetMouseButtonDown(0))
        {
            // Convertit la position de la souris en position dans le monde
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Vérifie si la case cliquée est adjacente à la position actuelle du personnage
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("GridCell"))
            {
                // Vérifie si la case cliquée est adjacente à la position actuelle du joueur
                if (IsAdjacent(hit.collider.transform.position))
                {
                    // Déplace le personnage vers la case cliquée
                    targetPosition = hit.collider.transform.position;
                }
            }
        }

        // Déplace progressivement le personnage vers la position cible
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Méthode pour vérifier si la case cliquée est adjacente à la position actuelle du personnage
    private bool IsAdjacent(Vector2 clickedPosition)
    {
        // Calcul de la distance entre la position actuelle du joueur et la case cliquée
        float distance = Vector2.Distance(transform.position, clickedPosition);

        // On considère que deux cases sont adjacentes si leur distance est inférieure ou égale à 1.5
        return distance <= 1.5f;
    }
}
