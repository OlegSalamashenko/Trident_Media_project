using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BetManager : MonoBehaviour
{
    [SerializeField] private int playerBalance = 1000; // ��������� ������ ������
    [SerializeField] private int initialBetAmount = 100; // ��������� ����� ������
    [SerializeField] private int winningMultiplier = 2; // ��������� ��������

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    [SerializeField] private int numberOfBots = 3; // ���������� �����
    private List<Bot> bots = new List<Bot>();

    private int actualWinningNumber;
    private int betNumber; // ������ ������
    private int bankBalance = 0;

    private void Start()
    {
        // �������������� �����
        for (int i = 0; i < numberOfBots; i++)
        {
            bots.Add(new Bot(1000, 400)); // ������ ��� �������� � 1000 ��������� � ������������ ������� 200
        }

        fortuneWheel.OnBetPlaced += BetManager_OnBetPlaced;
        fortuneWheel.OnGameEnd += BetManager_OnGameEnd;
        moneyUI.UpdateMoneyAmount(playerBalance);
        moneyUI.UpdateBankAmount(bankBalance);
    }

    private void BetManager_OnGameEnd(object sender, System.EventArgs e)
    {
        actualWinningNumber = fortuneWheel.GetWinningSector();

        // �������� �������� ������
        if (IsWinningBet())
        {
            playerBalance += (initialBetAmount * winningMultiplier) + initialBetAmount;

            // ��������� bankBalance, ���� ��� �� �������� � �������������� ��������
            if (bankBalance >= initialBetAmount)
            {
                bankBalance -= initialBetAmount;
            }
            else
            {
                bankBalance = 0;
            }
        }

        // �������� �������� ������� ����
        foreach (Bot bot in bots)
        {
            if (bot.BetNumber == actualWinningNumber)
            {
                bot.AddWinnings((bot.BetAmount * winningMultiplier) + bot.BetAmount);

                // ��������� bankBalance, ���� ��� �� �������� � �������������� ��������
                if (bankBalance >= bot.BetAmount)
                {
                    bankBalance -= bot.BetAmount;
                }
                else
                {
                    bankBalance = 0;
                }
            }

            // ����� ��������� ���� ��� �������� ����� ������
            bot.ChooseNewBet();
        }

        // ������� ����� � ������� ��������
        bots.RemoveAll(bot => bot.Balance <= 0);

        // ������� ���������� � ������� �����
        foreach (Bot bot in bots)
        {
            Debug.Log($"��� �������� {bot.BetAmount} �� ����� {bot.BetNumber}. ������: {bot.Balance}");
        }

        UpdateMoneyUI();
        fortuneWheel.SetSomeMoney(CanPlaceBet());
    }


    private void BetManager_OnBetPlaced(object sender, System.EventArgs e)
    {
        // ����� ������ ������
        if (CanPlaceBet())
        {
            playerBalance -= initialBetAmount;
            bankBalance += initialBetAmount;
        }

        // ������ ��� ������ ������
        foreach (Bot bot in bots)
        {
            if (bot.CanPlaceBet())
            {
                // ��� �������� ��������� ����� ��� ������
                int betNumber = Random.Range(0, 10);
                bot.PlaceBet(betNumber);

                // ���� ����� ������ ��������� ������, ������������� ������ � ������ ����
                if (bot.BetAmount > bot.Balance)
                {
                    bot.BetAmount = bot.Balance;
                }

                bot.DeductBet(); // ��������� ������ ����
            }
        }

        UpdateMoneyUI();
    }

    public void SetBetNumber(int number)
    {
        betNumber = number;
    }

    public void IncreaseValue()
    {
        initialBetAmount += 50;
        initialBetAmount = Mathf.Clamp(initialBetAmount, 0, playerBalance);
    }

    public void DecreaseValue()
    {
        initialBetAmount -= 50;
        initialBetAmount = Mathf.Clamp(initialBetAmount, 0, playerBalance);
    }

    public int GetInitialBetAmount()
    {
        return initialBetAmount;
    }

    private bool CanPlaceBet()
    {
        return initialBetAmount > 0 && initialBetAmount <= playerBalance;
    }

    private bool IsWinningBet()
    {
        return betNumber == actualWinningNumber;
    }

    private void UpdateMoneyUI()
    {
        moneyUI.UpdateMoneyAmount(playerBalance);
        moneyUI.UpdateBankAmount(bankBalance);
    }
}
