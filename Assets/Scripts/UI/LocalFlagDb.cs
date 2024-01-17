using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "LocalFlagDb", order = 1)]
public class LocalFlagDb : ScriptableObject
{
    [SerializedDictionary("Locale", "Flag")] public SerializedDictionary<string,Sprite> _flags = new();
}
