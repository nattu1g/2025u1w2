# CoinDefinition（ScriptableObject）の設定ガイド

## 概要

水の膨張率などのゲームバランスパラメータを`CoinDefinition`（ScriptableObject）で管理する方法を説明します。

---

## 📁 ファイル配置

CoinDefinitionは以下のディレクトリに配置してください：

```
Assets/_Projects/Datas/SOs/
├─ NormalCoinDefinition.asset
├─ DenseCoinDefinition.asset
└─ CoolingCoinDefinition.asset
```

---

## 🔧 CoinDefinitionの作成手順

### ステップ1: ScriptableObjectを作成

1. **Project** → **Assets/_Projects/Datas/SOs** フォルダを開く
2. 右クリック → **Create** → **WaterTank** → **CoinDefinition**
3. 名前を設定：
   - `NormalCoinDefinition`
   - `DenseCoinDefinition`
   - `CoolingCoinDefinition`

### ステップ2: パラメータを設定

#### 通常コイン（NormalCoinDefinition）

```
Coin Name: 通常コイン
Description: 安価で膨張率が大きいコイン
Price: 10
Expansion Rate: 0.1
Coin Prefab: CoinPrefab（Assets/_Projects/Prefabs/...）
Icon: （オプション）
```

#### 高密度コイン（DenseCoinDefinition）

```
Coin Name: 高密度コイン
Description: 高価で膨張率が小さいコイン
Price: 50
Expansion Rate: 0.02
Coin Prefab: CoinPrefab
Icon: （オプション）
```

#### 冷却コイン（CoolingCoinDefinition）

```
Coin Name: 冷却コイン
Description: 特別なコイン。水を収縮させる
Price: 100
Expansion Rate: -0.05
Coin Prefab: CoinPrefab
Icon: （オプション）
```

---

## 🔗 GlobalAssetAssemblyへの登録

### ステップ1: GlobalAssetAssemblyを探す

1. **Hierarchy** → **GlobalAssetAssembly** を選択
2. **Inspector** を確認

### ステップ2: CoinDefinitionを登録

**Inspector** の **Coin Definitions** セクションに、作成したCoinDefinitionをドラッグ&ドロップ：

```
Global Asset Assembly (Script)
├─ Water Tank Game
│   └─ コインPrefab: CoinPrefab
└─ Coin Definitions
    ├─ 通常コイン: NormalCoinDefinition
    ├─ 高密度コイン: DenseCoinDefinition
    └─ 冷却コイン: CoolingCoinDefinition
```

---

## ✅ 動作確認

### ステップ1: ゲームを実行

Unity Editorで **Play** ボタンを押す

### ステップ2: Consoleログを確認

**期待されるログ:**
```
CoinDefinitions loaded successfully from GlobalAssetAssembly.
Normal: 通常コイン, Dense: 高密度コイン, Cooling: 冷却コイン
```

**エラーの場合:**
```
CoinDefinitions are not assigned in GlobalAssetAssembly. Please assign them in the Inspector.
NormalCoinDef: NULL
DenseCoinDef: NULL
CoolingCoinDef: NULL
Creating dummy CoinDefinitions as fallback...
```

→ GlobalAssetAssemblyにCoinDefinitionが登録されていません。上記の手順を確認してください。

### ステップ3: コインを投下

コインを投下して、Consoleログを確認：

```
CoinType: ExpansionRate set to 0.1
CoinDropUseCase: Dropped 通常コイン (ExpansionRate: 0.1)
```

→ ScriptableObjectの膨張率が正しく設定されています。

---

## 🎮 バランス調整

### 膨張率の調整

CoinDefinitionを選択し、**Expansion Rate** を変更するだけでバランス調整できます。

**推奨値:**

| コイン種類 | 価格 | 膨張率 | 説明 |
|----------|------|--------|------|
| **通常コイン** | 10 | 0.05 ~ 0.15 | 基本的なコイン |
| **高密度コイン** | 50 | 0.01 ~ 0.05 | 高価で膨張率が小さい |
| **冷却コイン** | 100 | -0.03 ~ -0.1 | 水を収縮させる |

**調整のコツ:**
- 膨張率が大きすぎると、すぐにゲームオーバーになる
- 膨張率が小さすぎると、ゲームが単調になる
- 冷却コインの収縮率は、通常コインの膨張率の半分程度が良い

---

## ⚠️ トラブルシューティング

### 問題1: CoinDefinitionが読み込まれない

**原因:** GlobalAssetAssemblyに登録されていない

**解決方法:**
1. Hierarchy → GlobalAssetAssembly を選択
2. Inspector → Coin Definitions に CoinDefinition をドラッグ&ドロップ

### 問題2: コンパイルエラーが出る

**エラー例:**
```
型または名前空間の名前 'CoinDefinition' が見つかりませんでした
```

**原因:** アセンブリ定義の参照が不足

**解決方法:**
1. Unity Editorを再起動
2. Assets → Reimport All

### 問題3: 膨張率が変わらない

**原因:** CoinPrefabの`CoinType`スクリプトに設定された値が使われている

**解決方法:**
- CoinDefinitionの値が優先されるように実装済み
- ゲームを再起動して確認

---

## 📋 チェックリスト

設定が完了したら、以下を確認してください：

- [ ] `Assets/_Projects/Datas/SOs/` に3つのCoinDefinitionがある
- [ ] 各CoinDefinitionのパラメータが設定されている
- [ ] GlobalAssetAssemblyに3つのCoinDefinitionが登録されている
- [ ] ゲームを実行してConsoleログを確認
- [ ] コインを投下して膨張率が正しく設定されているか確認
