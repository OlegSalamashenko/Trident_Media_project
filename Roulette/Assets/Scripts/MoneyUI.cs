using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    private TextMeshProUGUI moneyAmount;
    private TextMeshProUGUI bankAmount;
    private void Awake()
    {
        moneyAmount = transform.Find("Money/text").GetComponent<TextMeshProUGUI>();
        bankAmount = transform.Find("Bank/text").GetComponent<TextMeshProUGUI>();
    }
    public void UpdateMoneyAmount(int currentBalance)
    {
        moneyAmount.text = currentBalance.ToString();
    }
    public void UpdateBankAmount(int bankBalance)
    {
        bankAmount.text = bankBalance.ToString();
    }


}
