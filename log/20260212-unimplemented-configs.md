# 未実装構成 調査レポート

調査日: 2026-02-12

## サマリー

| 分類 | 実装済 | 一部実装 | 未実装 |
|------|--------|----------|--------|
| GameParameterDef.toml セクション | 2 | 1 | 2 |
| ReactionDef.toml リアクション | 1 | 0 | 6 |
| コードスペック コンポーネント | 6 | 1 | 5 |
| ユースケース | 5 | 1 | 3 |
| WindowController拡張 | 0 | 0 | 3 |
| レガシースペック機能 | 0 | 0 | 4 |
| **合計** | **14** | **3** | **23** |

---

## 1. GameParameterDef.toml セクション別

### [server] - 実装済
| パラメータ | 定義値 | 実装箇所 | 状態 |
|-----------|--------|---------|------|
| `port = 8080` | 8080 | `Assets/Scripts/Server/ApiServer.cs:16` | 実装済 |
| `auto_start = true` | true | `Assets/Scripts/Server/ApiServer.cs:17` | 実装済 |

### [dialog] - 実装済
| パラメータ | 定義値 | 実装箇所 | 状態 |
|-----------|--------|---------|------|
| `typing_speed = 0.05` | 0.05 | `Assets/Scripts/UI/OjisanDialogUI.cs:20` | 実装済 |
| `auto_hide_delay = 5.0` | 5.0 | `Assets/Scripts/UI/OjisanDialogUI.cs:21` | 実装済 |
| `use_typing_effect = true` | true | `Assets/Scripts/UI/OjisanDialogUI.cs:22` | 実装済 |

### [window] - 一部実装
| パラメータ | 定義値 | 実装箇所 | 状態 |
|-----------|--------|---------|------|
| `default_width = 400` | 400 | `WindowController.cs` に `ResizeWindow()` あるが初期値未設定 | 一部実装 |
| `default_height = 600` | 600 | 同上 | 一部実装 |
| `default_transparency = 255` | 255 | `Assets/Scripts/WindowController.cs:58` | 実装済 |
| `always_on_top = true` | true | `Assets/Scripts/WindowController.cs:56` | 実装済 |
| `click_through = false` | false | `Assets/Scripts/WindowController.cs:57` | 実装済 |

### [ojisan] - 未実装
| パラメータ | 定義値 | 状態 |
|-----------|--------|------|
| `affection_initial = 0` | コメントアウト | 未実装 - AffectionSystemクラスが存在しない |
| `affection_max = 100` | コメントアウト | 未実装 |
| `affection_decay_rate = 0.1` | コメントアウト | 未実装 |

### [reaction] - 未実装
| パラメータ | 定義値 | 状態 |
|-----------|--------|------|
| `steam_play_threshold_hours = 3` | コメントアウト | 未実装 - ReactionSystem/SteamIntegrationが存在しない |
| `commit_warning_hours = 4` | コメントアウト | 未実装 - GitIntegrationが存在しない |

---

## 2. ReactionDef.toml リアクション別

### 実装済
| リアクション | トリガー | 実装箇所 |
|-------------|---------|---------|
| `reaction.greet.default` | アプリ起動・greetコマンド | `GameManager.cs:21-27, 110-117` |

### 未実装
| リアクション | トリガー | 必要なコンポーネント |
|-------------|---------|-------------------|
| `reaction.angry.ctrl_v` | Ctrl+V検知 | ExternalEventReceiver, VSCode連携 |
| `reaction.angry.steam_long_play` | Steamゲームプレイ3時間超過 | SteamIntegration, ReactionSystem |
| `reaction.angry.no_commit` | 長時間コミットなし | GitIntegration, ReactionSystem |
| `reaction.happy.coffee` | コーヒーをあげる | GameManager に `case "coffee"` が無い |
| `reaction.happy.show_commit_log` | コミットログ表示 | GitIntegration |
| `reaction.happy.commit` | コミット検知 | GitIntegration, GameManager に `case "commit"` が無い |

---

## 3. コードスペック コンポーネント

### 実装済
| コンポーネント | ファイル |
|--------------|---------|
| GameManager | `Assets/Scripts/Core/GameManager.cs` |
| ApiServer | `Assets/Scripts/Server/ApiServer.cs` |
| OjisanDialogUI | `Assets/Scripts/UI/OjisanDialogUI.cs` |
| CommandMenuUI | `Assets/Scripts/UI/CommandMenuUI.cs` |
| CommandButtonUI | `Assets/Scripts/UI/CommandButtonUI.cs` |
| WindowController (基本機能) | `Assets/Scripts/WindowController.cs` |

### 一部実装
| コンポーネント | 状態 | 詳細 |
|--------------|------|------|
| WindowController (拡張) | 一部実装 | Minimize/ドラッグ移動/画面端スナップが未実装 |

