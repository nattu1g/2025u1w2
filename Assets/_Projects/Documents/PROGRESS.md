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

### 積み上がり防止機能の強化

コインがまだ積み上がる問題に対処するため、追加の防止機能を実装しました。

**実装内容:**

1. **摩擦係数の最適化**
   - デフォルト値を`0.1`から`0.05`に変更（より滑りやすく）
   - Inspector上で`0.0 ~ 1.0`の範囲で調整可能

2. **コライダースケール調整機能**
   - コライダーを`0.95`倍に縮小（接触面積を減らす）
   - Inspector上で`0.8 ~ 1.0`の範囲で調整可能
   - CircleCollider2DとBoxCollider2Dの両方に対応

3. **ランダム初期回転**
   - コイン生成時に0~360度のランダムな回転を追加
   - 同じ場所に積み上がることを防止
   - Inspector上でON/OFF切り替え可能

4. **Rigidbody2D補間の有効化**
   - `Interpolate`モードを有効化
   - より滑らかな物理演算を実現

5. **反発係数の調整**
   - デフォルト値を`0.2`から`0.3`に変更
   - コインが跳ねやすくなり、積み上がる前に弾かれる

**ドキュメント作成:**
- `CoinStackingPreventionGuide.md` - 積み上がり防止のための詳細な微調整ガイド
- 推奨設定（最も滑りやすい設定、バランス型設定）を記載
- トラブルシューティング情報を追加

**推奨設定（積み上がり防止特化）:**
```
Friction: 0.02
Bounciness: 0.4
Collider Scale: 0.90
Random Initial Rotation: ON
Mass: 0.5
```

### コイン投下位置の操作機能実装

ゲームセンターのコイン落としゲームのように、プレイヤーがコインの投下位置（X座標）を左右に操作できる機能を実装しました。

**実装内容:**

1. **CoinSpawner.csの拡張**
   - X軸方向の移動機能を追加（`SetPositionX()`, `MoveX()`, `UpdatePositionX()`）
   - 移動範囲の制限機能（`_moveRangeX`パラメータ）
   - 移動速度の設定（`_moveSpeed`パラメータ）
   - 現在位置の取得プロパティ（`CurrentX`, `MinX`, `MaxX`）
   - Gizmosによる移動範囲の可視化（Scene Viewで黄色の線で表示）

2. **CoinDropUseCase.csの拡張**
   - `UpdateDropPosition()` - 移動方向を指定して位置を更新
   - `SetDropPosition()` - 絶対座標で位置を設定
   - `GetCurrentDropPosition()` - 現在位置を取得

3. **GamePresenterUIToolkit.csの拡張**
   - `ITickable`インターフェースを実装
   - `Tick()`メソッドで毎フレーム入力を処理
   - キーボード入力対応（左右矢印キー、A/Dキー）
   - マウス/タッチ入力対応（クリック/タッチ位置に直接移動）

**操作方法:**
- **キーボード**: 左右矢印キー または A/Dキー で左右に移動
- **マウス/タッチ**: 画面をクリック/タッチした位置に投下位置を移動

**Unity Editor設定:**
- `CoinSpawner`の`Move Range X`: 移動範囲（デフォルト: 5.0）
- `CoinSpawner`の`Move Speed`: 移動速度（デフォルト: 5.0）

**次のステップ:**
- Unity Editorでテストして動作確認
- 必要に応じて移動速度や範囲を調整

### 投下位置操作の不具合修正

投下位置が変わらない問題を修正しました。

**問題の原因:**
- `GamePresenterUIToolkit`が`ITickable`として登録されていなかった
- そのため`Tick()`メソッドが毎フレーム呼ばれず、入力処理が実行されていなかった

**修正内容:**

1. **MainLifeTimeScope.csの修正**
   - `GamePresenterUIToolkit`の登録を`.As<IStartable>()`から`.AsImplementedInterfaces()`に変更
   - これにより`IStartable`と`ITickable`の両方が自動的に登録される

2. **デバッグログの追加**
   - `HandleDropPositionInput()`にデバッグログを追加
   - キー入力時と位置更新時にログを出力
   - 動作確認が容易になる

**確認方法:**
1. Unity Editorでゲームを実行
2. 左右矢印キーまたはA/Dキーを押す
3. Consoleに「Left key pressed」または「Right key pressed」が表示される
4. 「Drop position updated: X -> Y」が表示され、位置が変わることを確認

### Camera.main nullエラーの修正

マウスクリック時に`NullReferenceException`が発生する問題を修正しました。

**問題の原因:**
- `Camera.main`がnullの場合、`ScreenToWorldPoint()`でエラーが発生
- シーンにMainCameraタグが付いたカメラがない、または設定されていない

**修正内容:**

1. **Camera.mainのnullチェック追加**
   - マウス入力処理の前にカメラの存在を確認
   - カメラがない場合は警告ログを出力し、マウス入力を無効化

2. **2Dゲーム対応の改善**
   - `mousePos.z = -Camera.main.transform.position.z;`を追加
   - 2Dゲームで正しくワールド座標に変換されるように修正

