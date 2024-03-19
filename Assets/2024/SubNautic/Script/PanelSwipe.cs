using System.Collections;
using System.Collections.Generic;
using Subnautica;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelSwipe : MonoBehaviour, IPointerEnterHandler , IPointerMoveHandler , IPointerExitHandler
{
    public PlayerMovements playerMovements;
   public bool isDrag;
    public Vector2 startPoint;
    public Vector2 endPoint;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isDrag = true;
        startPoint = eventData.position;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(isDrag)
        {
            endPoint = eventData.position;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDrag = false;
       
        playerMovements.MovePlayer((endPoint - startPoint).normalized);
    print ("bouge");

        
    }
}
