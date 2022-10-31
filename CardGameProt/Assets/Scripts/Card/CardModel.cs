using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public string name;
    public int hp;
    public int attack;
    public int cost;
    public bool isAlive;
    public bool canAttack;
    public bool isFieldCard;

    public CardModel(int cardID)
    {
        //cardEntityからデータ取り出す   
        CardEntity cardEntity = Resources.Load<CardEntity>("CardDatas/Card"+cardID);
        name = cardEntity.name;
        hp = cardEntity.hp;
        attack = cardEntity.attack;
        cost = cardEntity.cost;
        isAlive = true;
    }

    void Damage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }

    public void Attack(CardPresenter card)
    {
        card.model.Damage(attack);
    }
}
