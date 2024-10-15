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
    private int betNumber;// Ставка игрока
    private int bankBalance = 0 ;

    private void Start()
    {
        fortuneWheel.OnBetPlaced += BetManager_OnBetPlaced;
        fortuneWheel.OnGameEnd += BetManager_OnGameEnd;
        moneyUI.UpdateMoneyAmount(playerBalance);
        moneyUI.UpdateBankAmount(bankBalance);
    }

    private void BetManager_OnGameEnd(object sender, System.EventArgs e)
    {
        actualWinningNumber = fortuneWheel.GetWinningSector();
        if (IsWinningBet())
        {
            playerBalance += (initialBetAmount * winningMultiplier) + initialBetAmount;
            bankBalance -= initialBetAmount;
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
            bankBalance += initialBetAmount;
            UpdateMoneyUI();
        }
        else
        {
            Debug.Log("Not enough money to place the bet.");
        }
    }

    public void SetBetNumber(int number)
    {
        this.betNumber = number;
    }
    public void IncreaseValue() {
        initialBetAmount += 50;
        initialBetAmount = Mathf.Clamp(initialBetAmount , 0, playerBalance);
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
