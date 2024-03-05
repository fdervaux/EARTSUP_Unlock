using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public RectTransform Panel;
    public Canvas canvas;

    private void OnEnable()
    {
        ApplySafeArea(Screen.safeArea);
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea(Screen.safeArea);
    }

    void ApplySafeArea(Rect r)
    {
        Vector2Int screenSize;

#if UNITY_EDITOR
        var display = Display.displays[0];
        screenSize = new Vector2Int(display.systemWidth, display.systemHeight);
#else
        screenSize = new Vector2Int(Screen.width, Screen.height);
#endif

        if (!enabled)
            return;

        if (Panel == null)
            Panel = GetComponent<RectTransform>();

        if(canvas == null)
            canvas = GetComponentInParent<Canvas>();

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = r.position;
        Vector2 anchorMax = r.position + r.size;
        anchorMin.x /= screenSize.x;
        anchorMin.y /= screenSize.y;
        anchorMax.x /= screenSize.x;
        anchorMax.y /= screenSize.y;
        Panel.anchorMin = anchorMin;
        Panel.anchorMax = anchorMax;
    }
}
