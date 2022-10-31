using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{
    //�U������鑤
    public void OnDrop(PointerEventData eventData)
    {
        /*�U��*/
        //attacker�J�[�h��I��
        CardPresenter attacker = eventData.pointerDrag.GetComponent<CardPresenter>();
        //defender�J�[�h��I��
        CardPresenter defender = GetComponent<CardPresenter>();
        if(attacker == null || defender == null)
        {
            return;
        }
        if (attacker.model.canAttack)
        {
            //attacker��defender���킹��
            GameManager.instance.CardsBattle(attacker, defender);
        }

    }
}
