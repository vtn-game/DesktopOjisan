# OjisanDialogUI

おじさんの会話文を表示するダイアログUI。

## クラス定義

```csharp
namespace DesktopOjisan.UI

public class OjisanDialogUI : MonoBehaviour
```

## 責務

- 会話テキストの表示
- タイピングエフェクト
- 自動非表示タイマー

## 設定項目

| フィールド | 型 | デフォルト | 説明 |
|-----------|-----|----------|------|
| typingSpeed | float | 0.05 | 1文字あたりの表示時間（秒） |
| autoHideDelay | float | 5.0 | 自動非表示までの時間（秒） |
| useTypingEffect | bool | true | タイピングエフェクト有効化 |

## 公開インターフェース

### ShowMessage

```csharp
public void ShowMessage(string message, bool autoHide = true)
```

メッセージを表示。タイピングエフェクト付き。

### Hide

```csharp
public void Hide()
```

ダイアログを非表示。

### SkipTyping

```csharp
public void SkipTyping()
```

タイピングをスキップして全文表示。

## イベント

```csharp
public event Action OnDialogComplete;  // タイピング完了
public event Action OnDialogHidden;    // 非表示時
```

## UI階層

```
OjisanDialog
└── DialogPanel
    ├── OjisanIcon (Image)
    └── DialogText (TextMeshProUGUI)
```

## 対応ファイル

- `Assets/Scripts/UI/OjisanDialogUI.cs`
