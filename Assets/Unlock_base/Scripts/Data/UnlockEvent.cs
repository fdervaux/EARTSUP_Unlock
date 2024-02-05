using UnityEngine.Localization;


[System.Serializable]
public class UnlockEvent
{
    public LocalizedString localizedString;
    public string[] _cardToTake;

    public string[] _cardToDiscard;

    public bool _isEndEvent;
    public bool _isPenaltyEvent;
}
