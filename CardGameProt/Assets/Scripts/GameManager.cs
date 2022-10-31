using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public PlayerManager player;
    [SerializeField] public PlayerManager enemy;
    //�GAI
    [SerializeField] EnemyAI enemyAI;
    //UIManager
    [SerializeField] UIManager uiManager;

    //��D
    [SerializeField] CardPresenter cardPrefab;
    [SerializeField] public Transform playerHand,playerField,enemyHand,enemyField;

    public bool isPlayerTurn;
    

    //�V���O���g����
    public static GameManager instance;

    //���ԊǗ�
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
        uiManager.HideResultPanel();
        player.Init(new List<int>() { 1,5,1,2,3,4});
        enemy.Init(new List<int>() { 2,5,3,1,2,3});
        timeCount = 8;
        uiManager.UpdateTime(timeCount);
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
        uiManager.ShowHP(player.HP, enemy.HP);
        SettingInitHand();
        isPlayerTurn = true;
        TurnCalc();
    }


    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;
        }
        //UI�X�V
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }

    public void Restart()
    {
        //�Ֆʏ�̃J�[�h�폜
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

        //�f�b�L���Đ���
        player.deck = new List<int>() { 1, 1, 2, 2 };
        enemy.deck = new List<int>() { 2, 1, 2, 1 };

        StartGame();
    }

    void SettingInitHand()
    {
        //�J�[�h�����ꂼ���3���z��
        for (int i = 0; i < 3; i++)
        {
            Draw(player.deck, playerHand);
            Draw(enemy.deck, enemyHand);
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
        if(_hand.name == "PlayerHand")
        {
            card.Init(cardID,true);
        }
        else
        {
            card.Init(cardID, false);
        }
        
    }

    //�^�[������
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
            StartCoroutine(enemyAI.EnemyTurn());
        }
    }

    IEnumerator CountDown()
    {
        timeCount = 8;
        uiManager.UpdateTime(timeCount);
        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            uiManager.UpdateTime(timeCount);
        }
        ChangeTurn();
    }

    public CardPresenter[] GetEnemyFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return enemyField.GetComponentsInChildren<CardPresenter>();
        }
        else
        {
            return playerField.GetComponentsInChildren<CardPresenter>();
        }
        
    }

    public CardPresenter[] GetFriendFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return playerField.GetComponentsInChildren<CardPresenter>();
        }
        else
        {
            return enemyField.GetComponentsInChildren<CardPresenter>();
        }
    }

    public void OnClickTurnEndButton()
    {
        if (isPlayerTurn)
        {
            ChangeTurn();
        }
    }

    public void ChangeTurn()
    {
        //isPlayerTurn�̐؂�ւ�
        isPlayerTurn = !isPlayerTurn;

        //�^�[���I�����ɂ�������U���s�\�ɂ���
        CardPresenter[] playerFieldCardList = playerField.GetComponentsInChildren<CardPresenter>();
        SettingCanAttackView(playerFieldCardList, false);
        CardPresenter[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardPresenter>();
        SettingCanAttackView(enemyFieldCardList, false);


        //�^�[���؂�ւ��̍ۂɃh���[
        if (isPlayerTurn)
        {
            player.IncreaseManaCost();
            Draw(player.deck,playerHand);
        }
        else
        {
            enemy.IncreaseManaCost();
            Draw(enemy.deck,enemyHand);
        }
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
        TurnCalc();
    }

    public void SettingCanAttackView(CardPresenter[] fieldCardList, bool canAttack)
    {
        foreach(CardPresenter card in fieldCardList)
        {
            card.ShowSelectablePanel(canAttack);
        }
    }

    void PlayerTurn()
    {
        Debug.Log("player");
        //�t�B�[���h�̃J�[�h���U���\�ɂ���
        CardPresenter[] playerFieldCardList = playerField.GetComponentsInChildren<CardPresenter>();
        SettingCanAttackView(playerFieldCardList, true);
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


    public void AttackToCharacter(CardPresenter attacker)
    {
        if (attacker.model.isPlayerCard)
        {
            enemy.HP -= attacker.model.attack;
        }
        else
        {
            player.HP -= attacker.model.attack;
        }
        //�J�[�h���U���s��
        attacker.ShowSelectablePanel(false);
        uiManager.ShowHP(player.HP, enemy.HP);
        CheckHP();
    }

    public void HealToCharacter(CardPresenter healer)
    {
        if (healer.model.isPlayerCard)
        {
            player.HP += healer.model.attack;
        }
        else
        {
            enemy.HP += healer.model.attack;
        }
        //�\���X�V
        uiManager.ShowHP(player.HP, enemy.HP);
    }

    public void CheckHP()
    {
        if(player.HP <= 0 || enemy.HP <= 0)
        {
            StopAllCoroutines();
            uiManager.ShowResultPanel(player.HP);
        }
        
    }
}
