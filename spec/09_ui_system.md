# UIシステム仕様

## 概要

おじさんとのインタラクションを行うためのUIシステム。会話表示、コマンドボタン、メニューで構成。

## ファイル構成

```
Assets/Scripts/UI/
├── OjisanDialogUI.cs    # 会話文表示
├── CommandButtonUI.cs   # コマンドボタン
└── CommandMenuUI.cs     # コマンドメニュー
```

---

## OjisanDialogUI

おじさんの会話文を表示するダイアログUI。

### クラス定義

```csharp
namespace DesktopOjisan.UI

public class OjisanDialogUI : MonoBehaviour
```

### SerializeField

| 名前 | 型 | 説明 |
|------|-----|------|
| dialogPanel | GameObject | ダイアログパネル |
| dialogText | TextMeshProUGUI | テキスト表示 |
| ojisanIcon | Image | おじさんアイコン |
| typingSpeed | float | タイピング速度（秒/文字） |
| autoHideDelay | float | 自動非表示までの時間 |
| useTypingEffect | bool | タイピングエフェクト有効化 |

### プロパティ

| 名前 | 型 | 説明 |
|------|-----|------|
| IsShowing | bool | 表示中かどうか |

### イベント

| 名前 | シグネチャ | 説明 |
|------|-----------|------|
| OnDialogComplete | Action | タイピング完了時 |
| OnDialogHidden | Action | ダイアログ非表示時 |

### メソッド

| 名前 | 引数 | 説明 |
|------|------|------|
| ShowMessage | string message, bool autoHide | メッセージを表示 |
| Hide | - | ダイアログを非表示 |
| SkipTyping | - | タイピングをスキップ |
| SetOjisanIcon | Sprite icon | アイコンを設定 |

### 動作フロー

```
ShowMessage()
    │
    ├── useTypingEffect == true
    │       │
    │       ▼
    │   TypeText() コルーチン
    │       │
    │       ▼ 1文字ずつ表示
    │   OnDialogComplete
    │       │
    │       ▼ autoHide == true
    │   AutoHide() → Hide()
    │
    └── useTypingEffect == false
            │
            ▼ 即時表示
        OnDialogComplete
```

---

## CommandButtonUI

コマンドメニューを開くためのメインボタン。

### クラス定義

```csharp
namespace DesktopOjisan.UI

public class CommandButtonUI : MonoBehaviour
```

### SerializeField

| 名前 | 型 | 説明 |
|------|-----|------|
| mainButton | Button | ボタンコンポーネント |
| mainButtonText | TextMeshProUGUI | ボタンラベル |
| mainButtonIcon | Image | ボタンアイコン |
| buttonLabel | string | 表示ラベル |

### イベント

| 名前 | シグネチャ | 説明 |
|------|-----------|------|
| OnMainButtonClicked | Action | ボタンクリック時 |

### メソッド

| 名前 | 引数 | 説明 |
|------|------|------|
| SetButtonLabel | string label | ラベルを変更 |
| SetInteractable | bool interactable | 有効/無効を切り替え |
| SetIcon | Sprite icon | アイコンを設定 |

---

## CommandMenuUI

利用可能なコマンド一覧を表示するメニュー。

### クラス定義

```csharp
namespace DesktopOjisan.UI

public class CommandMenuUI : MonoBehaviour
```

### SerializeField

| 名前 | 型 | 説明 |
|------|-----|------|
| menuPanel | GameObject | メニューパネル |
| commandListParent | Transform | コマンドリストの親 |
| commandItemPrefab | GameObject | コマンドアイテムプレハブ |
| closeButton | Button | 閉じるボタン |
| closeOnCommandSelect | bool | 選択時に自動で閉じる |

### プロパティ

| 名前 | 型 | 説明 |
|------|-----|------|
| IsOpen | bool | メニューが開いているか |

### イベント

| 名前 | シグネチャ | 説明 |
|------|-----------|------|
| OnCommandSelected | Action<CommandInfo> | コマンド選択時 |
| OnMenuClosed | Action | メニューを閉じた時 |

### メソッド

| 名前 | 引数 | 説明 |
|------|------|------|
| Open | List<CommandInfo> commands | メニューを開く |
| Close | - | メニューを閉じる |
| Toggle | List<CommandInfo> commands | 開閉をトグル |

---

## CommandInfo

コマンドの情報を保持するデータクラス。

```csharp
[Serializable]
public class CommandInfo
{
    public string id;           // コマンドID
    public string displayName;  // 表示名
    public string description;  // 説明文
    public Sprite icon;         // アイコン
    public bool isEnabled;      // 有効フラグ
}
```

---

## UI階層構造

```
Canvas
├── OjisanDialog
│   └── DialogPanel
│       ├── OjisanIcon (Image)
│       └── DialogText (TextMeshProUGUI)
│
├── CommandButton
│   ├── ButtonIcon (Image)
│   └── ButtonText (TextMeshProUGUI)
│
└── CommandMenu
    └── MenuPanel
        ├── CloseButton
        └── CommandList (VerticalLayoutGroup)
            ├── CommandItem
            ├── CommandItem
            └── ...
```

---

## スタイル設定

### ダイアログパネル

- 位置: 画面下部
- 背景色: rgba(26, 26, 26, 0.9)
- パディング: 20px

### コマンドボタン

- 位置: 画面右下
- サイズ: 120x40
- 背景色: rgba(51, 128, 204, 1)

### コマンドメニュー

- 位置: コマンドボタンの上
- サイズ: 200x300
- 背景色: rgba(26, 26, 26, 0.9)

### コマンドアイテム

- 高さ: 40px
- 背景色: rgba(77, 77, 77, 1)

---

## エディタ拡張

`UISetupEditor.cs` でシーンの自動セットアップが可能。

```
メニュー: DesktopOjisan > Setup Scene
```

実行すると以下が自動生成される：
- GameManager
- Canvas + EventSystem
- OjisanDialog UI
- CommandButton UI
- CommandMenu UI
- CommandItem プレハブ
