# GameManager

ゲーム全体を統合管理するシングルトンコンポーネント。

## クラス定義

```csharp
namespace DesktopOjisan.Core

public class GameManager : MonoBehaviour
```

## 責務

- APIサーバ、UI、コマンド処理の統合
- おじさんの状態管理
- イベントハンドリングの中継

## 依存関係

```
GameManager
├── ApiServer          # 外部APIからのコマンド受信
├── OjisanDialogUI     # 会話表示
├── CommandButtonUI    # コマンドボタン
└── CommandMenuUI      # コマンドメニュー
```

## シングルトン

```csharp
private static GameManager _instance;
public static GameManager Instance => _instance;
```

## 公開インターフェース

### ExecuteCommand

```csharp
public void ExecuteCommand(string commandId, string data = null)
```

コマンドを実行する。UI選択・API経由どちらからも呼び出し可能。

### ShowOjisanMessage

```csharp
public void ShowOjisanMessage(string message, bool autoHide = true)
```

おじさんにメッセージを表示させる。

### AddCommand / RemoveCommand

```csharp
public void AddCommand(CommandInfo command)
public void RemoveCommand(string commandId)
```

動的にコマンドを追加・削除する。

## ライフサイクル

| メソッド | 処理 |
|---------|------|
| Awake() | シングルトン初期化、デフォルトコマンド登録 |
| Start() | イベントハンドラ登録、挨拶表示 |
| OnDestroy() | イベントハンドラ解除 |

## イベント連携

```csharp
// APIサーバからのコマンド
apiServer.OnCommandReceived += HandleApiCommand;

// UIからのコマンド選択
commandButton.OnMainButtonClicked += HandleCommandButtonClick;
commandMenu.OnCommandSelected += HandleCommandSelected;
```

## 対応ファイル

- `Assets/Scripts/Core/GameManager.cs`
