using UnityEngine;
using UnityEngine.Events;

public class ToadCollisionDetection : MonoBehaviour
{
    [SerializeField] UnityEvent _onTriggerEnter;
    [SerializeField] Bowser_Guard _guard;
    private ToadLauncher _launcher;

    private void Start()
    {
        _launcher = GetComponent<ToadLauncher>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_guard.IsGuarding == true) { return; }
        _onTriggerEnter.Invoke();
        Debug.Log("Hit");
        _launcher.ResetToadPosition();
    }
}
