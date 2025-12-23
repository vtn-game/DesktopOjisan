# デスクトップおじさん 仕様書

「チルウィズユー」の逆張り - うざいおじさんと仲良くなるデスクトップマスコットアプリ

## ディレクトリ構造

```
spec/
├── README.md                 # このファイル（インデックス）
├── TODO.md                   # 未定義・未実装項目一覧
├── code/                     # コードアーキテクチャ仕様
│   ├── GameManager.md        # ゲーム全体の管理
│   ├── ApiServer.md          # HTTP APIサーバ
│   ├── WindowController.md   # ウィンドウ制御（NativePlugin）
│   └── UI/
│       ├── OjisanDialogUI.md # 会話表示UI
│       ├── CommandButtonUI.md # コマンドボタン
│       └── CommandMenuUI.md  # コマンドメニュー
├── gamedesign/               # ゲームデザイン・パラメータ
│   ├── GameParameterDef.toml # ゲームパラメータ定義
│   ├── ReactionDef.toml      # リアクション定義
│   └── ojisan.md             # おじさんキャラクター仕様
├── rule/                     # プロジェクトルール
│   ├── general.md            # 一般ルール
│   ├── coding.md             # コーディング規約
│   └── gamerule.md           # ゲームルール
├── ubi/                      # ユビキタス言語（ドメイン用語）
│   └── ubiquitous.yaml       # 用語辞書
├── usecase/                  # ユースケース（振る舞い仕様）
│   ├── interaction.md        # おじさんとのインタラクション
│   └── api_usage.md          # API利用シナリオ
├── ux/                       # UXデザイン
│   └── ux_design.md          # コンセプト・体験設計
└── test/                     # テスト仕様
    ├── feature.md            # 機能テスト
    ├── api.md                # APIテスト
    └── performance.md        # パフォーマンス基準
```

## 旧仕様書（参照用）

以下は初期の設計ドキュメント。新構造への移行後も参照用に保持。

| ファイル | 内容 |
|---------|------|
| 01_overview.md | 概要・コンセプト |
| 02_game_flow.md | ゲームフロー |
| 03_reaction_system.md | リアクションシステム |
| 04_event_system.md | イベントシステム |
| 05_practice_problems.md | 練習問題 |
| 06_dev_time_tracking.md | 開発時間追跡 |
| 07_external_integrations.md | 外部連携 |
| 08_api_server.md | APIサーバ仕様 |
| 09_ui_system.md | UIシステム仕様 |
| 10_command_system.md | コマンドシステム仕様 |
| 11_game_manager.md | GameManager仕様 |

## クイックリンク

- **未定義項目を確認**: [TODO.md](TODO.md)
- **実装を始める**: [code/GameManager.md](code/GameManager.md)
- **ゲームルールを知る**: [rule/gamerule.md](rule/gamerule.md)
- **用語を確認する**: [ubi/ubiquitous.yaml](ubi/ubiquitous.yaml)
- **UXコンセプト**: [ux/ux_design.md](ux/ux_design.md)
