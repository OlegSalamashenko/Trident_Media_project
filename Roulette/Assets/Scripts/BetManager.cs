using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [SerializeField] private int playerBalance = 1000; // Стартовый баланс игрока
    [SerializeField] private int initialBetAmount = 100; // Начальная сумма ставки
    [SerializeField] private int winningMultiplier = 2; // Множитель выигрыша

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    private int actualWinningNumber;
    private int betNumber = 5; // Ставка игрока

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

        // Проверка, может ли игрок продолжать играть
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
