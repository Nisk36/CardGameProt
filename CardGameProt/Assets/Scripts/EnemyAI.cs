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
        //フィールドのカードリストを取得
        CardPresenter[] enemyFieldCardList = gameManager.enemyField.GetComponentsInChildren<CardPresenter>();
        //フィールドのカードを攻撃可能にする
        gameManager.SettingCanAttackView(enemyFieldCardList, true);

        yield return new WaitForSeconds(1);

        //場にカードを出す
        //手札のカードリストを取得
        CardPresenter[] handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
        //スペルか普通のカードか判定
        //コストを見て出せるだけ出す
        //(コスト以下)かつ(スペルでない　または　スペルであり使用条件満たしてる)
        while (Array.Exists(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost)
        && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())) ))
        {
            //コスト以下のカードリストを取得
            CardPresenter[] selectableHandCardList = Array.FindAll(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost)
                                                                 && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())));
            //場に出すカードを選択
            CardPresenter selectCard = selectableHandCardList[0];
            //スペルカードなら使用する
            if (selectCard.IsSpell)
            {
                CastSpell(selectCard);
                handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
            }
            else
            {
                //カードを移動
                selectCard.movement.SetCardTransform(gameManager.enemyField);
                selectCard.OnField();
            }

            //手札更新
            yield return new WaitForSeconds(1);
            handCardList = gameManager.enemyHand.GetComponentsInChildren<CardPresenter>();
            
        }



        //攻撃
        //フィールドのカードリストを取得
        enemyFieldCardList = gameManager.enemyField.GetComponentsInChildren<CardPresenter>();
        //攻撃可能可能カードがあれば攻撃を繰り返す
        while (Array.Exists(enemyFieldCardList, card => card.model.canAttack))
        {
            //攻撃可能カードを選択
            CardPresenter[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.model.canAttack);
            CardPresenter[] playerFieldCardList = gameManager.playerField.GetComponentsInChildren<CardPresenter>();

            //攻撃するカードを選択
            CardPresenter attacker = enemyCanAttackCardList[0];
            //playerのfieldにカードがあればそれ殴ってなければ顔面
            if (playerFieldCardList.Length > 0)
            {
                //シールドカードがあればそれのみ攻撃対象
                if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
                {
                    playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
                }
                //攻撃対象のカードを選択
                CardPresenter defender = playerFieldCardList[0];
                //戦闘処理
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
