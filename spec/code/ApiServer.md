# ApiServer

外部アプリケーションからコマンドを受け付けるHTTPサーバ。

## クラス定義

```csharp
namespace DesktopOjisan.Server

public class ApiServer : MonoBehaviour
```

## 責務

- HTTPリクエストの受信・レスポンス
- コマンドのキューイング（スレッドセーフ）
- メインスレッドへのコマンド転送

## 技術仕様

| 項目 | 値 |
|------|-----|
| 実装 | System.Net.HttpListener |
| ポート | 8080（デフォルト） |
| プロトコル | HTTP/1.1 |
| データ形式 | JSON |

## エンドポイント

| メソッド | パス | 説明 |
|---------|------|------|
| GET | /api/ping | ヘルスチェック |
| GET | /api/status | サーバ状態取得 |
| POST | /api/message | メッセージ表示 |
| POST | /api/command | コマンド実行 |

## スレッドモデル

```
ListenerThread (別スレッド)
    │
    ▼ HTTPリクエスト受信
ConcurrentQueue<ApiCommand>
    │
    ▼ Update() でデキュー
MainThread
    │
    ▼ OnCommandReceived イベント発火
GameManager
```

## 公開インターフェース

### StartServer / StopServer

```csharp
public void StartServer()
public void StopServer()
```

### イベント

```csharp
public event Action<ApiCommand> OnCommandReceived;
```

## データ構造

```csharp
[Serializable]
public class ApiCommand
{
    public string type;  // コマンドタイプ
    public string data;  // 追加データ
}
```

## 対応ファイル

- `Assets/Scripts/Server/ApiServer.cs`
