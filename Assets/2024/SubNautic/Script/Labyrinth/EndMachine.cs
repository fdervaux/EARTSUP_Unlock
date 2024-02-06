using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Subnautica
{
    public class EndMachine : MonoBehaviour
    {
        [SerializeField] private bool _endWithColliderTrigger = false;
        private void OnTriggerEnter(Collider other)
        {
            if(_endWithColliderTrigger)
                End();
        }

        public void End()
        {
            print("End !!!");
        }
    }
}