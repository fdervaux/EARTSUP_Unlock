using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Unlock.Mario
{
    public class VictoryTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onWin;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(OnTrigger());
            }
        }

        private IEnumerator OnTrigger()
        {
            yield return new WaitForSeconds(.5f);
            
            print("Victory!");

            Destroy(GameObject.FindGameObjectWithTag("Player"));
            
            onWin.Invoke();
        }
    }
}
