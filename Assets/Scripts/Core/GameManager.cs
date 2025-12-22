using System.Collections.Generic;
using UnityEngine;
using DesktopOjisan.Server;
using DesktopOjisan.UI;

namespace DesktopOjisan.Core
{
    /// <summary>
    /// ゲーム全体を管理するマネージャ
    /// APIサーバ、UI、コマンド処理を統合
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ApiServer apiServer;
        [SerializeField] private OjisanDialogUI dialogUI;
        [SerializeField] private CommandButtonUI commandButton;
        [SerializeField] private CommandMenuUI commandMenu;

        [Header("Ojisan Settings")]
        [SerializeField] private string[] greetings = new[]
        {
            "やあ、今日も頑張ってるね！",
            "調子はどうだい？",
            "何か手伝おうか？",
            "おっ、また会えたね！"
        };

        [Header("Available Commands")]
        [SerializeField] private List<CommandInfo> availableCommands = new();

        private static GameManager _instance;
        public static GameManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            InitializeDefaultCommands();
        }

        private void Start()
        {
            SetupEventHandlers();
            ShowGreeting();
        }

        private void OnDestroy()
        {
            RemoveEventHandlers();
        }

        private void InitializeDefaultCommands()
        {
            if (availableCommands.Count == 0)
            {
                availableCommands = new List<CommandInfo>
                {
                    new("talk", "おしゃべり", "おじさんと会話する"),
                    new("status", "ステータス確認", "現在の状態を確認"),
                    new("settings", "設定", "アプリの設定を開く"),
                    new("help", "ヘルプ", "使い方を表示"),
                    new("minimize", "最小化", "ウィンドウを最小化"),
                    new("exit", "終了", "アプリを終了")
                };
            }
        }

        private void SetupEventHandlers()
        {
            if (apiServer != null)
            {
                apiServer.OnCommandReceived += HandleApiCommand;
            }

            if (commandButton != null)
            {
                commandButton.OnMainButtonClicked += HandleCommandButtonClick;
            }

            if (commandMenu != null)
            {
                commandMenu.OnCommandSelected += HandleCommandSelected;
            }
        }

        private void RemoveEventHandlers()
        {
            if (apiServer != null)
            {
                apiServer.OnCommandReceived -= HandleApiCommand;
            }

            if (commandButton != null)
            {
                commandButton.OnMainButtonClicked -= HandleCommandButtonClick;
            }

            if (commandMenu != null)
            {
                commandMenu.OnCommandSelected -= HandleCommandSelected;
            }
        }

        private void ShowGreeting()
        {
            if (dialogUI != null && greetings.Length > 0)
            {
                var greeting = greetings[Random.Range(0, greetings.Length)];
                dialogUI.ShowMessage(greeting);
            }
        }

        private void HandleCommandButtonClick()
        {
            if (commandMenu != null)
            {
                commandMenu.Toggle(availableCommands);
            }
        }

        private void HandleCommandSelected(CommandInfo command)
        {
            Debug.Log($"[GameManager] コマンド選択: {command.id}");
            ExecuteCommand(command.id);
        }

        private void HandleApiCommand(ApiCommand command)
        {
            Debug.Log($"[GameManager] API コマンド受信: {command.type}");

            switch (command.type)
            {
                case "message":
                    ShowOjisanMessage(command.data);
                    break;
                default:
                    ExecuteCommand(command.type, command.data);
                    break;
            }
        }

        /// <summary>
        /// コマンドを実行
        /// </summary>
        public void ExecuteCommand(string commandId, string data = null)
        {
            switch (commandId)
            {
                case "talk":
                    ShowOjisanMessage("何か話したいことがあるのかい？");
                    break;

                case "status":
                    ShowOjisanMessage($"サーバはポート{apiServer?.Port}で稼働中だよ！");
                    break;

                case "settings":
                    ShowOjisanMessage("設定画面はまだ準備中なんだ。");
                    break;

                case "help":
                    ShowOjisanMessage("コマンドボタンを押すと、できることが表示されるよ！");
                    break;

                case "minimize":
                    ShowOjisanMessage("ちょっと休憩するね。");
                    // WindowController.Minimize() を呼び出す
                    break;

                case "exit":
                    ShowOjisanMessage("またね！");
                    StartCoroutine(ExitAfterDelay(2f));
                    break;

                case "greet":
                    ShowGreeting();
                    break;

                case "custom":
                    if (!string.IsNullOrEmpty(data))
                    {
                        ShowOjisanMessage(data);
                    }
                    break;

                default:
                    Debug.LogWarning($"[GameManager] 未知のコマンド: {commandId}");
                    ShowOjisanMessage($"「{commandId}」... そのコマンドは知らないなぁ。");
                    break;
            }
        }

        /// <summary>
        /// おじさんにメッセージを表示させる
        /// </summary>
        public void ShowOjisanMessage(string message, bool autoHide = true)
        {
            if (dialogUI != null)
            {
                dialogUI.ShowMessage(message, autoHide);
            }
        }

        /// <summary>
        /// コマンドを追加
        /// </summary>
        public void AddCommand(CommandInfo command)
        {
            if (!availableCommands.Exists(c => c.id == command.id))
            {
                availableCommands.Add(command);
            }
        }

        /// <summary>
        /// コマンドを削除
        /// </summary>
        public void RemoveCommand(string commandId)
        {
            availableCommands.RemoveAll(c => c.id == commandId);
        }

        /// <summary>
        /// コマンドの有効/無効を切り替え
        /// </summary>
        public void SetCommandEnabled(string commandId, bool enabled)
        {
            var command = availableCommands.Find(c => c.id == commandId);
            if (command != null)
            {
                command.isEnabled = enabled;
            }
        }

        private System.Collections.IEnumerator ExitAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
