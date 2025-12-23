# コマンドシステム仕様

## 概要

おじさんに対して実行できるコマンドの管理・実行システム。
UI選択とAPI経由の両方から統一的に処理する。

## コマンドの流れ

```
┌──────────────────┐     ┌──────────────────┐
│  CommandMenuUI   │     │    ApiServer     │
│  (UI選択)        │     │  (外部API)       │
└────────┬─────────┘     └────────┬─────────┘
         │                        │
         │ CommandInfo            │ ApiCommand
         ▼                        ▼
┌─────────────────────────────────────────────┐
│              GameManager                     │
│                                             │
│  ExecuteCommand(commandId, data)            │
└─────────────────────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────────────┐
│              各コマンド処理                  │
└─────────────────────────────────────────────┘
```

## デフォルトコマンド一覧

| ID | 表示名 | 説明 | 動作 |
|----|--------|------|------|
| talk | おしゃべり | おじさんと会話する | 会話メッセージ表示 |
| status | ステータス確認 | 現在の状態を確認 | サーバ状態表示 |
| settings | 設定 | アプリの設定を開く | 設定画面（未実装） |
| help | ヘルプ | 使い方を表示 | ヘルプメッセージ表示 |
| minimize | 最小化 | ウィンドウを最小化 | ウィンドウ最小化 |
| exit | 終了 | アプリを終了 | アプリ終了 |

## 特殊コマンド

### greet

挨拶メッセージをランダムに表示。

```json
{ "type": "greet" }
```

### message

APIからのみ使用可能。任意のメッセージを表示。

```json
{ "type": "message", "data": "表示するメッセージ" }
```

### custom

カスタムメッセージを表示。

```json
{ "type": "custom", "data": "カスタムメッセージ" }
```

## コマンドの追加

### コードから追加

```csharp
var newCommand = new CommandInfo("mycommand", "マイコマンド", "説明文");
GameManager.Instance.AddCommand(newCommand);
```

### インスペクターから追加

GameManager の `Available Commands` リストに追加。

## コマンドの削除

```csharp
GameManager.Instance.RemoveCommand("mycommand");
```

## コマンドの有効/無効切り替え

```csharp
GameManager.Instance.SetCommandEnabled("settings", false);
```

## 新規コマンドの実装

1. `GameManager.ExecuteCommand()` にcase文を追加

```csharp
case "mycommand":
    // 処理を記述
    ShowOjisanMessage("マイコマンドが実行されました！");
    break;
```

2. デフォルトコマンドリストに追加（オプション）

```csharp
new CommandInfo("mycommand", "マイコマンド", "説明文")
```

## API経由でのコマンド実行

### リクエスト

```bash
curl -X POST -H "Content-Type: application/json" \
  -d '{"type":"talk"}' \
  http://localhost:8080/api/command
```

### レスポンス

```json
{
  "status": "ok",
  "command": "talk"
}
```

## 未知のコマンド

登録されていないコマンドIDが指定された場合：

```
「{commandId}」... そのコマンドは知らないなぁ。
```

## コマンド実行のタイミング

| トリガー | 処理タイミング |
|----------|----------------|
| UI選択 | 即時（メインスレッド） |
| API受信 | 次のUpdate()（キュー経由） |

## おじさんの挨拶メッセージ

`greet` コマンドで使用されるランダムメッセージ：

- 「やあ、今日も頑張ってるね！」
- 「調子はどうだい？」
- 「何か手伝おうか？」
- 「おっ、また会えたね！」

インスペクターの `Greetings` 配列で編集可能。
