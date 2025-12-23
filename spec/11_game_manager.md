# GameManager仕様

## 概要

ゲーム全体を統合管理するシングルトンマネージャ。
APIサーバ、UI、コマンド処理を連携させる中心的なコンポーネント。

## ファイル構成

```
Assets/Scripts/Core/
└── GameManager.cs
```

## クラス定義

```csharp
namespace DesktopOjisan.Core

public class GameManager : MonoBehaviour
```

## シングルトンパターン

```csharp
private static GameManager _instance;
public static GameManager Instance => _instance;
```

## SerializeField

### Components

| 名前 | 型 | 説明 |
|------|-----|------|
| apiServer | ApiServer | APIサーバ参照 |
| dialogUI | OjisanDialogUI | ダイアログUI参照 |
| commandButton | CommandButtonUI | コマンドボタン参照 |
| commandMenu | CommandMenuUI | コマンドメニュー参照 |

### Ojisan Settings

| 名前 | 型 | 説明 |
|------|-----|------|
| greetings | string[] | 挨拶メッセージ配列 |

### Available Commands

| 名前 | 型 | 説明 |
|------|-----|------|
| availableCommands | List<CommandInfo> | 利用可能なコマンドリスト |

## 公開メソッド

### ExecuteCommand

```csharp
public void ExecuteCommand(string commandId, string data = null)
```

指定されたコマンドを実行する。

### ShowOjisanMessage

```csharp
public void ShowOjisanMessage(string message, bool autoHide = true)
```

おじさんにメッセージを表示させる。

### AddCommand

```csharp
public void AddCommand(CommandInfo command)
```

新しいコマンドを追加する。

### RemoveCommand

```csharp
public void RemoveCommand(string commandId)
```

コマンドを削除する。

### SetCommandEnabled

```csharp
public void SetCommandEnabled(string commandId, bool enabled)
```

コマンドの有効/無効を切り替える。

## ライフサイクル

```
Awake()
    │
    ├── シングルトン初期化
    └── デフォルトコマンド初期化

Start()
    │
    ├── イベントハンドラ登録
    └── 挨拶メッセージ表示

OnDestroy()
    │
    └── イベントハンドラ解除
```

## イベント連携

### ApiServer → GameManager

```csharp
apiServer.OnCommandReceived += HandleApiCommand;
```

### CommandButtonUI → GameManager

```csharp
commandButton.OnMainButtonClicked += HandleCommandButtonClick;
```

### CommandMenuUI → GameManager

```csharp
commandMenu.OnCommandSelected += HandleCommandSelected;
```

## アーキテクチャ図

```
┌─────────────────────────────────────────────────────────────┐
│                       GameManager                           │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │
│  │  ApiServer  │  │  DialogUI   │  │  CommandButton/Menu │  │
│  └──────┬──────┘  └──────▲──────┘  └──────────┬──────────┘  │
│         │                │                    │              │
│         │ OnCommand      │ ShowMessage        │ OnCommand    │
│         │ Received       │                    │ Selected     │
│         ▼                │                    ▼              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              ExecuteCommand(commandId)                │   │
│  └──────────────────────────────────────────────────────┘   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────┐
│                     外部システム                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────┐   │
│  │ WindowController │  │ 設定画面 │  │ アプリ終了処理 │      │
│  └──────────────┘  └──────────────┘  └──────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

## 他コンポーネントからの使用

```csharp
// メッセージ表示
GameManager.Instance.ShowOjisanMessage("何か起きたよ！");

// コマンド実行
GameManager.Instance.ExecuteCommand("greet");

// コマンド追加
GameManager.Instance.AddCommand(new CommandInfo("debug", "デバッグ", "デバッグ用"));
```

## 拡張ポイント

### 新規コマンド追加

`ExecuteCommand()` メソッドの switch 文に追加：

```csharp
case "newcommand":
    // 新しい処理
    break;
```

### 外部システム連携

`HandleApiCommand()` で type に応じた分岐を追加：

```csharp
case "external_event":
    HandleExternalEvent(command.data);
    break;
```

## エディタでの設定

1. シーンに空のGameObjectを作成
2. `GameManager` コンポーネントをアタッチ
3. 各UIコンポーネントの参照を設定
4. `Greetings` 配列を編集（オプション）
5. `Available Commands` を編集（オプション）

または、メニューから `DesktopOjisan > Setup Scene` で自動セットアップ。
