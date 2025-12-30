# 背景が表示されない場合のトラブルシューティング

## 🔍 チェックリスト

以下を順番に確認してください：

### 1. Backgroundオブジェクトの基本設定

**Hierarchy → Background を選択 → Inspector で確認:**

- [ ] **Position Z座標**: -10 または 0 より小さい値
  - カメラのZ座標より小さくする必要があります
  - Main CameraのZ座標が -10 の場合、Backgroundは -9 以上にする

- [ ] **Sprite Renderer → Sprite**: 画像が設定されているか
  - Noneになっていないか確認

- [ ] **Sprite Renderer → Color**: 白（255, 255, 255, 255）
  - アルファ値が0になっていないか確認

### 2. カメラの設定

**Hierarchy → Main Camera を選択 → Inspector で確認:**

- [ ] **Projection**: Orthographic
  - Perspectiveになっていないか確認

- [ ] **Culling Mask**: Backgroundレイヤーが含まれているか
  - Everything または Background にチェックが入っているか

- [ ] **Clear Flags**: Skybox または Solid Color
  - Don't Clear になっていないか

### 3. レイヤー設定

**Background オブジェクトを選択:**

- [ ] **Layer**: Default または Background
  - カメラのCulling Maskに含まれているレイヤーか確認

### 4. Sorting Layer設定

**Sprite Renderer → Sorting Layer:**

- [ ] **Sorting Layer**: Background または Default
- [ ] **Order in Layer**: -100 または小さい値

---

## 🎯 最も可能性が高い原因

### 原因1: Z座標の問題

**問題:**
- BackgroundのZ座標がカメラより手前にある
- カメラが -10、Backgroundが 0 の場合、Backgroundは見えない

**解決方法:**
```
Background の Position Z: 0
Main Camera の Position Z: -10

または

Background の Position Z: 10
Main Camera の Position Z: 0
```

**重要:** 2Dゲームでは、Z座標が**小さいほど手前**です。

### 原因2: Spriteが設定されていない

**問題:**
- Sprite Rendererに画像が設定されていない

**解決方法:**
1. Project → 背景画像を探す
2. Background → Sprite Renderer → Sprite にドラッグ&ドロップ

### 原因3: Culling Maskの問題

**問題:**
- カメラがBackgroundレイヤーを表示しない設定になっている

**解決方法:**
1. Main Camera → Culling Mask → Everything にチェック

---

## 💡 簡単な確認方法

### Scene Viewで確認

1. **Scene View** を開く
2. **Background** オブジェクトを選択
3. **Scene Viewで背景が見えるか確認**
   - 見える → カメラ設定の問題
   - 見えない → Sprite設定の問題

### Game Viewで確認

1. **Game View** を開く
2. **Play** ボタンを押す
3. **Consoleログを確認**
   - `BackgroundScaler: Adjusted to camera size` が出ているか

---

## 🔧 推奨設定（確実に表示される設定）

### Backgroundオブジェクト

```
Transform:
  Position: (0, 0, 0)  ← Z座標を0に
  Rotation: (0, 0, 0)
  Scale: (1, 1, 1)

Sprite Renderer:
  Sprite: （背景画像）
  Color: 白 (255, 255, 255, 255)
  Sorting Layer: Default
  Order in Layer: -100

Layer: Default

Background Scaler:
  Target Camera: Main Camera
  Padding: 0.5
```

### Main Camera

```
Transform:
  Position: (0, 0, -10)  ← Z座標を-10に

Camera:
  Projection: Orthographic
  Size: 5
  Culling Mask: Everything
  Clear Flags: Solid Color
  Background: 任意の色
```

---

## 🧪 デバッグ手順

### 1. 最小構成でテスト

1. **新しいSpriteを作成**
   ```
   Hierarchy → 右クリック → 2D Object → Sprite → Square
   名前: TestBackground
   Position: (0, 0, 0)
   Sprite Renderer → Color: 赤 (255, 0, 0, 255)
   ```

2. **Game Viewで確認**
   - 赤い四角が見えるか？
   - 見える → 元のBackgroundの設定が問題
   - 見えない → カメラの設定が問題

### 2. Consoleログを確認

**期待されるログ:**
```
BackgroundScaler: Adjusted to camera size. Scale: X.XX, Camera: XXxXX
```

**ログが出ない場合:**
- BackgroundScalerスクリプトがアタッチされていない
- スクリプトにエラーがある

---

## 📋 よくある間違い

### ❌ 間違い1: Z座標が逆

```
Background Z: -10
Camera Z: 0
```
→ Backgroundがカメラより手前にあるため見えない

### ✅ 正解:

```
Background Z: 0
Camera Z: -10
```

### ❌ 間違い2: Spriteが未設定

```
Sprite Renderer → Sprite: None
```

### ✅ 正解:

```
Sprite Renderer → Sprite: 背景画像
```

### ❌ 間違い3: アルファ値が0

```
Color: (255, 255, 255, 0)  ← 透明
```

### ✅ 正解:

```
Color: (255, 255, 255, 255)  ← 不透明
```

---

## 🎯 確実に表示させる手順

### ステップ1: 白い四角で確認

1. Hierarchy → 右クリック → 2D Object → Sprite → Square
2. 名前: Background
3. Position: (0, 0, 0)
4. Sprite Renderer → Color: 白
5. Game Viewで白い四角が見えることを確認

### ステップ2: 背景画像に変更

1. Sprite Renderer → Sprite: 背景画像
2. Game Viewで背景画像が見えることを確認

### ステップ3: BackgroundScalerを追加

1. Add Component → Background Scaler
2. Game Viewで画面全体に広がることを確認

---

現在の設定を教えていただければ、具体的な解決方法をお伝えします！

**確認していただきたい情報:**
1. Background の Position（特にZ座標）
2. Main Camera の Position（特にZ座標）
3. Sprite Renderer に Sprite が設定されているか
4. Scene View で Background が見えるか
