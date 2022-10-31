using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //手札
    [SerializeField] CardPresenter cardPrefab;
    [SerializeField] Transform playerHand,playerField,enemyHand,enemyField;

    bool isPlayerTurn;
    //デッキ(IDで管理)
    List<int> playerDeck = new List<int>() { 1, 1, 2, 2 },
              enemyDeck = new List<int>() { 2, 1, 2, 1 };

    //シングルトン化
    public static GameManager instance;

    //ぷれいやーとてき本体のデータ(ここ後でクラス分けたい)
    int playerHP;
    int enemyHP;
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI enemyHPText;

    //result関係のUI
    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;

    //マナコスト
    public int playerManaCost;
    public int enemyManaCost;
    int playerDefaultManaCost;
    int enemyDefaultManaCost;

    //マナコスト関係のurl
    [SerializeField] TextMeshProUGUI playerManaCostText;
    [SerializeField] TextMeshProUGUI enemyManaCostText;

    //時間管理
    [SerializeField] TextMeshProUGUI timeCountText;
    int timeCount;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        resultPanel.SetActive(false);
        playerHP = 2;
        enemyHP = 2;
        playerManaCost = 1;
        enemyManaCost = 1;
        playerDefaultManaCost = 1;
        enemyDefaultManaCost = 1;
        timeCount = 8;
        timeCountText.text = timeCount.ToString();
        ShowManaCost();
        ShowHPText();
        SettingInitHand();
        isPlayerTurn = true;
        TurnCalc();
    }

    void ShowManaCost()
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
    }

    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            playerManaCost -= cost;
        }
        else
        {
            enemyManaCost -= cost;
        }
        //UI更新
        ShowManaCost();
    }

    public void Restart()
    {
        //盤面上のカード削除
        foreach(Transform card in playerHand)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in playerField)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyHand)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyField)
        {
            Destroy(card.gameObject);
        }

        //デッキを再生成
        playerDeck = new List<int>() { 1, 1, 2, 2 };
        enemyDeck = new List<int>() { 2, 1, 2, 1 };

        StartGame();
    }

    void SettingInitHand()
    {
        //カードをそれぞれに3枚配る
        for (int i = 0; i < 3; i++)
        {
            Draw(playerDeck, playerHand);
            Draw(enemyDeck, enemyHand);
        }
    }

    void Draw(List<int> deck,Transform hand)
    {
        if(deck.Count == 0)
        {
            return;
        }
        int cardID = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }

    void CreateCard(int cardID,Transform _hand)
    {
        CardPresenter card = Instantiate(cardPrefab, _hand, false);
        card.Init(cardID);
    }

    //ターン処理
    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
            ChangeTurn();
        }
    }

    IEnumerator CountDown()
    {
        timeCount = 8;
        timeCountText.text = timeCount.ToString();
        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            timeCountText.text = timeCount.ToString();
        }
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        //isPlayerTurnの切り替え
        isPlayerTurn = !isPlayerTurn;
        //ターン切り替えの際にドロー
        if (isPlayerTurn)
        {
            playerDefaultManaCost++;
            playerManaCost = playerDefaultManaCost;
            Draw(playerDeck,playerHand);
        }
        else
        {
            enemyDefaultManaCost++;
            enemyManaCost = enemyDefaultManaCost;
            Draw(enemyDeck,enemyHand);
        }
        ShowManaCost();
        TurnCalc();
    }

    void PlayerTurn()
    {
        Debug.Log("player");
        //フィールドのカードを攻撃可能にする
        CardPresenter[] playerFieldCardList = playerField.GetComponentsInChildren<CardPresenter>();
        foreach(CardPresenter card in playerFieldCardList)
        {
            //cardを攻撃可能表示にする
            card.ShowSelectablePanel(true);
        }
    }

    void EnemyTurn()
    {
        Debug.Log("Enemy");
        //フィールドのカードリストを取得
        CardPresenter[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardPresenter>();
        //フィールドのカードを攻撃可能にする
        foreach(CardPresenter card in enemyFieldCardList)
        {
            card.model.canAttack = true;
        }


        //場にカードを出す
        //手札のカードリストを取得
        CardPresenter[] handCardList = enemyHand.GetComponentsInChildren<CardPresenter>();
        //コスト以下のカードリストを取得
        CardPresenter[] selectableHandCardList = Array.FindAll(handCardList, card => card.model.cost <= enemyManaCost);

        if(selectableHandCardList.Length > 0)
        {
            //場に出すカードを選択
            CardPresenter enemyCard = selectableHandCardList[0];
            //カードの移動
            enemyCard.movement.SetCardTransform(enemyField);
            ReduceManaCost(enemyCard.model.cost, false);
            enemyCard.model.isFieldCard = true;
        }

        //攻撃
        //攻撃可能カードを選択
        CardPresenter[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.model.canAttack);
        CardPresenter[] playerFieldCardList = playerField.GetComponentsInChildren<CardPresenter>();
        Debug.Log("enemyCanAttackCardList:" + enemyCanAttackCardList.Length);
        if (enemyCanAttackCardList.Length > 0)
        {
            //攻撃するカードを選択
            CardPresenter attacker = enemyCanAttackCardList[0];
            //playerのfieldにカードがあればそれ殴ってなければ顔面
            if(playerFieldCardList.Length > 0)
            {
                //攻撃対象のカードを選択
                CardPresenter defender = playerFieldCardList[0];
                //戦闘処理
                CardsBattle(attacker, defender);
            }
            else
            {
                AttackToCharacter(attacker, false);
            }
        }
        
    }

    public void CardsBattle(CardPresenter attacker, CardPresenter defender)
    {
        Debug.Log("CardsBattle");
        Debug.Log("attacker HP: " + attacker.model.hp);
        Debug.Log("defender HP: " + defender.model.hp);
        attacker.Attack(defender);
        defender.Attack(attacker);
        Debug.Log("After Battle");
        Debug.Log("attacker HP: " + attacker.model.hp);
        Debug.Log("defender HP: " + defender.model.hp);
        attacker.CheckAlive();
        defender.CheckAlive();
    }

    void ShowHPText()
    {
        playerHPText.text = playerHP.ToString();
        enemyHPText.text = enemyHP.ToString();
    }

    public void AttackToCharacter(CardPresenter attacker, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            enemyHP -= attacker.model.attack;
        }
        else
        {
            playerHP -= attacker.model.attack;
        }
        //カードを攻撃不可に
        attacker.ShowSelectablePanel(false);
        ShowHPText();
        CheckHP();
    }

    void CheckHP()
    {
        if(playerHP <= 0 || enemyHP <= 0)
        {
            resultPanel.SetActive(true);
            if(playerHP <= 0)
            {
                resultText.text = "LOSE";
            }
            else
            {
                resultText.text = "WIN";
            }
        }
        
    }
}
