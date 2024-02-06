using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlock_Doom_CorridorManager : MonoBehaviour
{
    public interface IButton
    {
        Vector3 GerButtonPosition();
    }
   public Vector3 GetButtonPosition()
   {
     return transform.position;
   }

}
