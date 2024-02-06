using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Doom_Demon : MonoBehaviour
{
    [SerializeField] private int _demonLifePoints = 50;
    private Doom_PandemoniumManager _demonManager;

    private void Start()
    {
        _demonManager = GetComponentInParent<Doom_PandemoniumManager>();
    }

    public void HitDemon()
    {
        _demonLifePoints --;
        CheckLifeDemon();
    }

    public void HitCriticalDemon()
    {
        _demonLifePoints -= 10;
        CheckLifeDemon();
    }

    private void CheckLifeDemon()
    {
        if (_demonLifePoints > 0) return;
        _demonManager.Close();
    }
}
