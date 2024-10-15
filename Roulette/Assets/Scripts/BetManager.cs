using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    [SerializeField] private int playerBalance = 1000; // ��������� ������ ������
    [SerializeField] private int initialBetAmount = 100; // ��������� ����� ������
    [SerializeField] private int winningMultiplier = 2; // ��������� ��������

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    [SerializeField] private int numberOfBots = 3; // ���������� �����
    private List<BotCheckmarkPair> botCheckmarkPairs = new List<BotCheckmarkPair>(); // ����� ����� � ���������

    [SerializeField] private List<Button> numberButtons; // ������ ������ ��� ������� �����
    [SerializeField] private List<RectTransform> botCheckmarks; // ������ ������� ��� ������� ����

    private int actualWinningNumber;
    private int betNumber; // ������ ������
    private int bankBalance = 0;

    private void Start()
    {
        // �������������� ����� � ��������� �� � ���������
        for (int i = 0; i < numberOfBots; i++)
        {
            Bot newBot = new Bot(1000, 400); // ����� ���
            RectTransform checkmark = botCheckmarks[i]; // �������
            botCheckmarkPairs.Add(new BotCheckmarkPair(newBot, checkmark)); // ��������� ����
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

            Debug.Log($"��� �������� {bot.BetAmount} �� ����� {bot.BetNumber}. ������: {bot.Balance}");

            bot.ChooseNewBet();
        }

        // ������� ����� � ������� �������� � �� �������
        for (int i = botCheckmarkPairs.Count - 1; i >= 0; i--)
        {
            if (botCheckmarkPairs[i].Bot.Balance <= 0)
            {
                RemoveBot(i); // ������� ���� � ��� �������
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
        // ������� �������
        botCheckmarkPairs[index].Checkmark.gameObject.SetActive(false);
        // ������� ���� � ��� �������
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
