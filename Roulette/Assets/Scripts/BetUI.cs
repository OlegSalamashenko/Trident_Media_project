using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetUI : MonoBehaviour
{

    [SerializeField] private BetManager betManager;

    private TextMeshProUGUI BetValueText;
    private void Awake()
    {
        BetValueText = transform.Find("BetValueText").GetComponent<TextMeshProUGUI>();
        transform.Find("BetIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            betManager.IncreaseValue();
            UpdateText();
        });
        transform.Find("BetDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
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
}
