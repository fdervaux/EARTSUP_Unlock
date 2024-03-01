using System.Collections;
using System.Collections.Generic;
using Subnautica;
using UnityEngine;
namespace Subnautica
{



public class LoseCondition : MonoBehaviour
{
    public GameObject player;
    public DragController dragController;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Wall"))
        {
            dragController.isDragActive = false;
            Death();
        }    
    }
    private void Death()
    {
        transform.position = new Vector3(0f, -4f, 0f);
    }
}
}
