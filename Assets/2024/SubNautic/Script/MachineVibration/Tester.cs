using UnityEngine;

namespace Subnautica
{

public class Tester : MonoBehaviour
{
    private void Start() 
    {
        SubnauticaVibrateMachine.Vibrate();
        SubnauticaVibrateMachine.Vibrate(1000);
    }
}

}