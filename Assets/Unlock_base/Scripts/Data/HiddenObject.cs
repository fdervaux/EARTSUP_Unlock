using UnityEngine.Localization;

[System.Serializable]
public class HiddenObject : IUnlockSaveHint
{
    public float Time;
    public LocalizedString message;
    public string precedingUnlockEventID;
    public string cancelingUnlockEventID;
}
