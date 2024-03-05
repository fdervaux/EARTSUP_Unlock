using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Subnautica
{
public class LeviathanMouvements : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float movementSpeedUp = 5f;

    
   
    
    void Update()
    {
        transform.Translate(Vector3.up * movementSpeedUp * Time.deltaTime);
    }

     
   

}
}
