using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unlock_Doom_PlayerController : MonoBehaviour
{
    // Méthode pour déplacer le personnage instantanément
    public void DeplacerInstantanement(Vector3 positionCible)
    {
        transform.position = positionCible;
    }
}
