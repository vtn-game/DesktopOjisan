# CommandButtonUI

コマンドメニューを開くためのメインボタン。

## クラス定義

```csharp
namespace DesktopOjisan.UI

public class CommandButtonUI : MonoBehaviour
```

## 責務

- ボタンクリックの検出
- メニュー表示トリガー

## 設定項目

| フィールド | 型 | デフォルト | 説明 |
|-----------|-----|----------|------|
| buttonLabel | string | "コマンド" | ボタンに表示するテキスト |

## 公開インターフェース

### SetButtonLabel

```csharp
public void SetButtonLabel(string label)
```

### SetInteractable

```csharp
public void SetInteractable(bool interactable)
```

### SetIcon

```csharp
public void SetIcon(Sprite icon)
```

## イベント

```csharp
public event Action OnMainButtonClicked;
```

## UI階層

```
CommandButton
├── ButtonIcon (Image)
└── ButtonText (TextMeshProUGUI)
```

## 対応ファイル

- `Assets/Scripts/UI/CommandButtonUI.cs`
