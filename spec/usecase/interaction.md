# おじさんとのインタラクション

## ユースケース一覧

### UC-001: アプリ起動時の挨拶

```gherkin
Feature: アプリ起動時の挨拶
  As a ユーザー
  I want to アプリ起動時におじさんから挨拶される
  So that おじさんの存在を認識できる

  Scenario: 正常起動
    Given アプリが起動した
    When GameManager.Start() が呼ばれる
    Then おじさんがランダムな挨拶メッセージを表示する
    And ダイアログは5秒後に自動で消える
```

### UC-002: コマンドメニューの表示

```gherkin
Feature: コマンドメニュー
  As a ユーザー
  I want to コマンドボタンを押してメニューを開く
  So that おじさんに指示を出せる

  Scenario: メニューを開く
    Given コマンドボタンが表示されている
    When ユーザーがコマンドボタンをクリックする
    Then コマンドメニューが表示される
    And 利用可能なコマンド一覧が表示される

  Scenario: コマンドを選択
    Given コマンドメニューが開いている
    When ユーザーが「おしゃべり」を選択する
    Then おじさんが反応する
    And コマンドメニューが閉じる
```

### UC-003: おじさんの会話

```gherkin
Feature: おじさんの会話
  As a ユーザー
  I want to おじさんと会話する
  So that おじさんとの交流を楽しめる

  Scenario: タイピングエフェクト
    Given おじさんにメッセージが渡される
    When ShowMessage() が呼ばれる
    Then メッセージが1文字ずつ表示される
    And すべて表示されたら自動で消える

  Scenario: スキップ
    Given おじさんがタイピング中
    When ユーザーがダイアログをクリックする
    Then 残りのメッセージが即座に表示される
```

### UC-004: リアクション（怒り）

```gherkin
Feature: 怒りリアクション
  As a システム
  I want to 特定の行動を検知しておじさんを怒らせる
  So that ユーザーに警告を与えられる

  Scenario: Ctrl+V検知（TODO: 未実装）
    Given ユーザーがコーディング中
    When ユーザーがCtrl+Vを押す
    Then おじさんが怒りメッセージを表示する

  Scenario: Steam長時間プレイ（TODO: 未実装）
    Given ユーザーがSteamでゲームをプレイ中
    When プレイ時間が3時間を超える
    Then おじさんが警告メッセージを表示する
```

### UC-005: リアクション（喜び）

```gherkin
Feature: 喜びリアクション
  As a システム
  I want to 良い行動を検知しておじさんを喜ばせる
  So that ユーザーにポジティブなフィードバックを与えられる

  Scenario: コミット検知（TODO: 未実装）
    Given ユーザーが開発中
    When ユーザーがGitコミットを行う
    Then おじさんが褒めるメッセージを表示する

  Scenario: コーヒーをあげる
    Given おじさんが表示されている
    When ユーザーがコーヒーコマンドを実行する
    Then おじさんが喜ぶメッセージを表示する
```

### UC-006: アプリ終了

```gherkin
Feature: アプリ終了
  As a ユーザー
  I want to おじさんアプリを終了する
  So that デスクトップをすっきりさせられる

  Scenario: 終了コマンド
    Given コマンドメニューが表示されている
    When ユーザーが「終了」を選択する
    Then おじさんが「またね！」と言う
    And 2秒後にアプリが終了する
```
