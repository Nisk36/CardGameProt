using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    //èD‚©ê‚©‚Ì”»’f
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;
    public void OnDrop(PointerEventData eventData)
    {
        //èD‚©‚çèD‚Ö‚ÌˆÚ“®‚¾‚Á‚½‚ç‚È‚ñ‚à‚µ‚È‚¢
        if(type == TYPE.HAND)
        {
            return;
        }
        //ƒJ[ƒh‚ªd‚È‚Á‚½©g‚ªe‚É‚È‚é
        CardPresenter card = eventData.pointerDrag.GetComponent<CardPresenter>();
        if (card != null)
        {
            if (!card.movement.isDraggable)
            {
                return;
            }
            card.movement.defaultParent = this.transform;

            if (card.model.isFieldCard)
            {
                return;
            }
            GameManager.instance.ReduceManaCost(card.model.cost, true);
            card.model.isFieldCard = true;
        }
    }
}
