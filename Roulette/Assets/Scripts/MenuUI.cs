using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI playerBalanceText;  // Текст для баланса игрока
    public List<TextMeshProUGUI> botBalanceTexts; // Список текстов для балансов ботов

    // Метод для обновления баланса игрока
    public void UpdatePlayerBalance(int playerBalance)
    {
        playerBalanceText.text = playerBalance + "$";
    }

    // Метод для обновления балансов ботов
    public void UpdateBotBalances(List<Bot> bots)
    {
        for (int i = 0; i < bots.Count; i++)
        {
            if (i < botBalanceTexts.Count) // Проверяем, что не выходим за пределы списка
            {
                botBalanceTexts[i].text = bots[i].Balance + "$";
            }
        }
    }
}
