using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Promotion
{
    public string PromotionName;
    public Sprite image;
    public bool activated;

    public List<UnlockGame> games;

}
