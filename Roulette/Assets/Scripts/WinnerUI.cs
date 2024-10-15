using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText; // Текст для отображения победителей

    public void DisplayWinners(List<string> winners)
    {
        // Объединяем имена победителей в одну строку
        string winnerList = string.Join(", ", winners);
        winnerText.text = "Winners: " + winnerList; // Обновляем текст
    }
    private void Start()
    {
        ClearWinnerText();
    }

    // Метод для очистки текста
    public void ClearWinnerText()
    {
        winnerText.text = ""; // Очищаем текст
    }
}
