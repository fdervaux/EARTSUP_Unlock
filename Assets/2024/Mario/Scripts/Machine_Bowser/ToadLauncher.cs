using UnityEngine;

public class ToadLauncher : MonoBehaviour
{
    [SerializeField] private Transform _scale;
    [SerializeField] float _maxForce;
    [SerializeField] float _multiplier;
    [SerializeField] Vector3 _minScale;
    [SerializeField] int _gravity;
    private Rigidbody2D _toadbody;
    private Collider2D _toadCollider;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _impulseForce;

    private void Start()
    {
        _toadbody = GetComponent<Rigidbody2D>();
        _toadCollider = GetComponentInChildren<Collider2D>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _toadCollider.enabled = false;
            _impulseForce = Vector2.Distance(transform.position, Input.mousePosition) / 1000;
            _impulseForce = Mathf.Clamp(_impulseForce, 0, _maxForce);
            _direction = GetMousePosition() - transform.position;
            _toadbody.AddForce(_impulseForce * _multiplier * _direction.normalized, ForceMode2D.Impulse);
            _toadbody.gravityScale = _gravity;
        }

        if (_toadbody.velocity.y <= 0)
        {
            _toadCollider.enabled = true;
        }

        if (_toadbody.gravityScale != 0)
        {
            if (transform.localScale.magnitude > _minScale.magnitude)
            {
                _scale.localScale *= 0.99f;
            }
        }

        if (transform.position.y <= -0.5 && _toadCollider.enabled == true)
        {
            ResetToadPosition();
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    public void ResetToadPosition()
    {
        transform.position = _startPosition;
        _toadbody.gravityScale = 0;
        _toadbody.velocity = Vector3.zero;
        _scale.localScale = Vector3.one;
    }
}
