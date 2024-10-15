using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText; // ����� ��� ����������� �����������

    public void DisplayWinners(List<string> winners)
    {
        // ���������� ����� ����������� � ���� ������
        string winnerList = string.Join(", ", winners);
        winnerText.text = "Winners: " + winnerList; // ��������� �����
    }
    private void Start()
    {
        ClearWinnerText();
    }

    // ����� ��� ������� ������
    public void ClearWinnerText()
    {
        winnerText.text = ""; // ������� �����
    }
}
