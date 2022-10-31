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
        //�R�X�g�����ďo���邾���o��
        while (Array.Exists(handCardList, card => card.model.cost <= gameManager.enemy.manaCost))
        {
            //�R�X�g�ȉ��̃J�[�h���X�g���擾
            CardPresenter[] selectableHandCardList = Array.FindAll(handCardList, card => card.model.cost <= gameManager.enemy.manaCost);
            //��ɏo���J�[�h��I��
            CardPresenter enemyCard = selectableHandCardList[0];
            //�J�[�h�̈ړ�
            enemyCard.movement.SetCardTransform(gameManager.enemyField);
            enemyCard.OnFiled(false);

            //��D�X�V
            handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
            yield return new WaitForSeconds(1);
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
                gameManager.AttackToCharacter(attacker, false);
            }
            enemyFieldCardList = gameManager.enemyField.GetComponentsInChildren<CardPresenter>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        gameManager.ChangeTurn();

    }
}
