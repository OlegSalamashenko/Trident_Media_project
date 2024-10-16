using UnityEngine;

public class Bot
{
    public string Name { get; private set; }
    public int Balance { get; private set; }
    public int BetAmount { get; private set; }
    public int BetNumber { get; private set; }

    // ����������� �� ���������, ��������������� ������� ������
    public Bot()
    {
        Name = "EmptyBot";
        Balance = 0;
        BetAmount = 0;
        BetNumber = -1; // -1 ������, ��� ��� �� ������ ������
    }

    // ����������� � ����������� ��� �������� �������� �����
    public Bot(string name, int balance, int betAmount)
    {
        Name = name;
        Balance = balance;
        BetAmount = betAmount;
        BetNumber = -1;
    }

    // ������ ������ ��� ��������
    public void AddWinnings(int amount)
    {
        Balance += amount;
    }

    // ������ ������ ��� ������ ����� ������
    public void ChooseNewBet()
    {
        BetNumber = Random.Range(0, 10);
    }

    // ��������, ����� �� ��� ������� ������
    public bool CanPlaceBet()
    {
        return Balance > 0 && BetAmount > 0;
    }

    // ����� ��� ������
    public void PlaceBet(int number)
    {
        BetNumber = number;
    }

    // ��������� ������
    public void DeductBet()
    {
        Balance -= BetAmount;
    }
}
