# コーディング規約

## 言語・フレームワーク

| 言語 | 用途 |
|------|------|
| C# | Unity スクリプト |
| C++ | ネイティブプラグイン |

## C# コーディング規約

### 命名規則

| 要素 | 規則 | 例 |
|------|------|-----|
| クラス | PascalCase | `GameManager` |
| メソッド | PascalCase | `ExecuteCommand()` |
| プライベートフィールド | _camelCase | `_isRunning` |
| パブリックプロパティ | PascalCase | `IsRunning` |
| ローカル変数 | camelCase | `commandId` |
| 定数 | UPPER_SNAKE_CASE | `DEFAULT_PORT` |

### ファイル構成

- 1ファイル1クラス
- ファイル名とクラス名を一致させる
- namespace は `DesktopOjisan.{カテゴリ}` 形式

```csharp
namespace DesktopOjisan.Server
{
    public class ApiServer : MonoBehaviour
    {
        // ...
    }
}
```

### クラス設計

- 疎結合を心がける
- 依存性注入（SerializeField経由）
- イベント駆動（Action、UnityEvent）

### 禁止事項

- `FindObjectOfType` の多用（パフォーマンス問題）
- `Update` 内での重い処理
- シングルトンの乱用

## C++ コーディング規約

### 命名規則

| 要素 | 規則 | 例 |
|------|------|-----|
| 関数 | PascalCase | `GetUnityWindowHandle()` |
| 変数 | camelCase | `hwnd` |

### エクスポート

```cpp
extern "C" {
    __declspec(dllexport) ReturnType FunctionName(Args);
}
```

## 使用ライブラリ

| ライブラリ | 用途 |
|-----------|------|
| TextMeshPro | UIテキスト表示 |
| (将来) UniTask | 非同期処理 |
| (将来) R3 | リアクティブプログラミング |

## コメント

- 日本語でOK
- XMLドキュメントコメント推奨

```csharp
/// <summary>
/// コマンドを実行する
/// </summary>
/// <param name="commandId">コマンドID</param>
public void ExecuteCommand(string commandId) { }
```
