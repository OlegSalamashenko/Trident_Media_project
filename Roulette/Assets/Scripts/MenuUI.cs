using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI playerBalanceText;  // Текст для баланса игрока
    public List<TextMeshProUGUI> botBalanceTexts; // Список текстов для балансов ботов

    [SerializeField] private Slider botCountSlider; // Добавляем переменную слайдера

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

    // Метод для возврата текущего значения слайдера
    public int GetBotCount()
    {
        return Mathf.RoundToInt(botCountSlider.value); // Возвращаем округленное значение слайдера
    }

    // Метод для обработки изменения значения слайдера
    public void OnBotSliderValueChanged()
    {
        int botCount = GetBotCount();
        Debug.Log("Новое количество ботов: " + botCount); // Добавьте отладочное сообщение
        BetManager.Instance.SetBotCount(botCount);
    }

}
