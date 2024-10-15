using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BetManager : MonoBehaviour
{
    [SerializeField] private int playerBalance = 1000; // Стартовый баланс игрока
    [SerializeField] private int initialBetAmount = 100; // Начальная сумма ставки
    [SerializeField] private int winningMultiplier = 2; // Множитель выигрыша

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    [SerializeField] private int numberOfBots = 3; // Количество ботов
    private List<Bot> bots = new List<Bot>();

    private int actualWinningNumber;
    private int betNumber; // Ставка игрока
    private int bankBalance = 0;

    private void Start()
    {
        // Инициализируем ботов
        for (int i = 0; i < numberOfBots; i++)
        {
            bots.Add(new Bot(1000, 400)); // Каждый бот стартует с 1000 единицами и максимальной ставкой 200
        }

        fortuneWheel.OnBetPlaced += BetManager_OnBetPlaced;
        fortuneWheel.OnGameEnd += BetManager_OnGameEnd;
        moneyUI.UpdateMoneyAmount(playerBalance);
        moneyUI.UpdateBankAmount(bankBalance);
    }

    private void BetManager_OnGameEnd(object sender, System.EventArgs e)
    {
        actualWinningNumber = fortuneWheel.GetWinningSector();

        // Проверка выигрыша игрока
        if (IsWinningBet())
        {
            playerBalance += (initialBetAmount * winningMultiplier) + initialBetAmount;

            // Уменьшаем bankBalance, если это не приведет к отрицательному значению
            if (bankBalance >= initialBetAmount)
            {
                bankBalance -= initialBetAmount;
            }
            else
            {
                bankBalance = 0;
            }
        }

        // Проверка выигрыша каждого бота
        foreach (Bot bot in bots)
        {
            if (bot.BetNumber == actualWinningNumber)
            {
                bot.AddWinnings((bot.BetAmount * winningMultiplier) + bot.BetAmount);

                // Уменьшаем bankBalance, если это не приведет к отрицательному значению
                if (bankBalance >= bot.BetAmount)
                {
                    bankBalance -= bot.BetAmount;
                }
                else
                {
                    bankBalance = 0;
                }
            }

            // После окончания игры бот выбирает новую ставку
            bot.ChooseNewBet();
        }

        // Удаляем ботов с нулевым балансом
        bots.RemoveAll(bot => bot.Balance <= 0);

        // Выводим информацию о ставках ботов
        foreach (Bot bot in bots)
        {
            Debug.Log($"Бот поставил {bot.BetAmount} на число {bot.BetNumber}. Баланс: {bot.Balance}");
        }

        UpdateMoneyUI();
        fortuneWheel.SetSomeMoney(CanPlaceBet());
    }


    private void BetManager_OnBetPlaced(object sender, System.EventArgs e)
    {
        // Игрок делает ставку
        if (CanPlaceBet())
        {
            playerBalance -= initialBetAmount;
            bankBalance += initialBetAmount;
        }

        // Каждый бот делает ставку
        foreach (Bot bot in bots)
        {
            if (bot.CanPlaceBet())
            {
                // Бот выбирает случайное число для ставки
                int betNumber = Random.Range(0, 10);
                bot.PlaceBet(betNumber);

                // Если сумма ставки превышает баланс, устанавливаем ставку в баланс бота
                if (bot.BetAmount > bot.Balance)
                {
                    bot.BetAmount = bot.Balance;
                }

                bot.DeductBet(); // Уменьшаем баланс бота
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
