using UnityEngine.Localization;

[System.Serializable]
public class Hint : IUnlockSaveHint
{
    public bool HasMoreHint;
    public bool HasSolution;
    public LocalizedString hintMessage1;
    public LocalizedString hintMessage2;
    public LocalizedString hintMessageAnswer;
    public string[] cardsToTake;
}
