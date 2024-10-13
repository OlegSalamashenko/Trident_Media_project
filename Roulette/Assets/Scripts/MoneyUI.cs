using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    private TextMeshProUGUI moneyAmount;
    private void Awake()
    {
        moneyAmount = transform.Find("Money/text").GetComponent<TextMeshProUGUI>();
    }
    public void UpdateMoneyUI(int currentBalance)
    {
        moneyAmount.text = currentBalance.ToString();
    }


}
