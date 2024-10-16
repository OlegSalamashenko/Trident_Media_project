using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI playerBalanceText;  // ����� ��� ������� ������
    public List<TextMeshProUGUI> botBalanceTexts; // ������ ������� ��� �������� �����

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
