using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [SerializeField] private int playerBalance = 1000; // ��������� ������ ������
    [SerializeField] private int initialBetAmount = 100; // ��������� ����� ������
    [SerializeField] private int winningMultiplier = 2; // ��������� ��������

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    private int actualWinningNumber;
    private int betNumber = 5; // ������ ������

    private void Start()
    {
        fortuneWheel.OnBetPlaced += BetManager_OnBetPlaced;
        fortuneWheel.OnGameEnd += BetManager_OnGameEnd;
        moneyUI.UpdateMoneyUI(playerBalance);
    }

    private void BetManager_OnGameEnd(object sender, System.EventArgs e)
    {
        actualWinningNumber = fortuneWheel.GetWinningSector();
        if (IsWinningBet())
        {
            playerBalance += (initialBetAmount * winningMultiplier) + initialBetAmount;
            Debug.Log("Win");
        }
        UpdateMoneyUI();

        // ��������, ����� �� ����� ���������� ������
        fortuneWheel.SetSomeMoney(CanPlaceBet());
    }

    private void BetManager_OnBetPlaced(object sender, System.EventArgs e)
    {
        if (CanPlaceBet())
        {
            playerBalance -= initialBetAmount;
            UpdateMoneyUI();
        }
        else
        {
            Debug.Log("Not enough money to place the bet.");
        }
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
        moneyUI.UpdateMoneyUI(playerBalance);
    }
}
