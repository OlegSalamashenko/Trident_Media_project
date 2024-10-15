using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    public int Balance { get; private set; }
    public int BetAmount { get;set; }
    public int BetNumber { get; private set; }

    private int maxBetAmount; // ������������ ����� ������
    private List<int> possibleBets; // ������ ��������� ������

    public Bot(int initialBalance, int maxBet)
    {
        Balance = initialBalance;
        maxBetAmount = maxBet;

        // ���������� ������ ��������� ������, ������� 50
        possibleBets = new List<int>();
        for (int i = 50; i <= maxBetAmount; i += 50)
        {
            possibleBets.Add(i);
        }

        // ����� ��������� ������ �� ������ ��������� ������
        ChooseNewBet();
    }

    public void ChooseNewBet()
    {
        // ����� ���������� ����� ��� ������
        BetAmount = possibleBets[UnityEngine.Random.Range(0, possibleBets.Count)];

        // ������������� ����� ������ (����� �� 0 �� 9)
        BetNumber = UnityEngine.Random.Range(0, 10);

        // ���� ����� ������ ��������� ������, ������������� ������ � ������ ����
        if (BetAmount > Balance)
        {
            BetAmount = Balance;
        }
    }

    public void PlaceBet(int betNumber)
    {
        if (CanPlaceBet()) // ���������, ����� �� ��� ������� ������
        {
            BetNumber = betNumber;
        }
        else
        {
            BetNumber = -1; // ������������� �������������� ��������, ���� ������ �� ����� ���� �������
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
