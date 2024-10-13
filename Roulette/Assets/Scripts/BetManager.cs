using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    private int playerBalance = 1000; // Стартовый баланс игрока
    private int betAmount = 100; // Сумма текущей ставки
    private int winningMultiplier = 2; // Множитель выигрыша

    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private FortuneWheel fortuneWheel;

    private void Start()
    {
       fortuneWheel.OnUpdateMoneyUI += BetManager_OnUpdateMoneyUI;
       moneyUI.UpdateMoneyUI(playerBalance);
    }

    private void BetManager_OnUpdateMoneyUI(object sender, System.EventArgs e)
    {
        int actualWinningNumber = fortuneWheel.GetWinningSector();
        int betNumber = 5; // test

        PlaceBet(betNumber, actualWinningNumber);
    }

    public void PlaceBet(int betNumber, int actualWinningNumber)
    {
        if (betAmount > 0 && betAmount <= playerBalance)
        {
            //Can play
            if (betNumber == actualWinningNumber)
            {
                //Win
                playerBalance += betAmount * winningMultiplier;
                Debug.Log("Win");
            }
            else
            {
                //Lose
                playerBalance -= betAmount;
                Debug.Log("Lose");
            }

           
            moneyUI.UpdateMoneyUI(playerBalance);
        }
        else if (betAmount == 0)
        {
            //Can not play
            Debug.Log("You need to place a bet.");
        }
        else
        {
            //Can not play
            Debug.Log("Not enough money to place the bet.");
        }
    }




}
