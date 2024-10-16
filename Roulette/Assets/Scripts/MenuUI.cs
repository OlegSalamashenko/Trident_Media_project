using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI playerBalanceText;  // ����� ��� ������� ������
    public List<TextMeshProUGUI> botBalanceTexts; // ������ ������� ��� �������� �����

    public Slider botSlider; // ������� ��� ����������� ���������� �����
    public event EventHandler BotSliderValueChanged; // ������� �� ��������� ��������

    private void Start()
    {
        // ����������� ��������� �������� �������� � ������
        botSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // �����, ������� ���������� ��� ��������� �������� ��������
    private void OnSliderValueChanged(float value)
    {
        int newBotCount = Mathf.RoundToInt(value); // ��������� �������� ��������
        BotSliderValueChanged?.Invoke(this, EventArgs.Empty); // ��������� �������
    }

    // ����� ��� ��������� �������� �������� ��������
    public int GetBotSliderValue()
    {
        return Mathf.RoundToInt(botSlider.value); // ��������� �������� ��������
    }

    // ����� ��� ���������� ������� ������
    public void UpdatePlayerBalance(int playerBalance)
    {
        playerBalanceText.text = playerBalance + "$";
    }

    // ����� ��� ���������� �������� �����
    public void UpdateBotBalances(List<Bot> bots)
    {
        for (int i = 0; i < bots.Count; i++)
        {
            if (i < botBalanceTexts.Count) // ���������, ��� �� ������� �� ������� ������
            {
                botBalanceTexts[i].text = bots[i].Balance + "$";
            }
        }
    }
}
