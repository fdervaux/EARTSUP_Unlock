using System.Collections;
using UnityEngine;

public class Bowser_Guard : MonoBehaviour
{
    [SerializeField] private Color _normalColor, _guardingColor;
    [SerializeField] [Range(0, 100)] private float _startGuardPercent;
    [SerializeField] [Range(0.1f, 5f)] private float _guardDuration;
    private SpriteRenderer _bowserRenderer;
    private float _currentGuardPercent;
    private int _randomNumber;
    private bool _isGuarding;

    public bool IsGuarding { get { return _isGuarding; } }

    private void Start()
    {
        _bowserRenderer = GetComponentInChildren<SpriteRenderer>();
        _currentGuardPercent = _startGuardPercent;
    }

    private void Update()
    {
        if (_isGuarding) { return; }

        _randomNumber = Random.Range(0, 101);

        if (_currentGuardPercent >= _randomNumber * 3f)
            StartCoroutine(BowserGuard());
        else
            _currentGuardPercent += Time.deltaTime;
    }

    IEnumerator BowserGuard()
    {
        _isGuarding = true;
        _bowserRenderer.color = _guardingColor;
        yield return new WaitForSeconds(_guardDuration);
        _bowserRenderer.color = _normalColor;
        _currentGuardPercent = 0;
        _isGuarding = false;
    }
}
