# 進捗記録

## 2025-12-29

### プロジェクトセットアップ完了

新ゲーム「もうひとつ、沈める？ 〜強欲の水槽〜」の開発環境を構築しました。

**実装内容:**

1. **フォルダ構造の作成**
   - `Assets/_Projects/Scripts/Features/WaterTank/` 配下に以下のフォルダを作成:
     - `Coin/` - コイン関連
     - `Water/` - 水オブジェクト関連
     - `Tank/` - 水槽関連
     - `Shop/` - ショップシステム

2. **コアスクリプトの作成**
   - `CoinType.cs` - コインの種類と膨張率を定義
   - `WaterExpansion.cs` - 水の膨張ロジック（OnTriggerEnterでコイン接触を検知）
   - `TankOverflowDetector.cs` - 水槽からのこぼれ検知
   - `CoinDefinition.cs` - コインデータのScriptableObject

3. **VContainer設定の更新**
   - `MainLifeTimeScope.cs` を新ゲーム用に簡素化
   - 既存ゲーム固有のUseCase（MainGameInitializeUseCase、OptionPresenter）を削除
   - 基本システム（Audio、Save/Load）のみ残す
   - 新ゲーム用のUseCase/Presenter追加用のコメントを追加

**次のステップ:**
- コイン投下システムの実装
- 水の膨張システムの実装

### VContainerエラー修正

`MainGameInitializeUseCase`が`MainLifeTimeScope`から削除されていたため、VContainerエラーが発生していました。

**修正内容:**
- ArchitectureGuideline.mdを確認し、既存のアーキテクチャパターンを維持
- `MainGameInitializeUseCase`を`MainLifeTimeScope`に再登録
- 既存の起動シーケンス（`MainSceneInitializer` → `MainGameInitializeUseCase` → `IInitializableUseCase`実装クラス群）を維持

### コイン投下システム実装

コイン投下システムの基本実装を完了しました。

**実装内容:**

1. **コイン関連スクリプト**
   - `CoinSpawner.cs` - コイン生成・投下コンポーネント
   - `CoinType.cs` - コインの種類と膨張率定義（既存）

2. **ゲームロジック**
   - `GameStateEntity.cs` - ゲーム状態管理（ポイント、水位、ゲームオーバー）
   - `CoinDropUseCase.cs` - コイン投下のビジネスロジック

3. **UI実装**
   - `GameView.uxml` / `GameView.uss` - ゲーム画面UI定義
   - `GameViewUIToolkit.cs` - ゲーム画面View
   - `GamePresenterUIToolkit.cs` - ゲーム画面Presenter

4. **VContainer登録**
   - `GameStateEntity`、`CoinDropUseCase`、`GamePresenterUIToolkit`を登録
   - `UIToolkitCanvas`に`GameView`を追加

**次のステップ:**
- Unityエディタでシーン設定（CoinSpawner、水オブジェクト、水槽の配置）
- CoinDefinitionのScriptableObject作成
- 物理演算の調整とテスト

### 2D物理への変換

3D物理から2D物理に変換しました。

**変更内容:**
- `CoinSpawner.cs` - `Rigidbody` → `Rigidbody2D`、`Vector3` → `Vector2`
- `WaterExpansion.cs` - `Collider` → `Collider2D`、`OnTriggerEnter` → `OnTriggerEnter2D`
- `TankOverflowDetector.cs` - `Collider` → `Collider2D`、`OnTriggerEnter` → `OnTriggerEnter2D`

**Unityエディタでの設定:**
- コインプレハブ: `Rigidbody2D`、`CircleCollider2D`または`BoxCollider2D`
- 水オブジェクト: `Rigidbody2D`、`BoxCollider2D`（Is Trigger: true）
- 水槽の壁: `BoxCollider2D`
- こぼれ検知: `BoxCollider2D`（Is Trigger: true）

### GlobalAssetAssembly対応

Resourcesフォルダの代わりに`GlobalAssetAssembly`を使用してコインプレハブを管理するように変更しました。

**変更内容:**
- `GlobalAssetAssembly.cs`にコインプレハブフィールドを追加
- VContainerに`GlobalAssetAssembly`を登録
- `GamePresenterUIToolkit`で`GlobalAssetAssembly`を注入してコインプレハブを取得

### 初回接触判定の実装

コインと水の衝突を適切に処理するため、初回接触のみ膨張判定を行うように修正しました。

**変更内容:**
- `WaterExpansion.cs`に`HashSet`で接触済みコインを記録
- 2回目以降の接触を無視
- `ResetWater()`メソッドで接触記録もクリア

### Physics2Dレイヤー設定

コインと水が物理的に衝突しないように、Physics2Dレイヤーを設定しました。

**設定内容:**
- レイヤー作成: `Coin`、`Water`、`Tank`
- Layer Collision Matrix:
  - Coin ↔ Water: 衝突しない
  - Coin ↔ Tank: 衝突する
  - Water ↔ Tank: 衝突する

**水オブジェクトの最終設定:**
- `BoxCollider2D` #1（Is Trigger: true）- コイン検知用
- `BoxCollider2D` #2（Is Trigger: false）- 水槽衝突用
- Layer: `Water`

## 2025-12-30

### コイン物理挙動の改善

コインが積み上がらず、自然に滑り落ちる物理挙動を実装しました。

**実装内容:**

1. **CoinType.csの拡張**
   - PhysicsMaterial2Dの自動生成・適用機能を追加
   - 摩擦係数（Friction: 0.1）と反発係数（Bounciness: 0.2）の設定
   - Rigidbody2Dパラメータの自動設定（Mass, Linear Drag, Angular Drag）
   - 連続衝突検出モード（Continuous）の有効化

2. **Inspector調整機能**
   - 摩擦係数、反発係数をInspectorで調整可能に
   - コインの種類別に物理パラメータをカスタマイズ可能

3. **ドキュメント作成**
   - `CoinPhysicsSetupGuide.md` - Unity Editor設定ガイド
   - コインの種類別の推奨設定を記載
   - トラブルシューティング情報を追加

**技術的詳細:**
- CircleCollider2Dの使用を推奨（BoxCollider2Dより滑りやすい）
- 低摩擦マテリアルにより、コイン同士が滑って重ならない
- 適度な反発係数で、壁に当たって跳ね返る挙動を実現

**次のステップ:**
- Unity Editor上でコインプレハブに設定を適用
- 実際の挙動をテストして微調整