### 未実装 (ファイル自体が存在しない)
| コンポーネント | スペック参照 |
|--------------|------------|
| **ReactionSystem** | `spec/TODO.md:62`, `spec/03_reaction_system.md` |
| **AffectionSystem** | `spec/TODO.md:63`, `spec/gamedesign/ojisan.md:56-75` |
| **ExternalEventReceiver** | `spec/TODO.md:64` |
| **SteamIntegration** | `spec/TODO.md:65`, `spec/07_external_integrations.md:9-12` |
| **GitIntegration** | `spec/TODO.md:66`, `spec/07_external_integrations.md:14-18` |

---

## 4. ユースケース

### 実装済
| UC | 概要 | 実装箇所 |
|----|------|---------|
| UC-001 | アプリ起動時の挨拶 | `GameManager.cs:50, 110-117` |
| UC-002 | コマンドメニュー表示 | `GameManager.cs`, `CommandMenuUI.cs`, `CommandButtonUI.cs` |
| UC-006 | アプリ終了 | `GameManager.cs:176-178, 241-250` |
| UC-API-001 | ヘルスチェック (ping/status) | `ApiServer.cs:139-142` |
| UC-API-002 | メッセージ送信 | `ApiServer.cs:189-214` |
| UC-API-003 | コマンド実行 | `ApiServer.cs:167-187`, `GameManager.cs` |

### 一部実装
| UC | 概要 | 状態 |
|----|------|------|
| UC-003 | おじさんの会話 | タイピングエフェクト実装済。ダイアログクリックでのスキップトリガーが未接続 |

### 未実装
| UC | 概要 | 必要コンポーネント |
|----|------|-------------------|
| UC-004 | 怒りリアクション (Ctrl+V, Steam) | ReactionSystem, ExternalEventReceiver, SteamIntegration |
| UC-005 | 喜びリアクション (コミット, コーヒー) | ReactionSystem, GitIntegration |
| UC-API-004 | VSCode連携 | VSCode拡張, GameManagerに `case "commit"` 追加 |

---

## 5. WindowController 拡張機能

| 機能 | 状態 | 詳細 |
|------|------|------|
| **Minimize** | 未実装 | `GameManager.cs:173` にコメント `// WindowController.Minimize() を呼び出す` あり。メソッド/DLLインポート未実装 |
| **ドラッグ移動** | 未実装 | `MoveWindow()` APIはあるが、マウスドラッグハンドラが未実装 |
| **画面端スナップ** | 未実装 | コード一切なし。`GetScreenDimensions()` は利用可能 |

---

## 6. レガシースペック機能 (完全未実装)

| 機能 | スペック | 概要 |
|------|---------|------|
| **イベントシステム** | `spec/04_event_system.md` | 焼き鳥イベント、自慢話イベント、豆知識イベント |
| **練習問題機能** | `spec/05_practice_problems.md` | Paiza風練習問題出題、FizzBuzz導入イベント |
| **開発時間トラッキング** | `spec/06_dev_time_tracking.md` | 開発時間計測、作業効率可視化 |
| **VSCode連携 (キーボード監視)** | `spec/07_external_integrations.md:5-8` | VSCode使用時のキーボード入力監視 |

---

## 7. その他未実装項目

| 項目 | 参照 | 状態 |
|------|------|------|
| 設定画面 | `GameManager.cs:164` `"設定画面はまだ準備中なんだ。"` | スタブのみ |
| キャラクターアニメーション (idle, talk, angry, happy, sleep) | `spec/TODO.md:17-22` | 未実装 |
| キャラクタービジュアルデザイン | `spec/gamedesign/ojisan.md:14` | 未実装 |
| 表情パターン (normal, angry, happy, disappointed, proud) | `spec/gamedesign/ojisan.md:18-24` | 未実装 |
| 好感度に応じたセリフ変化 | `spec/TODO.md:9` | 未実装 |
| 入力システム定義 (Input.md) | `spec/TODO.md:33-35` | 未実装 |
| アクセシビリティ機能 | `spec/TODO.md:40-43` | 未実装 |
| 自動テスト | `spec/TODO.md:54-57` | 未実装 |
| ビルドスクリプト / インストーラー | `spec/TODO.md:76-78` | 未実装 |

---

## 結論

プロジェクトの**コアインフラ**（ウィンドウ制御、APIサーバ、UIシステム、コマンドメニュー、挨拶機能）は実装済み。

**ゲーム層**に相当する以下の機能群が未実装:
1. **リアクションシステム全体** - 怒り/喜びの自動反応
2. **好感度システム** - ユーザー行動に応じた好感度変動
3. **外部連携** - Steam/Git/VSCode統合
4. **イベントシステム** - 焼き鳥/自慢話/豆知識
5. **練習問題機能** - FizzBuzz等の出題
6. **開発時間トラッキング** - 作業時間計測
7. **WindowController拡張** - Minimize/ドラッグ/スナップ
8. **アニメーション/表情** - キャラクター視覚表現
