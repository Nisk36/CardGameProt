using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.instance;
    }
    public IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy");
        //�t�B�[���h�̃J�[�h���X�g���擾
        CardPresenter[] enemyFieldCardList = gameManager.enemyField.GetComponentsInChildren<CardPresenter>();
        //�t�B�[���h�̃J�[�h���U���\�ɂ���
        gameManager.SettingCanAttackView(enemyFieldCardList, true);

        yield return new WaitForSeconds(1);

        //��ɃJ�[�h���o��
        //��D�̃J�[�h���X�g���擾
        CardPresenter[] handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
        //�X�y�������ʂ̃J�[�h������
        //�R�X�g�����ďo���邾���o��
        //(�R�X�g�ȉ�)����(�X�y���łȂ��@�܂��́@�X�y���ł���g�p�����������Ă�)
        while (Array.Exists(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost)
        && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())) ))
        {
            //�R�X�g�ȉ��̃J�[�h���X�g���擾
            CardPresenter[] selectableHandCardList = Array.FindAll(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost)
                                                                 && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())));
            //��ɏo���J�[�h��I��
            CardPresenter selectCard = selectableHandCardList[0];
            //�X�y���J�[�h�Ȃ�g�p����
            if (selectCard.IsSpell)
            {
                CastSpell(selectCard);
                handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
            }
            else
            {
                //�J�[�h���ړ�
                selectCard.movement.SetCardTransform(gameManager.enemyField);
                selectCard.OnField();
            }

            //��D�X�V
            yield return new WaitForSeconds(1);
            handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
            
        }



        //�U��
        //�t�B�[���h�̃J�[�h���X�g���擾
        enemyFieldCardList = gameManager.enemyField.GetComponentsInChildren<CardPresenter>();
        //�U���\�\�J�[�h������΍U�����J��Ԃ�
        while (Array.Exists(enemyFieldCardList, card => card.model.canAttack))
        {
            //�U���\�J�[�h��I��
            CardPresenter[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.model.canAttack);
            CardPresenter[] playerFieldCardList = gameManager.playerField.GetComponentsInChildren<CardPresenter>();

            //�U������J�[�h��I��
            CardPresenter attacker = enemyCanAttackCardList[0];
            //player��field�ɃJ�[�h������΂��ꉣ���ĂȂ���Ί��
            if (playerFieldCardList.Length > 0)
            {
                //�V�[���h�J�[�h������΂���̂ݍU���Ώ�
                if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
                {
                    playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
                }
                //�U���Ώۂ̃J�[�h��I��
                CardPresenter defender = playerFieldCardList[0];
                //�퓬����
                gameManager.CardsBattle(attacker, defender);
            }
            else
            {
                gameManager.AttackToCharacter(attacker);
            }
            enemyFieldCardList = gameManager.enemyField.GetComponentsInChildren<CardPresenter>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        gameManager.ChangeTurn();

    }

    void CastSpell(CardPresenter card)
    {
        CardPresenter target = null;
        if(card.model.spell == SPELL.HEAL_FRIEND)
        {
            target = gameManager.GetFriendFieldCards(card.model.isPlayerCard)[0];
            Debug.Log(target);
        }
        if(card.model.spell == SPELL.DAMAGE_ENEMY)
        {
            target = gameManager.GetEnemyFieldCards(card.model.isPlayerCard)[0];
        }
        card.UseSpellTo(target);
    }
}
