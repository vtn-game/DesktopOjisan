# 機能テスト仕様

## テスト対象

| コンポーネント | 優先度 |
|---------------|--------|
| GameManager | 高 |
| ApiServer | 高 |
| OjisanDialogUI | 中 |
| CommandMenuUI | 中 |
| WindowController | 低（手動テスト） |

## テストケース

### GameManager

#### GM-001: 初期化

```
テスト: GameManager が正しく初期化される
前提条件: シーンにGameManagerが配置されている
手順:
  1. シーンを再生する
期待結果:
  - Instance が設定される
  - デフォルトコマンドが登録される
  - 挨拶メッセージが表示される
```

#### GM-002: コマンド実行

```
テスト: ExecuteCommand が正しく動作する
前提条件: GameManager が初期化されている
手順:
  1. ExecuteCommand("talk") を呼び出す
期待結果:
  - ダイアログにメッセージが表示される
```

#### GM-003: コマンド追加・削除

```
テスト: コマンドの動的追加・削除
前提条件: GameManager が初期化されている
手順:
  1. AddCommand(new CommandInfo("test", "テスト", "")) を呼び出す
  2. コマンドメニューを開く
  3. RemoveCommand("test") を呼び出す
期待結果:
  - 追加後: メニューに「テスト」が表示される
  - 削除後: メニューから「テスト」が消える
```

### ApiServer

#### API-001: サーバ起動

```
テスト: サーバが正常に起動する
前提条件: ApiServer がシーンに配置されている
手順:
  1. シーンを再生する
期待結果:
  - IsRunning が true になる
  - ログに「サーバ起動」が出力される
```

#### API-002: Ping エンドポイント

```
テスト: GET /api/ping が応答する
前提条件: サーバが起動している
手順:
  1. curl http://localhost:8080/api/ping を実行
期待結果:
  - ステータスコード 200
  - {"status":"ok","message":"pong"}
```

#### API-003: Message エンドポイント

```
テスト: POST /api/message がメッセージを表示する
前提条件: サーバが起動している
手順:
  1. POST {"message":"テスト"} を送信
期待結果:
  - ダイアログに「テスト」が表示される
```

### OjisanDialogUI

#### DLG-001: メッセージ表示

```
テスト: ShowMessage でメッセージが表示される
前提条件: OjisanDialogUI が配置されている
手順:
  1. ShowMessage("こんにちは") を呼び出す
期待結果:
  - ダイアログパネルが表示される
  - テキストに「こんにちは」が表示される
```

#### DLG-002: タイピングエフェクト

```
テスト: タイピングエフェクトが動作する
前提条件: useTypingEffect が true
手順:
  1. ShowMessage("ABCDE") を呼び出す
期待結果:
  - 文字が1文字ずつ表示される
  - 表示間隔が typingSpeed に従う
```

#### DLG-003: 自動非表示

```
テスト: 自動非表示が動作する
前提条件: autoHideDelay が 5.0
手順:
  1. ShowMessage("テスト", autoHide: true) を呼び出す
  2. 5秒待つ
期待結果:
  - 5秒後にダイアログが非表示になる
```

### CommandMenuUI

#### MENU-001: メニュー開閉

```
テスト: メニューの開閉が動作する
前提条件: CommandMenuUI が配置されている
手順:
  1. Open() を呼び出す
  2. Close() を呼び出す
期待結果:
  - Open後: menuPanel が表示される
  - Close後: menuPanel が非表示になる
```

#### MENU-002: コマンド選択

```
テスト: コマンド選択でイベントが発火する
前提条件: メニューが開いている
手順:
  1. コマンドアイテムをクリックする
期待結果:
  - OnCommandSelected イベントが発火する
  - 選択した CommandInfo が渡される
```

## 手動テスト

### WindowController

| テストID | 内容 | 確認方法 |
|---------|------|---------|
| WIN-001 | ボーダーレス化 | ウィンドウ枠が消える |
| WIN-002 | 常に最前面 | 他のウィンドウの前に表示される |
| WIN-003 | 透明度設定 | ウィンドウが半透明になる |
| WIN-004 | クリック透過 | ウィンドウ越しにクリックできる |
