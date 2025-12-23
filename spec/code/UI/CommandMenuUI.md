# CommandMenuUI

利用可能なコマンド一覧を表示するメニュー。

## クラス定義

```csharp
namespace DesktopOjisan.UI

public class CommandMenuUI : MonoBehaviour
```

## 責務

- コマンドリストの動的生成
- コマンド選択の検出
- メニュー開閉管理

## 設定項目

| フィールド | 型 | デフォルト | 説明 |
|-----------|-----|----------|------|
| closeOnCommandSelect | bool | true | 選択後に自動で閉じる |

## 公開インターフェース

### Open

```csharp
public void Open(List<CommandInfo> commands)
```

コマンドリストを表示してメニューを開く。

### Close

```csharp
public void Close()
```

メニューを閉じる。

### Toggle

```csharp
public void Toggle(List<CommandInfo> commands)
```

開閉をトグル。

## イベント

```csharp
public event Action<CommandInfo> OnCommandSelected;
public event Action OnMenuClosed;
```

## データ構造

```csharp
[Serializable]
public class CommandInfo
{
    public string id;           // コマンドID
    public string displayName;  // 表示名
    public string description;  // 説明
    public Sprite icon;         // アイコン
    public bool isEnabled;      // 有効フラグ
}
```

## UI階層

```
CommandMenu
└── MenuPanel
    ├── CloseButton
    └── CommandList (VerticalLayoutGroup)
        ├── CommandItem
        ├── CommandItem
        └── ...
```

## 対応ファイル

- `Assets/Scripts/UI/CommandMenuUI.cs`
