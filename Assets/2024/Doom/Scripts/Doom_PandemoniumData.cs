using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "DoomPandemoniumData", menuName = "Unlock/DoomPandemoniumData", order = 2)]
public class Doom_PandemoniumData : ScriptableObject
{
    [SerializeField] private SerializedDictionary<string,Demon> _demons;

    public SerializedDictionary<string, Demon> Demon => _demons;
    
}