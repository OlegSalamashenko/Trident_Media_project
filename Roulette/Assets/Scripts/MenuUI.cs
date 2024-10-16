using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI playerBalanceText;  // ����� ��� ������� ������
    public List<TextMeshProUGUI> botBalanceTexts; // ������ ������� ��� �������� �����

    [SerializeField] private Slider botCountSlider; // ��������� ���������� ��������

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

    // ����� ��� �������� �������� �������� ��������
    public int GetBotCount()
    {
        return Mathf.RoundToInt(botCountSlider.value); // ���������� ����������� �������� ��������
    }

    // ����� ��� ��������� ��������� �������� ��������
    public void OnBotSliderValueChanged()
    {
        int botCount = GetBotCount();
        Debug.Log("����� ���������� �����: " + botCount); // �������� ���������� ���������
        BetManager.Instance.SetBotCount(botCount);
    }

}
