# API利用シナリオ

## ユースケース一覧

### UC-API-001: ヘルスチェック

```gherkin
Feature: APIヘルスチェック
  As a 外部アプリケーション
  I want to APIサーバの稼働状態を確認する
  So that 連携可能かどうか判断できる

  Scenario: Ping
    Given APIサーバが起動している
    When GET /api/ping にリクエストを送る
    Then ステータスコード 200 が返る
    And {"status":"ok","message":"pong"} が返る

  Scenario: Status
    Given APIサーバがポート8080で起動している
    When GET /api/status にリクエストを送る
    Then ステータスコード 200 が返る
    And {"status":"running","port":8080} が返る
```

### UC-API-002: メッセージ送信

```gherkin
Feature: メッセージ送信
  As a 外部アプリケーション
  I want to おじさんにメッセージを表示させる
  So that ユーザーに通知を届けられる

  Scenario: 正常なメッセージ送信
    Given APIサーバが起動している
    When POST /api/message に {"message":"こんにちは"} を送る
    Then ステータスコード 200 が返る
    And おじさんが「こんにちは」と表示する

  Scenario: 不正なリクエスト
    Given APIサーバが起動している
    When POST /api/message に {} を送る
    Then ステータスコード 200 が返る（エラー内容を含む）
    And {"error":"Invalid message format"} が返る
```

### UC-API-003: コマンド実行

```gherkin
Feature: コマンド実行
  As a 外部アプリケーション
  I want to おじさんにコマンドを実行させる
  So that 様々な操作を自動化できる

  Scenario: greetコマンド
    Given APIサーバが起動している
    When POST /api/command に {"type":"greet"} を送る
    Then ステータスコード 200 が返る
    And おじさんがランダムな挨拶を表示する

  Scenario: customコマンド
    Given APIサーバが起動している
    When POST /api/command に {"type":"custom","data":"カスタムメッセージ"} を送る
    Then おじさんが「カスタムメッセージ」と表示する

  Scenario: 未知のコマンド
    Given APIサーバが起動している
    When POST /api/command に {"type":"unknown"} を送る
    Then おじさんが「そのコマンドは知らないなぁ」と表示する
```

### UC-API-004: VSCode連携（将来）

```gherkin
Feature: VSCode連携
  As a VSCode拡張
  I want to コミット時におじさんに通知する
  So that ユーザーを褒められる

  Scenario: コミット通知
    Given VSCode拡張がインストールされている
    And APIサーバが起動している
    When ユーザーがGitコミットを行う
    Then VSCode拡張がPOST /api/command に {"type":"commit"} を送る
    And おじさんが褒めるメッセージを表示する
```

### UC-API-005: CLI連携

```gherkin
Feature: CLI連携
  As a 開発者
  I want to コマンドラインからおじさんにメッセージを送る
  So that スクリプトから通知できる

  Scenario: curlでメッセージ送信
    Given APIサーバが起動している
    When curl -X POST -d '{"message":"ビルド完了"}' http://localhost:8080/api/message
    Then おじさんが「ビルド完了」と表示する

  Scenario: PowerShellでコマンド実行
    Given APIサーバが起動している
    When Invoke-RestMethod でgreetコマンドを送る
    Then おじさんが挨拶を表示する
```

## エラーケース

### UC-API-ERR-001: サーバ未起動

```gherkin
Scenario: サーバ未起動時のリクエスト
  Given APIサーバが起動していない
  When リクエストを送る
  Then 接続エラーが発生する
```

### UC-API-ERR-002: 不正なエンドポイント

```gherkin
Scenario: 存在しないエンドポイント
  Given APIサーバが起動している
  When GET /api/unknown にリクエストを送る
  Then ステータスコード 404 が返る
  And {"error":"Not found"} が返る
```
