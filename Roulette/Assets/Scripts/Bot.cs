using UnityEngine;

public class Bot
{
    public string Name { get; private set; }
    public int Balance { get; private set; }
    public int BetAmount { get; private set; }
    public int BetNumber { get; private set; }

    // Конструктор по умолчанию, устанавливающий нулевой баланс
    public Bot()
    {
        Name = "EmptyBot";
        Balance = 0;
        BetAmount = 0;
        BetNumber = -1; // -1 значит, что бот не делает ставок
    }

    // Конструктор с параметрами для создания активных ботов
    public Bot(string name, int balance, int betAmount)
    {
        Name = name;
        Balance = balance;
        BetAmount = betAmount;
        BetNumber = -1;
    }

    // Пример метода для выигрыша
    public void AddWinnings(int amount)
    {
        Balance += amount;
    }

    // Пример метода для выбора новой ставки
    public void ChooseNewBet()
    {
        BetNumber = Random.Range(0, 10);
    }

    // Проверка, может ли бот сделать ставку
    public bool CanPlaceBet()
    {
        return Balance > 0 && BetAmount > 0;
    }

    // Метод для ставки
    public void PlaceBet(int number)
    {
        BetNumber = number;
    }

    // Вычитание ставки
    public void DeductBet()
    {
        Balance -= BetAmount;
    }
}
