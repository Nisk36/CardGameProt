using System;
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
        if(attacker.model.isPlayerCard == defender.model.isPlayerCard)
        {
            return;
        }
        //�V�[���h�J�[�h�������āA�^�[�Q�b�g���V�[���h�łȂ��Ȃ�U���ł��Ȃ�
        CardPresenter[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD) && defender.model.ability != ABILITY.SHIELD)

        {
            return;
        }
        if (attacker.model.canAttack)
        {
            //attacker��defender���킹��
            GameManager.instance.CardsBattle(attacker, defender);
            GameManager.instance.CheckHP();
        }

    }
}
