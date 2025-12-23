# パフォーマンス基準

## 目標

デスクトップマスコットとして常駐するため、リソース消費を最小限に抑える。

## 基準値

### CPU使用率

| 状態 | 許容値 |
|------|--------|
| アイドル時 | < 1% |
| メッセージ表示中 | < 5% |
| メニュー操作中 | < 10% |

### メモリ使用量

| 項目 | 許容値 |
|------|--------|
| 起動直後 | < 100MB |
| 安定稼働時 | < 150MB |
| 最大 | < 200MB |

### GCアロケーション

| 項目 | 許容値 |
|------|--------|
| 起動後60フレーム以降 | < 1KB/フレーム |
| Update()内 | 0 アロケーション推奨 |

### レスポンス時間

| 操作 | 許容値 |
|------|--------|
| コマンドボタンクリック → メニュー表示 | < 100ms |
| コマンド選択 → ダイアログ表示 | < 100ms |
| API リクエスト → レスポンス | < 50ms |

## 計測方法

### Unity Profiler

```
1. Window > Analysis > Profiler を開く
2. CPU Usage / Memory を確認
3. 各基準値を満たしているか確認
```

### GCアロケーション確認

```csharp
// テスト用コード
void Update()
{
    UnityEngine.Profiling.Profiler.BeginSample("Update Check");
    // 処理
    UnityEngine.Profiling.Profiler.EndSample();
}
```

### API レスポンス時間

```powershell
# PowerShell で計測
Measure-Command { Invoke-RestMethod http://localhost:8080/api/ping }
```

## 最適化ガイドライン

### 避けるべきパターン

```csharp
// NG: Update内での文字列結合
void Update()
{
    string s = "Value: " + value; // GCアロケーション発生
}

// NG: Update内でのLINQ
void Update()
{
    var items = list.Where(x => x.active).ToList(); // GCアロケーション発生
}

// NG: 毎フレームのFind
void Update()
{
    var obj = FindObjectOfType<SomeType>(); // 重い処理
}
```

### 推奨パターン

```csharp
// OK: StringBuilder を再利用
private StringBuilder _sb = new StringBuilder();
void Update()
{
    _sb.Clear();
    _sb.Append("Value: ");
    _sb.Append(value);
}

// OK: キャッシュを使用
private SomeType _cachedObj;
void Start()
{
    _cachedObj = FindObjectOfType<SomeType>();
}
```

## 監視項目

### 常時監視

- [ ] CPU使用率がアイドル時1%以下
- [ ] メモリリークがない（長時間稼働で増加しない）

### リリース前確認

- [ ] 1時間稼働後もメモリ150MB以下
- [ ] すべてのAPI応答が50ms以下
- [ ] GCスパイクがない
