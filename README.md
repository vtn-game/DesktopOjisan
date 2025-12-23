# デスクトップおじさん

うざいけど憎めないおじさんと一緒に開発しよう。

## 概要

「[Chill with You](https://store.steampowered.com/app/3548580/Chill_with_You__LoFi_Story/)」の**完全逆張り**デスクトップマスコットアプリ。

癒し系マスコットの対極として、開発者の行動を監視して怒ったり褒めたりするおじさんがデスクトップに常駐します。

### 特徴

- コピペ（Ctrl+V）すると怒る
- Steamで長時間ゲームすると怒る
- コミットすると褒めてくれる
- コーヒーをあげると喜ぶ
- 外部ツール（VSCode拡張など）と連携可能なAPIサーバ内蔵

## 動作環境

- **OS**: Windows 10/11
- **Unity**: 2022.3 LTS 以降
- **ビルドツール**: Visual Studio 2022（NativePlugin用）

## プロジェクト構成

```
DesktopOjisan/
├── Assets/
│   ├── Plugins/x86_64/      # NativePlugin DLL
│   ├── Prefabs/             # プレハブ
│   └── Scripts/
│       ├── Core/            # GameManager など
│       ├── Server/          # APIサーバ
│       ├── UI/              # UI関連
│       └── Editor/          # エディタ拡張
├── NativePlugin/            # C++ ウィンドウ制御
│   ├── WindowController.cpp
│   ├── CMakeLists.txt
│   └── build.bat
├── spec/                    # 仕様書
│   ├── README.md            # 仕様書インデックス
│   ├── TODO.md              # 未実装項目
│   ├── code/                # コード設計
│   ├── gamedesign/          # ゲームデザイン
│   ├── rule/                # プロジェクトルール
│   ├── ubi/                 # ユビキタス言語
│   ├── usecase/             # ユースケース
│   ├── ux/                  # UXデザイン
│   └── test/                # テスト仕様
└── Tools/                   # 開発ツール
    ├── test_api.ps1         # APIテスト（PowerShell）
    └── test_api.bat         # APIテスト（バッチ）
```

## セットアップ

### 1. リポジトリをクローン

```bash
git clone <repository-url>
cd DesktopOjisan
```

### 2. NativePlugin をビルド

```bash
cd NativePlugin
./build.bat
```

> 要件: Visual Studio 2022, CMake

### 3. Unity でプロジェクトを開く

1. Unity Hub でプロジェクトを追加
2. Unity 2022.3 LTS で開く
3. TextMeshPro をインポート（初回のみ）

### 4. シーンをセットアップ

Unity エディタで:

```
メニュー > DesktopOjisan > Setup Scene
```

これで必要なオブジェクトが自動生成されます。

### 5. 実行

Play ボタンを押すと、おじさんが起動します。

## APIサーバ

アプリ起動時に `http://localhost:8080` でAPIサーバが立ち上がります。

### エンドポイント

| メソッド | パス | 説明 |
|---------|------|------|
| GET | /api/ping | ヘルスチェック |
| GET | /api/status | サーバ状態 |
| POST | /api/message | メッセージ表示 |
| POST | /api/command | コマンド実行 |

### 使用例

```bash
# メッセージを表示
curl -X POST -H "Content-Type: application/json" \
  -d '{"message":"ビルド完了！"}' \
  http://localhost:8080/api/message

# コマンドを実行
curl -X POST -H "Content-Type: application/json" \
  -d '{"type":"greet"}' \
  http://localhost:8080/api/command
```

### 利用可能なコマンド

| type | 説明 |
|------|------|
| talk | おしゃべり |
| status | ステータス確認 |
| greet | 挨拶 |
| help | ヘルプ |
| exit | 終了 |

## 開発

### コーディング規約

- 詳細: [spec/rule/coding.md](spec/rule/coding.md)
- namespace: `DesktopOjisan.{カテゴリ}`
- 1ファイル1クラス

### 仕様書

- [spec/README.md](spec/README.md) - 仕様書インデックス
- [spec/TODO.md](spec/TODO.md) - 未実装項目

### テスト

```powershell
# APIテスト
./Tools/test_api.ps1
```

## ライセンス

TBD

## 謝辞

- コンセプト参考: [Chill with You : Lo-Fi Story](https://store.steampowered.com/app/3548580/Chill_with_You__LoFi_Story/)
