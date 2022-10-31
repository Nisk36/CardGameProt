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
        //�J�[�h�̃R�X�g��Player�̃}�i�R�X�g���r
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
        //defaultParent�͎����̐e
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        //card���g���}�E�X�|�C���^�����ray�ɔ�������Drop�����܂������Ȃ��̂ň�xDrag�����Drag���̓I�t�ɂ���
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        //eventData.position�̓h���b�O���Ă鎞�̏ꏊ
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
