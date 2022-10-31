using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCharacter : MonoBehaviour, IDropHandler
{
    //攻撃される側
    public void OnDrop(PointerEventData eventData)
    {
        /*攻撃*/
        //attackerカードを選択
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
