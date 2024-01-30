using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockGameData", menuName = "Unlock/UnlockGameData", order = 1)]
public class UnlockGameData : ScriptableObject
{
    [SerializeField] private SerializedDictionary<string,Machine> _machines;
    [SerializeField] private SerializedDictionary<string,Hint> _hints;
    [SerializeField] private SerializedDictionary<string,HiddenObject> _hiddenObjects;
    [SerializeField] private SerializedDictionary<string,Code> _codes;
    [SerializeField] private SerializedDictionary<string,UnlockEvent> _unlockEvents;

    public SerializedDictionary<string, Machine> Machines => _machines;
    public SerializedDictionary<string, Hint> Hints => _hints;
    public SerializedDictionary<string, HiddenObject> HiddenObjects => _hiddenObjects;
    public SerializedDictionary<string, Code> Codes => _codes;
    public SerializedDictionary<string, UnlockEvent> UnlockEvents => _unlockEvents;
    
}
