using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    [SerializeField] private int playerBalance = 1000; // Стартовый баланс игрока
    [SerializeField] private int initialBetAmount = 100; // Начальная сумма ставки
    [SerializeField] private int winningMultiplier = 2; // Множитель выигрыша

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    [SerializeField] private int numberOfBots = 3; // Количество ботов
    private List<BotCheckmarkPair> botCheckmarkPairs = new List<BotCheckmarkPair>(); // Связь ботов с галочками

    [SerializeField] private List<Button> numberButtons; // Список кнопок для каждого числа
    [SerializeField] private List<RectTransform> botCheckmarks; // Список галочек для каждого бота

    private int actualWinningNumber;
    private int betNumber; // Ставка игрока
    private int bankBalance = 0;

    private void Start()
    {
        // Инициализируем ботов и связываем их с галочками
        for (int i = 0; i < numberOfBots; i++)
        {
            Bot newBot = new Bot(1000, 400); // Новый бот
            RectTransform checkmark = botCheckmarks[i]; // Галочка
            botCheckmarkPairs.Add(new BotCheckmarkPair(newBot, checkmark)); // Добавляем пару
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
        foreach (var botPair in botCheckmarkPairs)
        {
            Bot bot = botPair.Bot;
            if (bot.BetNumber == actualWinningNumber)
            {
                bot.AddWinnings((bot.BetAmount * winningMultiplier) + bot.BetAmount);

                if (bankBalance >= bot.BetAmount)
                {
                    bankBalance -= bot.BetAmount;
                }
                else
                {
                    bankBalance = 0;
                }
            }

            Debug.Log($"Бот поставил {bot.BetAmount} на число {bot.BetNumber}. Баланс: {bot.Balance}");

            bot.ChooseNewBet();
        }

        // Удаляем ботов с нулевым балансом и их галочки
        for (int i = botCheckmarkPairs.Count - 1; i >= 0; i--)
        {
            if (botCheckmarkPairs[i].Bot.Balance <= 0)
            {
                RemoveBot(i); // Удаляем бота и его галочку
            }
        }

        UpdateMoneyUI();
        fortuneWheel.SetSomeMoney(CanPlaceBet());
    }

    private void BetManager_OnBetPlaced(object sender, System.EventArgs e)
    {
        if (CanPlaceBet())
        {
            playerBalance -= initialBetAmount;
            bankBalance += initialBetAmount;
        }

        foreach (var botPair in botCheckmarkPairs)
        {
            Bot bot = botPair.Bot;
            if (bot.CanPlaceBet())
            {
                int betNumber = Random.Range(0, 10);
                bot.PlaceBet(betNumber);
                bot.DeductBet();
                MoveBotCheckmarkToNumber(botPair.Checkmark, betNumber);
            }
        }

        UpdateMoneyUI();
    }

    private void MoveBotCheckmarkToNumber(RectTransform checkmark, int chosenNumber)
    {
        Button chosenButton = numberButtons[chosenNumber];
        RectTransform buttonRect = chosenButton.GetComponent<RectTransform>();
        checkmark.SetParent(buttonRect.GetChild(0), false);
    }

    private void RemoveBot(int index)
    {
        // Убираем галочку
        botCheckmarkPairs[index].Checkmark.gameObject.SetActive(false);
        // Удаляем бота и его галочку
        botCheckmarkPairs.RemoveAt(index);
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

public class BotCheckmarkPair
{
    public Bot Bot { get; private set; }
    public RectTransform Checkmark { get; private set; }

    public BotCheckmarkPair(Bot bot, RectTransform checkmark)
    {
        Bot = bot;
        Checkmark = checkmark;
    }
}
