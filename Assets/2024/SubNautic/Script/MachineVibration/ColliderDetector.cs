using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    public List<Collider2D> colliderList;
    public float time;
    Vector3 point;
    public float GetColliderDistanceTime()
    {
        float minDistance = Mathf.Infinity;
        Vector3 closestPoint = Vector3.zero;
        foreach (var item in colliderList)
        {
            Vector2 nearestPoint = item.ClosestPoint(transform.position);
            float currentDistance = Vector2.Distance(transform.position, nearestPoint);

            if(currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestPoint = nearestPoint;
            }
        }

        return Mathf.InverseLerp(1, 0, Vector2.Distance(transform.position, closestPoint));
    }
    private void Update() 
    {
        time = GetColliderDistanceTime();
    }
}
