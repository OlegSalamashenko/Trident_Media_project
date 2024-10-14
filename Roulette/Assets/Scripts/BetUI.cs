using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetUI : MonoBehaviour
{
    [SerializeField] private BetManager betManager;

    private TextMeshProUGUI BetValueText;
    private Button betIncreaseBtn;
    private Button betDecreaseBtn;

    private void Awake()
    {
        BetValueText = transform.Find("BetValueText").GetComponent<TextMeshProUGUI>();
        betIncreaseBtn = transform.Find("BetIncreaseBtn").GetComponent<Button>();
        betDecreaseBtn = transform.Find("BetDecreaseBtn").GetComponent<Button>();

        betIncreaseBtn.onClick.AddListener(() =>
        {
            betManager.IncreaseValue();
            UpdateText();
        });

        betDecreaseBtn.onClick.AddListener(() =>
        {
            betManager.DecreaseValue();
            UpdateText();
        });
    }

    private void Start()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        BetValueText.SetText(betManager.GetInitialBetAmount().ToString());
    }

    // Метод для блокировки/разблокировки кнопок
    public void LockBetButtons(bool canChange)
    {
        betIncreaseBtn.interactable = canChange;
        betDecreaseBtn.interactable = canChange;
    }
}
