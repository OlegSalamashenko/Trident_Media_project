using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    public int Balance { get; private set; }
    public int BetAmount { get;set; }
    public int BetNumber { get; private set; }

    private int maxBetAmount; // ћаксимальна€ сумма ставки
    private List<int> possibleBets; // —писок возможных ставок

    public Bot(int initialBalance, int maxBet)
    {
        Balance = initialBalance;
        maxBetAmount = maxBet;

        // «аполнение списка возможных ставок, кратных 50
        possibleBets = new List<int>();
        for (int i = 50; i <= maxBetAmount; i += 50)
        {
            possibleBets.Add(i);
        }

        // ¬ыбор случайной ставки из списка возможных ставок
        ChooseNewBet();
    }

    public void ChooseNewBet()
    {
        // ¬ыбор случайного числа дл€ ставки
        BetAmount = possibleBets[UnityEngine.Random.Range(0, possibleBets.Count)];

        // ”станавливаем номер ставки (число от 0 до 9)
        BetNumber = UnityEngine.Random.Range(0, 10);

        // ≈сли сумма ставки превышает баланс, устанавливаем ставку в баланс бота
        if (BetAmount > Balance)
        {
            BetAmount = Balance;
        }
    }

    public void PlaceBet(int betNumber)
    {
        if (CanPlaceBet()) // ѕровер€ем, может ли бот сделать ставку
        {
            BetNumber = betNumber;
        }
        else
        {
            BetNumber = -1; // ”станавливаем несуществующее значение, если ставка не может быть сделана
        }
    }

    public bool CanPlaceBet()
    {
        return Balance >= BetAmount;
    }

    public void DeductBet()
    {
        Balance -= BetAmount;
    }

    public void AddWinnings(int amount)
    {
        Balance += amount;
    }
}
