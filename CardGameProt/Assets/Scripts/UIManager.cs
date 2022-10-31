using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //result関係のUI
    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;
    //HP関連のUI
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI enemyHPText;

    //マナコスト関係のurl
    [SerializeField] TextMeshProUGUI playerManaCostText;
    [SerializeField] TextMeshProUGUI enemyManaCostText;
    //時間管理UI
    [SerializeField] TextMeshProUGUI timeCountText;

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }

    public void ShowManaCost(int playerManaCost, int enemyManaCost)
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
    }

    public void UpdateTime(int time)
    {
        timeCountText.text = time.ToString();
    }

    public void ShowHP(int playerHP,int enemyHP)
    {
        playerHPText.text = playerHP.ToString();
        enemyHPText.text = enemyHP.ToString();
    }

    public void ShowResultPanel(int playerHP)
    {
        resultPanel.SetActive(true);
        if (playerHP <= 0)
        {
            resultText.text = "LOSE";
        }
        else
        {
            resultText.text = "WIN";
        }
    }
}
