using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sonar : MonoBehaviour
{
    public List<Collider2D> colliderList;
    public float timeDistance;
    Vector3 point;
    public AudioClip clip;
    private AudioSource source;
    public float sonarFrequency = 5;
    public float sonarTimer;
    public Light2D sonarLight;
    public float lightBibRange;
    public float rangeDecreasedSpeed;
    public float lightRotateSpeed;
    

    private void Start() 
    {
        source = GetComponent<AudioSource>();
    }

    public float GetColliderDistanceTime(out Vector2 pointDirection)
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

        pointDirection = (closestPoint - transform.position).normalized;
        return Mathf.InverseLerp(0, 1, Vector2.Distance(transform.position, closestPoint));
    }
    
    private void Update() 
    {
        Vector2 pointDirection = Vector2.zero;
        timeDistance = GetColliderDistanceTime(out pointDirection);
        sonarLight.transform.up = Vector3.Slerp(sonarLight.transform.up, pointDirection, Time.deltaTime * lightRotateSpeed);

        if(sonarLight.pointLightOuterRadius > 1)
            sonarLight.pointLightOuterRadius -= Time.deltaTime * rangeDecreasedSpeed;

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
        sonarLight.pointLightOuterRadius = lightBibRange;
    }
}
