using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI playerBalanceText;  // Текст для баланса игрока
    public List<TextMeshProUGUI> botBalanceTexts; // Список текстов для балансов ботов

    public Slider botSlider; // Слайдер для регулировки количества ботов
    public event EventHandler BotSliderValueChanged; // Событие на изменение слайдера

    private void Start()
    {
        // Привязываем изменение значения слайдера к методу
        botSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Метод, который вызывается при изменении значения слайдера
    private void OnSliderValueChanged(float value)
    {
        int newBotCount = Mathf.RoundToInt(value); // Округляем значение слайдера
        BotSliderValueChanged?.Invoke(this, EventArgs.Empty); // Генерация события
    }

    // Метод для получения текущего значения слайдера
    public int GetBotSliderValue()
    {
        return Mathf.RoundToInt(botSlider.value); // Округляем значение слайдера
    }

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
