using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyHelper
{
    internal static void Destroy(Object @object)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Object.Destroy(@object);
        }
        else
        {
            Object.DestroyImmediate(@object);
        }
#else
            Object.Destroy(@object);
#endif
    }
}
[DisallowMultipleComponent]      
[ExecuteInEditMode, RequireComponent(typeof(Image))]
public class HSV_UI : MonoBehaviour
{
    private static readonly int HSVAAdjust = Shader.PropertyToID("_HSVAAdjust");
    private static readonly int _HSVRangeMax = Shader.PropertyToID("_HSVRangeMax");
    private static readonly int _HSVRangeMin = Shader.PropertyToID("_HSVRangeMin");

    private Material material;
    [HideInInspector, SerializeField] private MaskableGraphic image;

    [SerializeField, Range(0, 1)] private float _rangeMin = 0;
    [SerializeField, Range(0, 1)] private float _rangeMax = 1;
    

    [SerializeField, Range(-1, 1)] private float hue;
    [SerializeField, Range(-1, 1)] private float saturation;
    [SerializeField, Range(-1, 1)] private float value;
    [SerializeField, Range(-1, 1)] private float alpha;


    public float RangeMin
    {
        get => _rangeMin;
        set
        {
            _rangeMin = value;
            if(Application.isPlaying)
                Refresh();
        }
    }

    public float RangeMax
    {
        get => _rangeMax;
        set
        {
            _rangeMax = value;
            if(Application.isPlaying)
                Refresh();
        }
    }

    public float Hue
    {
        get => hue;
        set
        {
            hue = value;
            if(Application.isPlaying)
                Refresh();
        }
    }

    public float Saturation
    {
        get => saturation;
        set
        {
            saturation = value;
            if(Application.isPlaying)
                Refresh();
        }
    }

    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            if(Application.isPlaying)
                Refresh();
        }
    }

    public float Alpha
    {
        get => alpha;
        set
        {
            alpha = value;
            if(Application.isPlaying)
                Refresh();
        }
    }
    

    public void Validate()
    {
        if (material == null)
        {
            material = new Material(Shader.Find("UI/HSV"));
        }

        if (image == null)
        {
            TryGetComponent(out image);
        }

        if (image != null)
        {
            image.material = material;
        }
    }

    public void Refresh()
    {
        if (material != null)
        {
            material.SetFloat(_HSVRangeMin, _rangeMin);
            material.SetFloat(_HSVRangeMax, _rangeMax);
            material.SetVector(HSVAAdjust, new Vector4(hue, saturation, value, alpha));

            image.materialForRendering.SetVector(HSVAAdjust, new Vector4(hue, saturation, value, alpha));
            image.materialForRendering.SetFloat(_HSVRangeMin, _rangeMin);
            image.materialForRendering.SetFloat(_HSVRangeMax, _rangeMax);
        }
    }


    public void OnEnable()
    {
        Validate();
        Refresh();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (enabled && material != null)
        {
            Refresh();
        }
    }


    private void OnDestroy()
    {
        image.material = null;

        DestroyHelper.Destroy(material);
        image = null;
        material = null;
    }

    public void OnValidate()
    {
        Validate();
        Refresh();
    }

}
