    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class NumberUI : MonoBehaviour
    {
        [SerializeField] private Transform panel;
        [SerializeField] private BetManager betManager;
        [SerializeField] private RectTransform checkmark; // Ссылка на галочку

        private bool canChange = true;
        private void Start()
        {
            if (canChange)
            {
                SetupButtons();
            }
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

            // Устанавливаем позицию галочки в левом верхнем углу кнопки 
            checkmark.localPosition = buttonRect.localPosition + new Vector3(-buttonRect.sizeDelta.x / 2 + checkmark.rect.width / 2 - 20,
            buttonRect.sizeDelta.y / 2 - checkmark.rect.height / 2 + 10, 0);
        }

        public void SetCanChange(bool canChange)
        {
            this.canChange = canChange;
        }

    }
