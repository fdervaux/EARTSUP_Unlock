using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unlock.Mario
{
    public class SlingShot : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private float launchForce = 1.5f;
        [SerializeField] private CurvedLineRenderer curvedLineRenderer;
        [SerializeField] private GameObject bloodStainPrefab;
        [SerializeField] private GameObject tutorialText;

        [Header("Debug mode")] 
        [SerializeField] private bool doPrint = false;
        
        private Vector2 _startMousePos;
        private Vector2 _velocity;

        private Vector3 _cameraBasePosition;
        private Vector3 _toadBasePosition;
        
        private Quaternion _toadBaseRotation;
        
        private bool _isLaunched;
        private bool _hadCollision;

        private Rigidbody2D _rb;

        private LineRenderer _lineRenderer;

        private SpriteRenderer _spriteRenderer;

        private Camera _mainCamera;

        private void Awake()
        {
            curvedLineRenderer = GetComponent<CurvedLineRenderer>();
            _lineRenderer = curvedLineRenderer.GetComponent<LineRenderer>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _rb = GetComponent<Rigidbody2D>();

            if (Camera.main) _mainCamera = Camera.main;
            if (_mainCamera) _cameraBasePosition = _mainCamera.transform.position;
            
            // Store toad's transform in variables
            _toadBasePosition = transform.position;
            _toadBaseRotation = transform.rotation;
        }

        private IEnumerator Start()
        {
            // Fix IPointerDownHandler issue by refreshing the Collider2D
            Collider2D col = GetComponent<Collider2D>();
            
            col.enabled = false;
            
            yield return new WaitForSeconds(.01f);
            
            col.enabled = true;

            // Check if there is a Camera in scene
            yield return new WaitForSeconds(.5f);

            if (!Camera.main)
            {
                Debug.LogError("Il n'y a pas de caméra dans la scène !");
                AppHelper.Quit();
            }
        }

        private void Update()
        {
            if (_isLaunched)
            {
                // Set Toad's rotation according to its velocity
                Vector2 velocity = _rb.velocity;
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
                
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (doPrint) print("Begin drag");
            
            tutorialText.SetActive(false);
            _startMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (doPrint) print("Drag");

            // Set velocity according to drag distance, and draw the trajectory
            Vector2 currentMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _velocity = (_startMousePos - currentMousePosition) * launchForce;
            _velocity.x = Mathf.Clamp(_velocity.x, 0, Mathf.Infinity);
            _velocity = Vector2.ClampMagnitude(_velocity, 35f);

            curvedLineRenderer.DrawTrajectory(_velocity);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (doPrint) print("End drag");

            _isLaunched = true;
            
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.velocity = _velocity;

            _lineRenderer.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_hadCollision)
            {
                _hadCollision = true;
                
                StartCoroutine(OnDeath(other));
            }
        }

        private IEnumerator OnDeath(Collision2D other)
        {
            Instantiate(bloodStainPrefab, other.contacts[0].point, Quaternion.identity);

            _spriteRenderer.enabled = false;

            _rb.constraints = RigidbodyConstraints2D.FreezeAll;

            yield return new WaitForSeconds(1f);

            ResetSlingshot();
        }

        private void ResetSlingshot()
        {
            _isLaunched = false;
            
            _mainCamera.transform.position = _cameraBasePosition;

            transform.position = _toadBasePosition;
            transform.rotation = _toadBaseRotation;

            _spriteRenderer.enabled = true;

            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
            curvedLineRenderer.ResetLineRenderer();

            _hadCollision = false;
        }
    }
}
