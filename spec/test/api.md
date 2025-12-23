# APIテスト仕様

## テスト環境

- サーバ: localhost:8080
- ツール: curl, PowerShell, または任意のHTTPクライアント

## エンドポイントテスト

### GET /api/ping

```bash
# リクエスト
curl http://localhost:8080/api/ping

# 期待レスポンス
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"status":"ok","message":"pong"}
```

### GET /api/status

```bash
# リクエスト
curl http://localhost:8080/api/status

# 期待レスポンス
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"status":"running","port":8080}
```

### POST /api/message

#### 正常系

```bash
# リクエスト
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"message":"テストメッセージ"}' \
  http://localhost:8080/api/message

# 期待レスポンス
HTTP/1.1 200 OK
{"status":"ok"}
```

#### 異常系: 空メッセージ

```bash
# リクエスト
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"message":""}' \
  http://localhost:8080/api/message

# 期待レスポンス
HTTP/1.1 200 OK
{"error":"Invalid message format"}
```

#### 異常系: GETメソッド

```bash
# リクエスト
curl http://localhost:8080/api/message

# 期待レスポンス
HTTP/1.1 200 OK
{"error":"POST method required"}
```

### POST /api/command

#### 正常系: greet

```bash
# リクエスト
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"type":"greet"}' \
  http://localhost:8080/api/command

# 期待レスポンス
HTTP/1.1 200 OK
{"status":"ok","command":"greet"}
```

#### 正常系: custom with data

```bash
# リクエスト
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"type":"custom","data":"カスタムメッセージ"}' \
  http://localhost:8080/api/command

# 期待レスポンス
HTTP/1.1 200 OK
{"status":"ok","command":"custom"}
```

#### 異常系: 不正なJSON

```bash
# リクエスト
curl -X POST \
  -H "Content-Type: application/json" \
  -d 'invalid json' \
  http://localhost:8080/api/command

# 期待レスポンス
HTTP/1.1 200 OK
{"error":"Invalid command format"}
```

### 404 Not Found

```bash
# リクエスト
curl http://localhost:8080/api/unknown

# 期待レスポンス
HTTP/1.1 404 Not Found
{"error":"Not found"}
```

## 負荷テスト

### 連続リクエスト

```bash
# 100回連続でpingを送信
for i in {1..100}; do
  curl -s http://localhost:8080/api/ping > /dev/null
done
```

期待: すべて正常に応答する

### 並列リクエスト

```bash
# 10並列でリクエスト
seq 1 10 | xargs -P 10 -I {} curl -s http://localhost:8080/api/ping
```

期待: すべて正常に応答する

## PowerShell テストスクリプト

```powershell
# Tools/test_api.ps1 を使用
.\Tools\test_api.ps1
```

## チェックリスト

| テストID | 内容 | 結果 |
|---------|------|------|
| API-T01 | GET /api/ping が 200 を返す | □ |
| API-T02 | GET /api/status が 200 を返す | □ |
| API-T03 | POST /api/message が成功する | □ |
| API-T04 | POST /api/command が成功する | □ |
| API-T05 | 不正なエンドポイントが 404 を返す | □ |
| API-T06 | CORSヘッダーが付与される | □ |
| API-T07 | UTF-8日本語が正しく処理される | □ |
| API-T08 | 連続リクエストに耐える | □ |
