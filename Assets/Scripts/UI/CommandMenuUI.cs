using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DesktopOjisan.UI
{
    /// <summary>
    /// コマンドメニューUI
    /// 利用可能なコマンド一覧を表示
    /// </summary>
    public class CommandMenuUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Transform commandListParent;
        [SerializeField] private GameObject commandItemPrefab;
        [SerializeField] private Button closeButton;

        [Header("Settings")]
        [SerializeField] private bool closeOnCommandSelect = true;

        private readonly List<GameObject> _commandItems = new();

        public event Action<CommandInfo> OnCommandSelected;
        public event Action OnMenuClosed;

        public bool IsOpen => menuPanel != null && menuPanel.activeSelf;

        private void Awake()
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(false);
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }
        }

        private void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Close);
            }
        }

        /// <summary>
        /// メニューを開く
        /// </summary>
        public void Open(List<CommandInfo> commands)
        {
            if (menuPanel == null) return;

            ClearCommandItems();
            CreateCommandItems(commands);

            menuPanel.SetActive(true);
        }

        /// <summary>
        /// メニューを閉じる
        /// </summary>
        public void Close()
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(false);
            }

            OnMenuClosed?.Invoke();
        }

        /// <summary>
        /// メニューの開閉をトグル
        /// </summary>
        public void Toggle(List<CommandInfo> commands)
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open(commands);
            }
        }

        private void ClearCommandItems()
        {
            foreach (var item in _commandItems)
            {
                Destroy(item);
            }
            _commandItems.Clear();
        }

        private void CreateCommandItems(List<CommandInfo> commands)
        {
            if (commandListParent == null || commandItemPrefab == null) return;

            foreach (var command in commands)
            {
                var item = Instantiate(commandItemPrefab, commandListParent);
                SetupCommandItem(item, command);
                _commandItems.Add(item);
            }
        }

        private void SetupCommandItem(GameObject item, CommandInfo command)
        {
            // ボタンの設定
            var button = item.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => HandleCommandClick(command));
                button.interactable = command.isEnabled;
            }

            // テキストの設定
            var text = item.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = command.displayName;
            }

            // アイコンの設定
            var icon = item.transform.Find("Icon")?.GetComponent<Image>();
            if (icon != null && command.icon != null)
            {
                icon.sprite = command.icon;
                icon.gameObject.SetActive(true);
            }

            // 説明の設定
            var description = item.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
            if (description != null)
            {
                description.text = command.description;
            }
        }

        private void HandleCommandClick(CommandInfo command)
        {
            OnCommandSelected?.Invoke(command);

            if (closeOnCommandSelect)
            {
                Close();
            }
        }
    }

    [Serializable]
    public class CommandInfo
    {
        public string id;
        public string displayName;
        public string description;
        public Sprite icon;
        public bool isEnabled = true;

        public CommandInfo(string id, string displayName, string description = "")
        {
            this.id = id;
            this.displayName = displayName;
            this.description = description;
            this.isEnabled = true;
        }
    }
}
