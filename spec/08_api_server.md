# APIサーバ仕様

## 概要

外部アプリケーション（VSCode拡張、CLIツールなど）からおじさんを操作するためのHTTP APIサーバ。

## 技術仕様

- **実装**: C# HttpListener
- **ポート**: 8080（デフォルト）
- **プロトコル**: HTTP/1.1
- **データ形式**: JSON

## ファイル構成

```
Assets/Scripts/Server/
└── ApiServer.cs
```

## クラス設計

### ApiServer

```csharp
namespace DesktopOjisan.Server

public class ApiServer : MonoBehaviour
```

#### プロパティ

| 名前 | 型 | 説明 |
|------|-----|------|
| port | int | リスニングポート（デフォルト: 8080） |
| autoStart | bool | 自動起動フラグ |
| Port | int | 現在のポート（読み取り専用） |
| IsRunning | bool | サーバ稼働状態 |

#### イベント

| 名前 | シグネチャ | 説明 |
|------|-----------|------|
| OnCommandReceived | Action<ApiCommand> | コマンド受信時に発火 |

#### メソッド

| 名前 | 説明 |
|------|------|
| StartServer() | サーバを起動 |
| StopServer() | サーバを停止 |

## APIエンドポイント

### GET /api/ping

ヘルスチェック用エンドポイント。

**レスポンス**
```json
{
  "status": "ok",
  "message": "pong"
}
```

### GET /api/status

サーバ状態の取得。

**レスポンス**
```json
{
  "status": "running",
  "port": 8080
}
```

### POST /api/message

おじさんにメッセージを表示させる。

**リクエストボディ**
```json
{
  "message": "表示するメッセージ"
}
```

**レスポンス**
```json
{
  "status": "ok"
}
```

### POST /api/command

コマンドを実行する。

**リクエストボディ**
```json
{
  "type": "コマンドタイプ",
  "data": "追加データ（オプション）"
}
```

**レスポンス**
```json
{
  "status": "ok",
  "command": "実行されたコマンド"
}
```

## データ構造

### ApiCommand

```csharp
[Serializable]
public class ApiCommand
{
    public string type;  // コマンドタイプ
    public string data;  // 追加データ
}
```

### MessageCommand

```csharp
[Serializable]
public class MessageCommand
{
    public string message;  // 表示メッセージ
}
```

## スレッドモデル

```
┌─────────────────┐
│  ListenerThread │  ← HTTPリクエスト受信
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  CommandQueue   │  ← ConcurrentQueue でスレッドセーフ
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Main Thread   │  ← Update() でキューを処理
└─────────────────┘
```

## 使用例

### curl コマンド

```bash
# Ping
curl http://localhost:8080/api/ping

# メッセージ送信
curl -X POST -H "Content-Type: application/json" \
  -d '{"message":"こんにちは！"}' \
  http://localhost:8080/api/message

# コマンド実行
curl -X POST -H "Content-Type: application/json" \
  -d '{"type":"greet"}' \
  http://localhost:8080/api/command
```

### PowerShell

```powershell
# メッセージ送信
$body = @{ message = "テスト" } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:8080/api/message" -Method Post -Body $body -ContentType "application/json"
```

## CORS対応

すべてのレスポンスに以下のヘッダーを付与：

```
Access-Control-Allow-Origin: *
```

## エラーハンドリング

| ステータスコード | 状況 |
|-----------------|------|
| 200 | 成功 |
| 404 | エンドポイントが存在しない |
| 500 | サーバ内部エラー |

エラーレスポンス形式：
```json
{
  "error": "エラーメッセージ"
}
```
