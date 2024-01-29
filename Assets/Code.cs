using UnityEngine.Localization;

[System.Serializable]
public class Code
{
    public bool IsError = false;
    public bool IsEnd = false;
    public string[] CardsToTake;
    public string[] CardsToDiscard;
    public LocalizedString message;  
}
