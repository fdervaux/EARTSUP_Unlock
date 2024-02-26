using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Subnautica
{
    
public class PhoneVibration : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Vibrate");
            VibratePhone(2000);
        }
    }
    // private void Update() 
    // {   
    //     Debug.Log("Vibrate");
    //     VibratePhone(2000);
    // }
    public void VibratePhone(long milliseconds)
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            Debug.LogWarning("La vibration du téléphone n'est supportée que sur Android.");
        }
    }
}
}
