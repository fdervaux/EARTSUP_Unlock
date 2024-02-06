using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockPromotions", menuName = "Unlock/Promotions", order = 1)]
public class PromotionsData : ScriptableObject
{
    public List<Promotion> promotion;
}    
