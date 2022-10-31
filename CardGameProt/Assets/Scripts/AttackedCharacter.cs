using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCharacter : MonoBehaviour, IDropHandler
{
    //�U������鑤
    public void OnDrop(PointerEventData eventData)
    {
        /*�U��*/
        //attacker�J�[�h��I��
        CardPresenter attacker = eventData.pointerDrag.GetComponent<CardPresenter>();
        if (attacker == null)
        {
            return;
        }
        if (attacker.model.canAttack)
        {
            GameManager.instance.AttackToCharacter(attacker, true);
        }

    }
}
