using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //デッキ(IDで管理)
    public List<int> deck = new List<int>();

    //マナコスト
    public int manaCost;
    public int defaultManaCost;

    //HP
    public int HP;

    public void Init(List<int> deck)
    {
        this.deck = deck;
        HP = 10;
        manaCost = 1;
        defaultManaCost = 1;
    }

    public void IncreaseManaCost()
    {
        defaultManaCost++;
        manaCost = defaultManaCost;
    }
}
