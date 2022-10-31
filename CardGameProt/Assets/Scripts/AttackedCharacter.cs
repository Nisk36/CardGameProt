using System;
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

        //敵フィールドにシールドカードがあれば攻撃できない
        CardPresenter[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards();
        if(Array.Exists(enemyFieldCards,card => card.model.ability == ABILITY.SHIELD))
        {
            return;
        }

        if (attacker.model.canAttack)
        {
            GameManager.instance.AttackToCharacter(attacker, true);
        }

    }
}
