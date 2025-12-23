# WindowController

Windowsネイティブ API を使用したウィンドウ制御。

## 構成

| ファイル | 言語 | 説明 |
|---------|------|------|
| WindowController.cpp | C++ | ネイティブDLL実装 |
| WindowController.cs | C# | Unity側ラッパー |

## ネイティブ関数

### GetUnityWindowHandle

```cpp
HWND GetUnityWindowHandle()
```

現在のUnityウィンドウハンドルを取得。

### RemoveWindowFrame / RestoreWindowFrame

```cpp
void RemoveWindowFrame(HWND hwnd)
void RestoreWindowFrame(HWND hwnd)
```

ボーダーレスウィンドウ化 / 復元。

### SetWindowTopMost

```cpp
void SetWindowTopMost(HWND hwnd, bool topMost)
```

常に最前面表示の切り替え。

### SetWindowPosition / SetWindowSize

```cpp
void SetWindowPosition(HWND hwnd, int x, int y)
void SetWindowSize(HWND hwnd, int width, int height)
```

ウィンドウ位置・サイズの設定。

### SetClickThrough

```cpp
void SetClickThrough(HWND hwnd, bool enable)
```

クリック透過モードの切り替え。

### SetWindowTransparency

```cpp
void SetWindowTransparency(HWND hwnd, BYTE alpha)
```

ウィンドウ透明度の設定（0-255）。

## C# ラッパー

```csharp
public class WindowController : MonoBehaviour
{
    [DllImport("WindowController")]
    private static extern IntPtr GetUnityWindowHandle();
    // ...
}
```

## ビルド

```bash
cd NativePlugin
./build.bat
```

出力先: `Assets/Plugins/x86_64/WindowController.dll`

## 対応ファイル

- `NativePlugin/WindowController.cpp`
- `NativePlugin/CMakeLists.txt`
- `Assets/Scripts/WindowController.cs`
