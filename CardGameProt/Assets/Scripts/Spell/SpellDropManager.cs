using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellDropManager : MonoBehaviour, IDropHandler
{
    //�U������鑤
    public void OnDrop(PointerEventData eventData)
    {
        //�X�y���J�[�h���擾
        CardPresenter spellCard = eventData.pointerDrag.GetComponent<CardPresenter>();
        //target�J�[�h��I��
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

