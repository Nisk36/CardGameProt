using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject selectablePanal;

    public void Show(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        hpText.text = cardModel.hp.ToString();
        attackText.text = cardModel.attack.ToString();
        costText.text = cardModel.cost.ToString();
    }

    public void Refresh(CardModel cardModel)
    {
        hpText.text = cardModel.hp.ToString();
        attackText.text = cardModel.attack.ToString();
    }

    public void ShowSelectablePanel(bool flag)
    {
        selectablePanal.SetActive(flag);
    }
}
