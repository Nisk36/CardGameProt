using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    //��D���ꂩ�̔��f
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;
    public void OnDrop(PointerEventData eventData)
    {
        //��D�����D�ւ̈ړ���������Ȃ�����Ȃ�
        if(type == TYPE.HAND)
        {
            return;
        }
        //�J�[�h���d�Ȃ��������g���e�ɂȂ�
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
