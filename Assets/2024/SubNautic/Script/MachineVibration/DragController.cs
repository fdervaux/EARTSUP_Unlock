using Subnautica;
using UnityEngine;

namespace Subnautica
{



public class DragController : MonoBehaviour
{
    private bool isDragActive;
    // public Rigidbody2D rb;
    private Vector2 screenPosition;
    private Vector3 worldPosition;
    private DraggingObject lastDragged;
    private void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if(controllers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
    private void Update() 
    {
        if(isDragActive)
        {
            if (Input. GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Drop();
                return;
            }
        }
        if(Input.GetMouseButton(0))
        {
            Debug.Log("mouseinputclicked");
            Vector3 mousePos = Input.mousePosition;
            screenPosition = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        if(isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if(hit.collider != null)
            {
                DraggingObject draggingObject = hit.transform.gameObject.GetComponent<DraggingObject>();
                if(draggingObject != null)
                {
                    Debug.Log("InitDrag");
                    lastDragged = draggingObject;
                    InitDrag();
                }
            }
        }
    }
    public void InitDrag()
    {
        isDragActive = true;
    }
    public void Drag()
    {
        lastDragged.transform.position = new Vector2(worldPosition.x, worldPosition.y);
        Debug.Log("IsDragging");
    }
    public void Drop()
    {
        isDragActive = false;
    }
}
}