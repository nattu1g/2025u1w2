# 水の膨張が動作しない場合のトラブルシューティング

## 問題

コインが水に当たっても膨張していないように見える。

## 確認手順

### 1. Consoleログを確認

Unity Editorでゲームを実行し、コインを投下した際のConsoleログを確認してください。

#### 期待されるログ

```
WaterExpansion: OnTriggerEnter2D called with [コイン名], Tag: Coin
WaterExpansion: Coin detected! [コイン名]
WaterExpansion: CoinType found! ExpansionRate: 0.1
水が膨張しました: (1.1, 1.1, 1.1)
コイン初回接触: [コイン名]
```

#### ログが全く出ない場合

**原因**: トリガーが発生していない

**確認項目**:
1. 水オブジェクトに`WaterExpansion`スクリプトがアタッチされているか
2. 水オブジェクトに`Collider2D`（BoxCollider2Dなど）があるか
3. Collider2Dの`Is Trigger`がチェックされているか
4. コインに`Rigidbody2D`があるか
5. コインに`Collider2D`があるか

#### 「Not a Coin」と表示される場合

**原因**: コインに「Coin」タグが設定されていない

**修正方法**:
1. コインプレハブを選択
2. Inspector上部の`Tag`を「Coin」に設定
3. 「Coin」タグがない場合は、`Tags & Layers`で追加

#### 「CoinType component not found」と表示される場合

**原因**: コインに`CoinType`スクリプトがアタッチされていない

**修正方法**:
1. コインプレハブを選択
2. `Add Component` → `CoinType`を追加
3. `Expansion Rate`を設定（例: 0.1）

---

## Unity Editor設定チェックリスト

### コインプレハブの設定

- [ ] **Tag**: 「Coin」に設定
- [ ] **Layer**: 「Coin」に設定（推奨）
- [ ] **Rigidbody2D**: アタッチされている
  - Body Type: Dynamic
  - Gravity Scale: 1.0
- [ ] **Collider2D**: CircleCollider2D または BoxCollider2D
  - Is Trigger: ❌ チェックを外す
- [ ] **CoinType**: スクリプトがアタッチされている
  - Expansion Rate: 0.1（通常コイン）

### 水オブジェクトの設定

- [ ] **WaterExpansion**: スクリプトがアタッチされている
- [ ] **Collider2D #1（トリガー用）**: 
  - Is Trigger: ✅ チェック
  - Layer: Water
- [ ] **Collider2D #2（衝突用）**: 
  - Is Trigger: ❌ チェックを外す
  - Layer: Water
- [ ] **Rigidbody2D**: アタッチされている（推奨）
  - Body Type: Dynamic または Kinematic

### Physics2D Layer Collision Matrix

**Edit** → **Project Settings** → **Physics 2D** → **Layer Collision Matrix**

| レイヤー | Coin | Water | Tank |
|---------|------|-------|------|
| **Coin** | ✅ | ❌ | ✅ |
| **Water** | ❌ | ✅ | ✅ |
| **Tank** | ✅ | ✅ | - |

**重要**: Coin ↔ Water は **チェックを外す**（物理衝突しない、Triggerのみ）

---

## よくある問題と解決方法

### 問題1: コインが水を通過してしまう

**原因**: レイヤー衝突設定が正しくない

**解決方法**:
- Coin ↔ Water のレイヤー衝突を **無効化**
- 水のCollider2Dの1つを`Is Trigger: true`に設定

### 問題2: コインが水の上で止まってしまう

**原因**: 水のCollider2Dがすべてトリガーになっている

**解決方法**:
- 水に2つのCollider2Dを設定
  - 1つ目: `Is Trigger: true`（コイン検知用）
  - 2つ目: `Is Trigger: false`（物理衝突用）

### 問題3: 水が膨張しているが見えない

**原因**: 膨張率が小さすぎる、またはカメラの視野外

**解決方法**:
- `Expansion Rate`を大きくする（例: 0.5）
- Scene Viewで水オブジェクトのScaleを確認
- Consoleログで実際のScale値を確認

### 問題4: 初回のみ膨張し、2回目以降膨張しない

**原因**: 正常な動作（設計通り）

**説明**:
- 同じコインは初回接触のみ膨張を引き起こす
- 新しいコインを投下すれば、再度膨張する

---

## デバッグ手順

1. **Consoleログを確認**
   - どのログが出ているか確認
   - エラーや警告がないか確認

2. **Scene Viewで確認**
   - コインが水に接触しているか
   - 水のScaleが変化しているか

3. **Inspector確認**
   - コインのTag、Layer、コンポーネント
   - 水のTag、Layer、コンポーネント

4. **Physics 2D設定確認**
   - Layer Collision Matrixが正しいか

---

## それでも動作しない場合

以下の情報を確認してください：

1. Consoleに表示されているログ（すべて）
2. コインプレハブのInspector設定（スクリーンショット）
3. 水オブジェクトのInspector設定（スクリーンショット）
4. Physics 2D Layer Collision Matrix（スクリーンショット）
