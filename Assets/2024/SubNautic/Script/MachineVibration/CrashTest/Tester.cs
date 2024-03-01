using Subnautica.RDG;
using UnityEngine;

namespace Subnautica
{
public class Tester : MonoBehaviour
{
    private void Update() 
    {
        if(Input.GetMouseButton(0))
        {
            Vibration.Vibrate(1000, -1  , false);
            Debug.Log("Vibrate");
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Vibration.Cancel();
        }
    }
}

}