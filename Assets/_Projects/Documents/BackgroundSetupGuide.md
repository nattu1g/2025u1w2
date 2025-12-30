# 画面伸縮対応の背景設定ガイド

## 概要

画面解像度やアスペクト比が変わっても、背景が常にカメラ全体を覆うように自動調整する方法を説明します。

---

## 🔧 実装手順

### ステップ1: 背景Spriteを作成

1. **Hierarchy** → 右クリック → **2D Object** → **Sprite**
2. 名前を「**Background**」に変更
3. **Inspector** で設定：
   ```
   Position: (0, 0, -10)  ← カメラより後ろ
   Rotation: (0, 0, 0)
   Scale: (1, 1, 1)  ← スクリプトが自動調整
   ```

### ステップ2: 背景画像を設定

1. **Sprite Renderer** → **Sprite** に背景画像を設定
2. **Color**: 白（または好みの色）
3. **Sorting Layer**: Background（新規作成）
4. **Order in Layer**: -100（最背面）

### ステップ3: BackgroundScalerスクリプトをアタッチ

1. **Background** オブジェクトを選択
2. **Inspector** → **Add Component** → **Background Scaler**
3. 設定：
   ```
   Target Camera: Main Camera（自動検出）
   Padding: 0.5（カメラより少し大きくする余白）
   ```

---

## 🎨 Sorting Layerの設定

### Sorting Layerを作成

1. **Edit** → **Project Settings** → **Tags and Layers**
2. **Sorting Layers** を展開
3. 以下の順序で作成：

| Sorting Layer | Order in Layer | 用途 |
|--------------|----------------|------|
| **Background** | -100 | 背景 |
| **Default** | 0 | 水、水槽 |
| **Coin** | 10 | コイン |
| **UI** | 100 | UI要素 |

### 各オブジェクトに設定

**Background:**
```
Sprite Renderer → Sorting Layer: Background
Sprite Renderer → Order in Layer: -100
```

**Water:**
```
Sprite Renderer → Sorting Layer: Default
Sprite Renderer → Order in Layer: 0
```

**Coin:**
```
Sprite Renderer → Sorting Layer: Coin
Sprite Renderer → Order in Layer: 10
```

---

## 📐 動作原理

### BackgroundScalerの仕組み

1. **カメラサイズを取得**
   ```
   高さ = カメラのOrthographic Size × 2
   幅 = 高さ × アスペクト比
   ```

2. **スプライトのサイズを取得**
   ```
   スプライトの幅・高さ
   ```

3. **スケールを計算**
   ```
   scaleX = カメラ幅 / スプライト幅
   scaleY = カメラ高さ / スプライト高さ
   scale = Max(scaleX, scaleY)  ← 大きい方を使用
   ```

4. **自動調整**
   - 画面サイズが変わるたびに再計算
   - 常にカメラ全体を覆う

---

## 🎯 推奨設定

### 背景画像のサイズ

**推奨解像度:**
- **1920x1080** 以上
- **2048x2048** （正方形、汎用性が高い）
- **4096x4096** （高解像度、様々なアスペクト比に対応）

**アスペクト比:**
- 16:9（一般的）
- 4:3（iPad）
- 正方形（最も汎用性が高い）

### Paddingの調整

```
Padding: 0.5   ← デフォルト（少し余裕を持たせる）
Padding: 0.0   ← ぴったりサイズ
Padding: 1.0   ← 大きめの余白
```

---

## 🧪 テスト方法

### Game Viewでテスト

1. **Game View** を開く
2. **Aspect** を変更して確認：
   - 16:9
   - 16:10
   - 4:3
   - Free Aspect（自由にリサイズ）

3. **背景が常に画面全体を覆っているか確認**

### Scene Viewで確認

1. **Background** オブジェクトを選択
2. Scene Viewで黄色の枠（カメラ範囲）を確認
3. 背景がカメラ範囲を覆っているか確認

---

## 💡 応用例

### グラデーション背景

**方法1: Sprite Rendererの色**
```
Sprite: 白い正方形
Color: グラデーション（シェーダーで実装）
```

**方法2: 複数レイヤー**
```
Background1 (Order: -100) - 遠景
Background2 (Order: -90)  - 中景
Background3 (Order: -80)  - 近景
```

### パララックス効果

BackgroundScalerを拡張して、カメラの移動に応じて背景を少しずらす：

```csharp
// カメラの移動量の50%だけ背景を移動
transform.position = _targetCamera.transform.position * 0.5f;
```

---

## ⚠️ トラブルシューティング

### 問題1: 背景が表示されない

**原因:** Sorting Layerの順序が間違っている

**解決方法:**
- Background の Order in Layer を -100 に設定
- 他のオブジェクトより小さい値にする

### 問題2: 背景が伸びて歪む

**原因:** スプライトのアスペクト比とカメラが合っていない

**解決方法:**
- BackgroundScalerが自動的に調整（大きい方のスケールを使用）
- 正方形の背景画像を使用

### 問題3: 画面サイズ変更時に更新されない

**原因:** LateUpdate()が呼ばれていない

**解決方法:**
- BackgroundScalerスクリプトが有効か確認
- Consoleログを確認

---

## 📋 チェックリスト

設定が完了したら、以下を確認してください：

- [ ] Backgroundオブジェクトが作成されている
- [ ] 背景画像（Sprite）が設定されている
- [ ] BackgroundScalerスクリプトがアタッチされている
- [ ] Sorting Layerが「Background」に設定されている
- [ ] Order in Layerが -100 に設定されている
- [ ] Game Viewで様々なアスペクト比をテスト
- [ ] 背景が常に画面全体を覆っている
