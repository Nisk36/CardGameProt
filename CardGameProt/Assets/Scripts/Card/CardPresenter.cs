using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPresenter : MonoBehaviour
{
    public CardModel model;

    CardView view;

    public CardMovement movement;

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID)
    {
        model = new CardModel(cardID);
        view.Show(model);
    }

    public void CheckAlive()
    {
        if (model.isAlive)
        {
            view.Refresh(model);
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

    public void ShowSelectablePanel (bool flag)
    {
        model.canAttack = flag;
        view.ShowSelectablePanel(flag);
    }

}
