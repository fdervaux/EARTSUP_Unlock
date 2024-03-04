using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Subnautica
{
public class KillPlayer : MonoBehaviour
{

    public GameObject Player;

    public GameObject Levi;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
         
         else if (collision.gameObject.tag == "Levi")
        {
            Destroy(gameObject);
           
        }
    }

    
}
}
