using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doom_CorridorEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si le joueur entre en collision avec la case
        if (other.CompareTag("Player"))
        {
            // Déclenche la victoire
            UnlockGameManager.Instance.TriggerEvent("9");
            Close();
        }
    }
    
    public void Close(){
        UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("_Unlock_Doom_Salletentacules"); });
    }
}
