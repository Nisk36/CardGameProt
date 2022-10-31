using System;
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

        //�G�t�B�[���h�ɃV�[���h�J�[�h������΍U���ł��Ȃ�
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
