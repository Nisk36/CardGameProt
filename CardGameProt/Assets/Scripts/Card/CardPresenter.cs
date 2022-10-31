using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPresenter : MonoBehaviour
{
    public CardModel model;

    CardView view;

    public CardMovement movement;

    GameManager gameManager;

    public bool IsSpell
    {
        get { return model.spell != SPELL.NONE; }
    }

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID,bool isPlayer)
    {
        gameManager = GameManager.instance;
        model = new CardModel(cardID, isPlayer);
        view.Show(model);
    }

    public void CheckAlive()
    {
        if (model.isAlive)
        {
            RefreshView();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Attack(CardPresenter enemyCard)
    {
        model.Attack(enemyCard);
        ShowSelectablePanel(false);
    }

    public void Heal(CardPresenter friendCard)
    {
        model.Heal(friendCard);
        friendCard.RefreshView();
    }

    public void RefreshView()
    {
        view.Refresh(model);
    }
    public void ShowSelectablePanel (bool flag)
    {
        model.canAttack = flag;
        view.ShowSelectablePanel(flag);
    }

    public void OnField()
    {
        gameManager.ReduceManaCost(model.cost, model.isPlayerCard);
        model.isFieldCard = true;
        if(model.ability == ABILITY.FAST_ATTACK)
        {
            ShowSelectablePanel(true);
        }
    }

    //スペル使用可能か判定
    public bool CanUseSpell()
    {
        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY:
            case SPELL.DAMAGE_ENEMYS:
                //相手フィールド全てのカードに攻撃
                CardPresenter[] enemyCards = gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                if(enemyCards.Length > 0)
                {
                    return true;
                }
                return false;
            case SPELL.HEAL_FRIEND:
            case SPELL.HEAL_FRIENDS:
                //味方フィールドカード取得
                CardPresenter[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                if(friendCards.Length > 0)
                {
                    return true;
                }
                return false;
            case SPELL.DAMAGE_ENEMY_PLAYER:
            case SPELL.HEAL_PLAYER:
                return true;
            case SPELL.NONE:
                return false;
        }
        return false;
    }

    //スペル処理関連
    public void UseSpellTo(CardPresenter target)
    {
        switch (model.spell) 
        {
            case SPELL.DAMAGE_ENEMY:
                //特定の敵を攻撃する
                if(target == null)
                {
                    return;
                }
                if(target.model.isPlayerCard == model.isPlayerCard)
                {
                    return;
                }
                Attack(target);
                target.CheckAlive();
                break;
            case SPELL.DAMAGE_ENEMYS:
                //相手フィールド全てのカードに攻撃
                CardPresenter[] enemyCards = gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                foreach(CardPresenter enemyCard in enemyCards)
                {
                    Attack(enemyCard);
                }
                foreach(CardPresenter enemyCard in enemyCards)
                {
                    enemyCard.CheckAlive();
                }
                break;
            case SPELL.DAMAGE_ENEMY_PLAYER:
                gameManager.AttackToCharacter(this);
                break;
            case SPELL.HEAL_FRIEND:
                if(target == null)
                {
                    return;
                }
                if (target.model.isPlayerCard != model.isPlayerCard)
                {
                    return;
                }
                Heal(target);    
                break;
            case SPELL.HEAL_FRIENDS:
                CardPresenter[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                foreach(CardPresenter friendCard in friendCards)
                {
                    Heal(friendCard);
                }
                break;
            case SPELL.HEAL_PLAYER:
                gameManager.HealToCharacter(this);
                break;
            case SPELL.NONE:
                return;
        }
        gameManager.ReduceManaCost(model.cost, model.isPlayerCard);
        Destroy(this.gameObject);
    }
}
