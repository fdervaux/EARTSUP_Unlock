using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Subnautica
{
    public class LabyrinthCanvas : MonoBehaviour
    {
        [SerializeField] private Image _gameHider;
        [SerializeField] private AnimationCurve _unHideCurve;

        public void HideGame(float duration, Action toDoWhileHide)
        {
            _gameHider.raycastTarget = true;
            _gameHider.DOColor(new Color(0, 0, 0, 1), duration / 2)
            .OnComplete(() =>
            {
                toDoWhileHide.Invoke();
                _gameHider.DOColor(new Color(0, 0, 0, 0), duration / 2)
                .SetEase(_unHideCurve);
                _gameHider.raycastTarget = false;
            });
        }
    }
}
