using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{
    //攻撃される側
    public void OnDrop(PointerEventData eventData)
    {
        /*攻撃*/
        //attackerカードを選択
        CardPresenter attacker = eventData.pointerDrag.GetComponent<CardPresenter>();
        //defenderカードを選択
        CardPresenter defender = GetComponent<CardPresenter>();
        if(attacker == null || defender == null)
        {
            return;
        }
        if(attacker.model.isPlayerCard == defender.model.isPlayerCard)
        {
            return;
        }
        //シールドカードがあって、ターゲットがシールドでないなら攻撃できない
        CardPresenter[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD) && defender.model.ability != ABILITY.SHIELD)

        {
            return;
        }
        if (attacker.model.canAttack)
        {
            //attackerとdefenderを戦わせる
            GameManager.instance.CardsBattle(attacker, defender);
            GameManager.instance.CheckHP();
        }

    }
}
