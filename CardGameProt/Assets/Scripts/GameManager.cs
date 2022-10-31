using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //��D
    [SerializeField] CardPresenter cardPrefab;
    [SerializeField] Transform playerHand,playerField,enemyHand,enemyField;

    bool isPlayerTurn;
    //�f�b�L(ID�ŊǗ�)
    List<int> playerDeck = new List<int>() { 1, 1, 2, 2 },
              enemyDeck = new List<int>() { 2, 1, 2, 1 };

    //�V���O���g����
    public static GameManager instance;

    //�Ղꂢ��[�ƂĂ��{�̂̃f�[�^(������ŃN���X��������)
    int playerHP;
    int enemyHP;
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI enemyHPText;

    //result�֌W��UI
    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;

    //�}�i�R�X�g
    public int playerManaCost;
    public int enemyManaCost;
    int playerDefaultManaCost;
    int enemyDefaultManaCost;

    //�}�i�R�X�g�֌W��url
    [SerializeField] TextMeshProUGUI playerManaCostText;
    [SerializeField] TextMeshProUGUI enemyManaCostText;

    //���ԊǗ�
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
        //UI�X�V
        ShowManaCost();
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
        playerDeck = new List<int>() { 1, 1, 2, 2 };
        enemyDeck = new List<int>() { 2, 1, 2, 1 };

        StartGame();
    }

    void SettingInitHand()
    {
        //�J�[�h�����ꂼ���3���z��
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
        //isPlayerTurn�̐؂�ւ�
        isPlayerTurn = !isPlayerTurn;
        //�^�[���؂�ւ��̍ۂɃh���[
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
        //�t�B�[���h�̃J�[�h���U���\�ɂ���
        CardPresenter[] playerFieldCardList = playerField.GetComponentsInChildren<CardPresenter>();
        foreach(CardPresenter card in playerFieldCardList)
        {
            //card���U���\�\���ɂ���
            card.ShowSelectablePanel(true);
        }
    }

    void EnemyTurn()
    {
        Debug.Log("Enemy");
        //�t�B�[���h�̃J�[�h���X�g���擾
        CardPresenter[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardPresenter>();
        //�t�B�[���h�̃J�[�h���U���\�ɂ���
        foreach(CardPresenter card in enemyFieldCardList)
        {
            card.model.canAttack = true;
        }


        //��ɃJ�[�h���o��
        //��D�̃J�[�h���X�g���擾
        CardPresenter[] handCardList = enemyHand.GetComponentsInChildren<CardPresenter>();
        //�R�X�g�ȉ��̃J�[�h���X�g���擾
        CardPresenter[] selectableHandCardList = Array.FindAll(handCardList, card => card.model.cost <= enemyManaCost);

        if(selectableHandCardList.Length > 0)
        {
            //��ɏo���J�[�h��I��
            CardPresenter enemyCard = selectableHandCardList[0];
            //�J�[�h�̈ړ�
            enemyCard.movement.SetCardTransform(enemyField);
            ReduceManaCost(enemyCard.model.cost, false);
            enemyCard.model.isFieldCard = true;
        }

        //�U��
        //�U���\�J�[�h��I��
        CardPresenter[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.model.canAttack);
        CardPresenter[] playerFieldCardList = playerField.GetComponentsInChildren<CardPresenter>();
        Debug.Log("enemyCanAttackCardList:" + enemyCanAttackCardList.Length);
        if (enemyCanAttackCardList.Length > 0)
        {
            //�U������J�[�h��I��
            CardPresenter attacker = enemyCanAttackCardList[0];
            //player��field�ɃJ�[�h������΂��ꉣ���ĂȂ���Ί��
            if(playerFieldCardList.Length > 0)
            {
                //�U���Ώۂ̃J�[�h��I��
                CardPresenter defender = playerFieldCardList[0];
                //�퓬����
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
        //�J�[�h���U���s��
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
