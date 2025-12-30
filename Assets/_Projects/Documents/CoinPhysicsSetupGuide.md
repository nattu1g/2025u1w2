# コイン物理設定ガイド

## 概要

コインが積み上がらず、自然に滑り落ちる物理挙動を実現するための設定ガイドです。

## 自動設定される項目

`CoinType.cs`スクリプトをアタッチすると、以下の設定が自動的に適用されます：

### PhysicsMaterial2D（物理マテリアル）

- **Friction（摩擦係数）**: `0.1` - コインが滑りやすくなる
- **Bounciness（反発係数）**: `0.2` - 少し跳ねる程度

### Rigidbody2D設定

- **Mass（質量）**: `0.5`
- **Linear Drag（線形抵抗）**: `0.5`
- **Angular Drag（角度抵抗）**: `0.5`
- **Gravity Scale**: `1.0`
- **Collision Detection Mode**: `Continuous`（貫通防止）

## コインプレハブの推奨設定

### 1. 必須コンポーネント

コインプレハブには以下のコンポーネントが必要です：

- ✅ `CoinType` スクリプト
- ✅ `Rigidbody2D`
- ✅ `CircleCollider2D`（推奨）または `BoxCollider2D`
- ✅ `SpriteRenderer`（ビジュアル）

### 2. Rigidbody2D設定

**Body Type**: `Dynamic`  
**Material**: 自動設定されるため、空欄でOK  
**Simulated**: ✅ チェック  
**Use Auto Mass**: ❌ チェックを外す  
**Constraints**: すべてチェックなし

### 3. CircleCollider2D設定（推奨）

**Is Trigger**: ❌ チェックを外す  
**Material**: 自動設定されるため、空欄でOK  
**Radius**: コインのサイズに合わせて調整（例: `0.5`）

> **なぜCircleCollider2D？**  
> 円形コライダーは角がないため、BoxCollider2Dよりも滑りやすく、積み上がりにくい特性があります。

### 4. レイヤー設定

**Layer**: `Coin`

### 5. タグ設定

**Tag**: `Coin`

## Inspector上での調整

`CoinType`スクリプトのInspectorで、以下のパラメータを調整できます：

### 物理設定

- **Friction（摩擦係数）**: `0.0 ~ 1.0`
  - `0.0`: 完全に滑る（氷の上のような挙動）
  - `0.1`: 推奨値（適度に滑る）
  - `1.0`: 滑らない（積み上がりやすい）

- **Bounciness（反発係数）**: `0.0 ~ 1.0`
  - `0.0`: 全く跳ねない
  - `0.2`: 推奨値（少し跳ねる）
  - `1.0`: 完全に跳ね返る

### Rigidbody2D設定

- **Mass（質量）**: `0.5`
  - 重いほど落下速度が速く、軽いほど遅い
  
- **Linear Drag（線形抵抗）**: `0.5`
  - 空気抵抗。大きいほど減速しやすい
  
- **Angular Drag（角度抵抗）**: `0.5`
  - 回転抵抗。大きいほど回転が止まりやすい

## コインの種類別の推奨設定

### 通常コイン

```
Friction: 0.1
Bounciness: 0.2
Mass: 0.5
```

### 高密度コイン

```
Friction: 0.15（少し滑りにくい）
Bounciness: 0.1（あまり跳ねない）
Mass: 1.0（重い）
```

### 冷却コイン

```
Friction: 0.05（よく滑る）
Bounciness: 0.3（よく跳ねる）
Mass: 0.3（軽い）
```

## Physics2D Layer Collision Matrix設定

コインが正しく動作するために、以下のレイヤー衝突設定を確認してください：

| レイヤー | Coin | Water | Tank |
|---------|------|-------|------|
| **Coin** | ✅ | ❌ | ✅ |
| **Water** | ❌ | ✅ | ✅ |
| **Tank** | ✅ | ✅ | - |

- **Coin ↔ Coin**: ✅ 衝突する（コイン同士がぶつかる）
- **Coin ↔ Water**: ❌ 衝突しない（Triggerのみ）
- **Coin ↔ Tank**: ✅ 衝突する（壁で跳ね返る）

## テスト方法

1. **同じ位置に複数回投下**
   - コインが積み上がらず、滑り落ちることを確認

2. **壁への衝突**
   - 壁に当たって跳ね返ることを確認

3. **水との接触**
   - 水を通過して膨張することを確認（物理衝突はしない）

## トラブルシューティング

### コインが積み上がってしまう

- ✅ Frictionを`0.1`以下に設定
- ✅ CircleCollider2Dを使用
- ✅ Massを適切に設定（軽すぎると積み上がりやすい）

### コインが滑りすぎる

- ✅ Frictionを`0.2`程度に上げる
- ✅ Linear Dragを大きくする

### コインが貫通する

- ✅ Collision Detection Modeが`Continuous`になっているか確認
- ✅ 初期速度が速すぎないか確認

### コインが回転しすぎる

- ✅ Angular Dragを大きくする
- ✅ Rigidbody2DのConstraintsで`Freeze Rotation Z`をチェック
