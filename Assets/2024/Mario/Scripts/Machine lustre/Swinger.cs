using DG.Tweening;
using UnityEngine;

public class Swinger : MonoBehaviour
{
    [SerializeField] RectTransform SwingerRectTrans;
    float SwingerRotation;
    [SerializeField] float speed;
    [SerializeField] Vector2 minMaxRange;
    [SerializeField] Vector2 minMaxRotation;
    public bool isInRange;

    void Update()
    {
        float temp_CurrentSpeed;
        CheckRange();
        CheckRotation();
        temp_CurrentSpeed = speed;
        SwingerRectTrans.Rotate(0, 0, temp_CurrentSpeed);
        SwingerRotation += temp_CurrentSpeed;


    }


    void CheckRange()
    {
        if (SwingerRotation > minMaxRange.x && SwingerRotation < minMaxRange.y)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
        return;
    }
    void CheckRotation()
    {
        if (SwingerRotation < minMaxRotation.x)
        {
            speed = -speed;
        }
        if (SwingerRotation > minMaxRotation.y)
        {
            speed = -speed;
        }
        return;
    }
    public void GoToMiddle(float temp_t)
    {
        speed = 0;
        SwingerRectTrans.DORotate(Vector3.zero, temp_t);
    }
    public void Shake(float temp_t)
    {
        SwingerRectTrans.DOShakePosition(temp_t);
    }
}
