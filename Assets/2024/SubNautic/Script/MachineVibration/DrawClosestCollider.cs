using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Subnautica
{


public class DrawClosestCollider : MonoBehaviour
{
    public Vector3 location;
    public void OnDrawGizmos()
    {
        var collider = GetComponent<Collider2D>();
        if(!collider)
        return;
        Vector3 closestPoint = collider.ClosestPoint(location);
        Gizmos.DrawSphere(location, 0.1f);
        Gizmos.DrawWireSphere(closestPoint, 0.1f);

    }
}

}