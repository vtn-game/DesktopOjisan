using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DesktopOjisan.UI
{
    /// <summary>
    /// コマンド選択ボタンUI
    /// メインボタンを押すとコマンドメニューが表示される
    /// </summary>
    public class CommandButtonUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button mainButton;
        [SerializeField] private TextMeshProUGUI mainButtonText;
        [SerializeField] private Image mainButtonIcon;

        [Header("Settings")]
        [SerializeField] private string buttonLabel = "コマンド";

        public event Action OnMainButtonClicked;

        private void Awake()
        {
            if (mainButton != null)
            {
                mainButton.onClick.AddListener(HandleMainButtonClick);
            }

            UpdateButtonLabel();
        }

        private void OnDestroy()
        {
            if (mainButton != null)
            {
                mainButton.onClick.RemoveListener(HandleMainButtonClick);
            }
        }

        private void HandleMainButtonClick()
        {
            OnMainButtonClicked?.Invoke();
        }

        public void SetButtonLabel(string label)
        {
            buttonLabel = label;
            UpdateButtonLabel();
        }

        private void UpdateButtonLabel()
        {
            if (mainButtonText != null)
            {
                mainButtonText.text = buttonLabel;
            }
        }

        public void SetInteractable(bool interactable)
        {
            if (mainButton != null)
            {
                mainButton.interactable = interactable;
            }
        }

        public void SetIcon(Sprite icon)
        {
            if (mainButtonIcon != null)
            {
                mainButtonIcon.sprite = icon;
                mainButtonIcon.gameObject.SetActive(icon != null);
            }
        }
    }
}