**Unity Editor設定:**
- シーンにカメラを配置
- カメラのTagを「MainCamera」に設定
- 2Dゲームの場合、カメラのProjectionを「Orthographic」に設定

**注意:**
- マウス入力を使用する場合は、必ずMainCameraタグが付いたカメラをシーンに配置してください
- キーボード入力はカメラなしでも動作します

### Camera.main警告の頻度削減

マウスクリック時の警告が毎フレーム表示される問題を改善しました。

**修正内容:**
- `_cameraWarningShown`フラグを追加
- Camera.mainがnullの場合の警告を初回のみ表示
- 2回目以降のクリックでは警告を表示しない

**効果:**
- Consoleログが煩雑にならない
- パフォーマンスへの影響を最小化

### マウス入力のドラッグ操作への変更

UIボタンクリック時にマウス位置を追従してしまう問題を修正しました。

**問題の原因:**
- `Input.GetMouseButton(0)`はUIボタンのクリックも含まれる
- ボタンをクリックすると、その位置に投下位置が移動してしまう
- 画面端のボタンをクリックすると、画面端にコインを落としてしまう

**修正内容:**

1. **ドラッグ判定の追加**
   - 前フレームのマウス位置を記録（`_lastMousePosition`）
   - マウスが1ピクセル以上移動している場合のみ位置を更新
   - 静止したクリック（UIボタンクリック）では位置が変わらない

2. **デバッグログの改善**
   - 「Mouse position」→「Mouse drag position」に変更
   - ドラッグ操作であることを明確化

**操作方法の変更:**
- **キーボード**: 左右矢印キー または A/Dキー（変更なし）
- **マウス**: ドラッグ操作で投下位置を移動（クリックのみでは移動しない）

**効果:**
- UIボタンをクリックしても投下位置が変わらない
- 意図しない位置にコインを落とすことがなくなる

### 水の膨張デバッグログ追加

水に当たっても膨張していない問題を調査するため、デバッグログを追加しました。

**追加したログ:**
1. `OnTriggerEnter2D`が呼ばれたとき
2. コインが検出されたとき
3. CoinTypeコンポーネントが見つかったとき
4. 膨張率の値
5. タグが「Coin」でない場合の警告

**ドキュメント作成:**
- `WaterExpansionTroubleshooting.md` - 水の膨張が動作しない場合のトラブルシューティングガイド
- Unity Editor設定チェックリスト
- よくある問題と解決方法
- デバッグ手順

**確認方法:**
1. Unity Editorでゲームを実行
2. コインを投下
3. Consoleログを確認
4. トラブルシューティングガイドに従って設定を確認

### 膨張率をScriptableObjectから設定

水の膨張率を`CoinDefinition`（ScriptableObject）から設定できるようにしました。

**変更前:**
- コインプレハブの`CoinType`スクリプトに設定された値が使われていた
- 各コインの膨張率を変更するには、プレハブを直接編集する必要があった

**変更後:**
- `CoinDefinition`（SO）に設定された膨張率が使われる
- コイン生成時に`CoinDefinition`の値を`CoinType`に設定
- SOを編集するだけで膨張率を変更できる

**実装内容:**

1. **CoinType.csの拡張**
   - `SetExpansionRate(float)`メソッドを追加
   - 外部から膨張率を設定できるようにした

2. **CoinDropUseCase.csの修正**
   - `DropCoin()`メソッドでコイン生成後に膨張率を設定
   - `DropCoinFree()`メソッドでも同様に設定
   - `coinType.SetExpansionRate(coinDefinition.ExpansionRate)`を呼び出し

**使い方:**
1. `CoinDefinition`（SO）を作成
   - Project → 右クリック → Create → WaterTank → CoinDefinition
2. 膨張率を設定
   - Expansion Rate: 0.1（通常コイン）、0.02（高密度コイン）、-0.05（冷却コイン）
3. コインプレハブを設定
4. ゲームで使用

**メリット:**
- SOを編集するだけで膨張率を調整できる
- プレハブを直接編集する必要がない
- バランス調整が容易

### 親オブジェクト（Water）も膨張するように修正

WaterTrigger（子オブジェクト）だけでなく、親のWaterオブジェクト（見た目のCube）も一緒に膨張するように修正しました。

**問題:**
- WaterTriggerのスケールだけが変わり、親のWater（Cube）は変わらなかった
- 見た目が膨張しない
- Colliderのサイズも変わらない

**修正内容:**

1. **ExpandWater()メソッドの修正**
   - `transform.parent`（親のWater）のスケールを変更
   - 親がない場合は自身のスケールを変更（フォールバック）

2. **Awake()メソッドの修正**
   - 親オブジェクトの初期スケールを記録
   - Collider2Dのnullチェックを追加

3. **ResetWater()メソッドの修正**
   - 親オブジェクトのスケールをリセット

**コード例:**
```csharp
// 親オブジェクト（Water）のスケールを変更
Transform waterTransform = transform.parent != null ? transform.parent : transform;
Vector3 newScale = waterTransform.localScale + expansion;
waterTransform.localScale = newScale;
```

**効果:**
- 見た目のCubeが膨張する
- Colliderも一緒に大きくなる
- 2回目以降のコイン接触も正しく検知される
