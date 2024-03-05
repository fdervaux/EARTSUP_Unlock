using UnityEngine;
using UnityEngine.Events;

public class Bowser_Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private UnityEvent _onDeath;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public int Health 
    { 
        get 
        { 
            return _currentHealth; 
        } 
        set 
        { 
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                _onDeath.Invoke();
            }
        } 
    }

    public void DecreaseHealth() => Health = Health - 1;
}
