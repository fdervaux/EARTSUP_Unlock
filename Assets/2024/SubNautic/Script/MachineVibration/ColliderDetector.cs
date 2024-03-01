using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    public List<Collider2D> colliderList;
    public float timeDistance;
    Vector3 point;
    public AudioClip clip;
    private AudioSource source;
    public float sonarFrequency = 5;
    public float sonarTimer;

    private void Start() {
        source = GetComponent<AudioSource>();
    }

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

        return Mathf.InverseLerp(0, 1, Vector2.Distance(transform.position, closestPoint));
    }
    
    private void Update() 
    {
        timeDistance = GetColliderDistanceTime();

        if(timeDistance == 1)
            return;

        sonarTimer += Time.deltaTime;
        if(sonarTimer > timeDistance / sonarFrequency)
            Bip();
    }

    private void Bip()
    {
        sonarTimer = 0;
        source.PlayOneShot(clip);
    }
}
