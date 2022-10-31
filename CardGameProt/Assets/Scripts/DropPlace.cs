using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    //手札か場かの判断
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;
    public void OnDrop(PointerEventData eventData)
    {
        //手札から手札への移動だったらなんもしない
        if(type == TYPE.HAND)
        {
            return;
        }
        //カードが重なった時自身が親になる
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
            card.OnFiled(true);
        }
    }
}
