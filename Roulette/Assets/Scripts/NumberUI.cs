    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class NumberUI : MonoBehaviour
    {
        [SerializeField] private Transform panel;
        [SerializeField] private BetManager betManager;
        [SerializeField] private Transform checkmark; 

        private void Start()
        {
            SetupButtons();
        }
        private void SetupButtons()
        {
            Button[] buttons = panel.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                int number = int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text);
                button.onClick.AddListener(() => OnNumberSelected(button, number));
            }
        }

        private void OnNumberSelected(Button button, int number)
        {
            betManager.SetBetNumber(number);
            Debug.Log("Выбрано число: " + number);

            // Перемещение галочки на выбранную кнопку
            MoveCheckmarkToButton(button);
        }

        private void MoveCheckmarkToButton(Button button)
        {
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            checkmark.SetParent(buttonRect.GetChild(0), false);
    }

    public void LockNumbers(bool canChange)
    {
        // Получаем все кнопки в панели и устанавливаем их состояние
        Button[] buttons = panel.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = canChange; // Блокируем или разблокируем кнопки
        }
    }


}
