using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellDropManager : MonoBehaviour, IDropHandler
{
    //攻撃される側
    public void OnDrop(PointerEventData eventData)
    {
        //スペルカードを取得
        CardPresenter spellCard = eventData.pointerDrag.GetComponent<CardPresenter>();
        //targetカードを選択
        CardPresenter target = GetComponent<CardPresenter>();
        if (spellCard == null)
        {
            return;
        }
        if (spellCard.CanUseSpell())
        {
            spellCard.UseSpellTo(target);
        }
    }
}

