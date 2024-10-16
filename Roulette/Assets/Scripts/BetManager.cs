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
    [SerializeField] private WinnerUI winnerUI;
    [SerializeField] private MenuUI menuUI;

    private int numberOfBots = 3; // Количество ботов, будет получаться с слайдера
    private List<BotCheckmarkPair> botCheckmarkPairs = new List<BotCheckmarkPair>(); // Связь ботов с галочками

    [SerializeField] private List<Button> numberButtons; // Список кнопок для каждого числа
    [SerializeField] private List<RectTransform> botCheckmarks; // Список галочек для каждого бота

    private int actualWinningNumber;
    private int betNumber; // Ставка игрока
    private int bankBalance = 0;

    public static BetManager Instance { get; private set; } // Singleton

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        numberOfBots = menuUI.GetBotCount(); // Получаем количество ботов с слайдера
        Debug.Log("Количество ботов: " + numberOfBots);
        InitializeBots(); // Инициализируем ботов
        fortuneWheel.OnBetPlaced += BetManager_OnBetPlaced;
        fortuneWheel.OnGameEnd += BetManager_OnGameEnd;
        moneyUI.UpdateMoneyAmount(playerBalance);
        moneyUI.UpdateBankAmount(bankBalance);
    }


    private void BetManager_OnGameEnd(object sender, System.EventArgs e)
    {
        actualWinningNumber = fortuneWheel.GetWinningSector();
        List<string> winnerNames = new List<string>();

        // Проверка выигрыша игрока
        if (IsWinningBet())
        {
            playerBalance += (initialBetAmount * winningMultiplier) + initialBetAmount;
            winnerNames.Add("Player");

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
                winnerNames.Add(bot.Name);

                if (bankBalance >= bot.BetAmount)
                {
                    bankBalance -= bot.BetAmount;
                }
                else
                {
                    bankBalance = 0;
                }
            }
            else
            {
                bankBalance += bot.BetAmount;
            }

            Debug.Log($"Бот {bot.Name} поставил {bot.BetAmount} на число {bot.BetNumber}. Баланс: {bot.Balance}");
            bot.ChooseNewBet();
        }

        // Выводим всех победителей
        if (winnerNames.Count > 0)
        {
            winnerUI.DisplayWinners(winnerNames);
        }

        // Удаляем ботов с нулевым балансом и их галочки
        for (int i = botCheckmarkPairs.Count - 1; i >= 0; i--)
        {
            if (botCheckmarkPairs[i].Bot.Balance <= 0)
            {
                RemoveBot(i);
            }
        }

        UpdateMoneyUI();
        fortuneWheel.SetSomeMoney(CanPlaceBet());
    }

    private void BetManager_OnBetPlaced(object sender, System.EventArgs e)
    {
        winnerUI.ClearWinnerText(); // Очищаем текст победителей перед новой ставкой
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
        botCheckmarkPairs[index].Checkmark.gameObject.SetActive(false);
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

        menuUI.UpdatePlayerBalance(playerBalance);

        List<Bot> bots = new List<Bot>();
        foreach (var botPair in botCheckmarkPairs)
        {
            bots.Add(botPair.Bot);
        }

        menuUI.UpdateBotBalances(bots);

        moneyUI.UpdateBankAmount(bankBalance);
    }

    private void InitializeBots()
    {
        // Очищаем предыдущие данные о ботах и их галочках
        foreach (var botPair in botCheckmarkPairs)
        {
            botPair.Checkmark.gameObject.SetActive(false); // Убираем галочки
        }
        botCheckmarkPairs.Clear(); // Очищаем список пар ботов

        for (int i = 0; i < numberOfBots; i++)
        {
            string botName = "Bot " + (i + 1);
            Bot newBot = new Bot(botName, 1000, 400);
            RectTransform checkmark = botCheckmarks[i];
            checkmark.gameObject.SetActive(true); // Активируем галочку для нового бота
            botCheckmarkPairs.Add(new BotCheckmarkPair(newBot, checkmark)); // Добавляем пару бот-галочка
        }
    }
    private void UpdateBalances()
    {
        List<Bot> bots = new List<Bot>();
        foreach (var botPair in botCheckmarkPairs)
        {
            bots.Add(botPair.Bot);
        }

        menuUI.UpdateBotBalances(bots); // Обновляем тексты балансов ботов в MenuUI
    }



    public void SetBotCount(int count)
    {
        numberOfBots = count;
        InitializeBots(); // Обновляем ботов при изменении числа
        UpdateBalances();  // Обновляем балансы в MenuUI
    }
}
