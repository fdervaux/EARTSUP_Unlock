using UnityEngine.Localization;

[System.Serializable]
public class Machine
{
    public string Name;
    public string[] CardsToTake;
    public string[] CardsToDiscard;
    public LocalizedString message;
}
