using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    public int Balance { get; private set; }
    public int BetAmount { get; set; }
    public int BetNumber { get; private set; }
    public string Name { get; private set; } // Имя бота

    private int maxBetAmount; // Максимальная сумма ставки
    private List<int> possibleBets; // Список возможных ставок

    public Bot(string name, int initialBalance, int maxBet)
    {
        Name = name; // Установка имени бота
        Balance = initialBalance;
        maxBetAmount = maxBet;

        // Заполнение списка возможных ставок, кратных 50
        possibleBets = new List<int>();
        for (int i = 50; i <= maxBetAmount; i += 50)
        {
            possibleBets.Add(i);
        }

        // Выбор случайной ставки из списка возможных ставок
        ChooseNewBet();
    }

    public void ChooseNewBet()
    {
        // Выбор случайного числа для ставки
        BetAmount = possibleBets[Random.Range(0, possibleBets.Count)];

        // Устанавливаем номер ставки (число от 0 до 9)
        BetNumber = Random.Range(0, 10);

        // Если сумма ставки превышает баланс, устанавливаем ставку в баланс бота
        if (BetAmount > Balance)
        {
            BetAmount = Balance;
        }
    }

    public void PlaceBet(int betNumber)
    {
        if (CanPlaceBet()) // Проверяем, может ли бот сделать ставку
        {
            BetNumber = betNumber;
        }
        else
        {
            BetNumber = -1; // Устанавливаем несуществующее значение, если ставка не может быть сделана
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
