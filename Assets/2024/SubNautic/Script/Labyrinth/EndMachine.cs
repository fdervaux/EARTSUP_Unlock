using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Subnautica
{
    public class EndMachine : MonoBehaviour
    {
        [SerializeField] private bool _endWithColliderTrigger = false;
        private void OnTriggerEnter(Collider other)
        {
            if (_endWithColliderTrigger)
                End();
        }

        public void End()
        {
            UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("Subnautica_Labyrinth"); });
            UnlockGameManager.Instance.TriggerEvent("1_SubnauticaEndMachineLabyrinthe");
        }
    }
}