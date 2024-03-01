using System.Collections;
using System.Collections.Generic;
using Subnautica;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public DragController dragController;
    public Vector3 lastPos;
    public Vector3 currentPos;
    private void LateUpdate() 
    {
        currentPos = dragController.MousePos();
        if(Vector2.Distance(lastPos, currentPos) < 0.1)
        {
            return;
        }
        Vector3 dir = (currentPos - lastPos).normalized ;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, dir), Time.deltaTime * 50);
        
        Quaternion.FromToRotation(Vector3.up, dir);
        lastPos = dragController.MousePos();
    }
}
