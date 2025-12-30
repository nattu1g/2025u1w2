# コイン投下位置の操作機能実装ガイド

## 概要

ゲームセンターのコイン落としゲーム（メダルゲーム）のように、プレイヤーがコインの投下位置を左右に操作できる機能が実装されました。

## 操作方法

### キーボード操作

- **左に移動**: 左矢印キー または `A`キー
- **右に移動**: 右矢印キー または `D`キー

### マウス/タッチ操作

- **画面をクリック/タッチ**: その位置に投下位置を移動

## Unity Editor設定

### CoinSpawner設定

**Inspector設定項目:**

| パラメータ | 説明 | デフォルト値 |
|----------|------|------------|
| **Move Range X** | X軸方向の移動範囲（±この値の範囲で移動可能） | 5.0 |
| **Move Speed** | 移動速度（秒間の移動量） | 5.0 |

### 移動範囲の可視化

Scene Viewで`CoinSpawner`を選択すると、移動範囲が黄色の線で表示されます。

## 実装詳細

### 追加されたメソッド

#### CoinSpawner.cs

```csharp
// 現在のX位置を取得
public float CurrentX { get; }

// X位置を設定（絶対座標）
public void SetPositionX(float x)

// X位置を相対的に移動
public void MoveX(float deltaX)

// 移動速度を考慮してX位置を更新
public void UpdatePositionX(float direction)
```

#### CoinDropUseCase.cs

```csharp
// 投下位置を左右に移動
public void UpdateDropPosition(float direction)

// 投下位置を直接設定
public void SetDropPosition(float x)

// 現在の投下位置を取得
public float GetCurrentDropPosition()
```

## カスタマイズ

### 移動速度の調整

**速くしたい場合:**
```
Move Speed: 8.0 ~ 10.0
```

**ゆっくりにしたい場合:**
```
Move Speed: 2.0 ~ 3.0
```

### 移動範囲の調整

**広くしたい場合:**
```
Move Range X: 8.0 ~ 10.0
```

**狭くしたい場合:**
```
Move Range X: 2.0 ~ 3.0
```

## トラブルシューティング

### 移動しない

- ✅ `CoinSpawner`に`Spawn Point`が設定されているか確認
- ✅ `Move Range X`が0より大きいか確認
- ✅ `GamePresenterUIToolkit`が`ITickable`を実装しているか確認

### 移動範囲が正しく制限されない

- ✅ `CoinSpawner`の親オブジェクトの位置が中心になっているか確認
- ✅ Scene Viewで黄色の線（移動範囲）を確認

### マウス操作が正しく動作しない

- ✅ `Camera.main`が正しく設定されているか確認
- ✅ 2D/3Dカメラの設定を確認
