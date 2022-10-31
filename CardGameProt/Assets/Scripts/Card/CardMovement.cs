using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform defaultParent;

    public bool isDraggable;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //カードのコストとPlayerのマナコストを比較
        CardPresenter card = GetComponent<CardPresenter>();
        if(!card.model.isFieldCard &&card.model.cost <= GameManager.instance.playerManaCost)
        {
            isDraggable = true;
        }
        else if(card.model.isFieldCard && card.model.canAttack)
        {
            isDraggable = true;
        }
        else
        {
            isDraggable = false;
        }

        if (!isDraggable)
        {
            return;
        }
        //defaultParentは自分の親
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        //card自身がマウスポインタからのrayに反応してDropがうまくいかないので一度DragするとDrag中はオフにする
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        //eventData.positionはドラッグしてる時の場所
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void SetCardTransform(Transform parentTransform)
    {
        defaultParent = parentTransform;
        transform.SetParent(defaultParent);
    }
}
