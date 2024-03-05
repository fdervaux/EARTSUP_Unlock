using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace Unlock.Mario
{
    public class LaunchMachine : MonoBehaviour
    {
        [SerializeField] private Machine machineToOpen;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            
            OnValidateButton();
        }

        [ContextMenu("Validate button")]
        public void OnValidateButton()
        {
            Close();
            OpenMachine();
        }

        private void OpenMachine()
        {
            if (machineToOpen != null)
            {   
                FindObjectOfType<PopupViewController>().closePopup();
            
                var xrManagerSettings = UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager;
                // //xrManagerSettings.DeinitializeLoader();
                // //xrManagerSettings.InitializeLoader();
                LoaderUtility.Deinitialize();
                LoaderUtility.Initialize();
                
                GameManager.Instance.TransitionSceneManager.LoadScene(machineToOpen.Name, LoadSceneMode.Additive, () =>
                {
                    UnlockGameManager.Instance.HideMenuInstant();
                });

                UnlockGameManager.Instance.CurrentMachine = machineToOpen;
                //trigger new Machine
                return;
            }

            GetComponent<PopupViewController>().closePopup();
        }
        
        private void Close()
        {
            UnlockGameManager.Instance.CurrentMachine = null;
            UnlockGameManager.Instance.ShowMenu(() => { SceneManager.UnloadSceneAsync("Machine_Toads"); });
        }
    }
}
