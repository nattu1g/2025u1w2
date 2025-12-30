using System;
using App.Features;
using App.SOs;
using App.UIs.Core;
using App.UIs.Views;
using App.Vcontainer.Entity;
using App.Vcontainer.UseCase.RunTime;
using Common.Vcontainer.Handler;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace App.Vcontainer.Presenter
{
    /// <summary>
    /// ゲーム画面のPresenter（UI Toolkit版）
    /// </summary>
    public class GamePresenterUIToolkit : IDisposable, IStartable, ITickable
    {
        private readonly UIToolkitCanvas _uiToolkitCanvas;
        private readonly GameStateEntity _gameState;
        private readonly CoinDropUseCase _coinDropUseCase;
        private readonly UIToolkitButtonHandler _buttonHandler;
        private readonly GlobalAssetAssembly _assetAssembly;
        private readonly CompositeDisposable _disposables = new();

        private GameViewUIToolkit _gameView;

        // Camera.main警告を一度だけ表示するためのフラグ
        private bool _cameraWarningShown = false;

        // 前フレームのマウス位置（ドラッグ判定用）
        private Vector3 _lastMousePosition;

        // コイン定義（後でScriptableObjectから読み込む）
        // TODO: CoinDefinitionをResourcesやAddressablesから読み込む
        private CoinDefinition _normalCoinDef;
        private CoinDefinition _denseCoinDef;
        private CoinDefinition _coolingCoinDef;

        public GamePresenterUIToolkit(
            UIToolkitCanvas uiToolkitCanvas,
            GameStateEntity gameState,
            CoinDropUseCase coinDropUseCase,
            UIToolkitButtonHandler buttonHandler,
            GlobalAssetAssembly assetAssembly)
        {
            _uiToolkitCanvas = uiToolkitCanvas;
            _gameState = gameState;
            _coinDropUseCase = coinDropUseCase;
            _buttonHandler = buttonHandler;
            _assetAssembly = assetAssembly;
        }

        public void Start()
        {
            // アーキテクチャガイドラインに従い、Start()でViewを取得
            _gameView = _uiToolkitCanvas.GameView;

            if (_gameView == null)
            {
                Debug.LogError("GameView is null in GamePresenterUIToolkit.Start()");
                return;
            }

            // TODO: CoinDefinitionを読み込む
            // 現時点では仮実装
            LoadCoinDefinitions();

            // --- GameStateEntityのデータ変更を購読し、UIを更新する ---
            _gameState.Points
                .Subscribe(points => _gameView.SetPoints(points))
                .AddTo(_disposables);

            _gameState.IsGameOver
                .Subscribe(isGameOver =>
                {
                    if (isGameOver)
                    {
                        _gameView.SetButtonsEnabled(false);
                        Debug.Log("Game Over! Buttons disabled.");
                    }
                })
                .AddTo(_disposables);

            // --- UIイベントを購読し、UseCaseを呼び出す ---
            _buttonHandler.SetupActionButton(_gameView.DropNormalCoinButton, async () =>
            {
                await DropCoinAsync(_normalCoinDef);
            });

            _buttonHandler.SetupActionButton(_gameView.DropDenseCoinButton, async () =>
            {
                await DropCoinAsync(_denseCoinDef);
            });

            _buttonHandler.SetupActionButton(_gameView.DropCoolingCoinButton, async () =>
            {
                await DropCoinAsync(_coolingCoinDef);
            });

            _buttonHandler.SetupActionButton(_gameView.FoldButton, async () =>
            {
                await FoldAsync();
            });

            // 初期ポイントを設定（デバッグ用）
            _gameState.AddPoints(100);
        }

        public void Tick()
        {
            // 投下位置の操作（キーボード入力）
            HandleDropPositionInput();
        }

        /// <summary>
        /// 投下位置の入力処理
        /// </summary>
        private void HandleDropPositionInput()
        {
            float direction = 0f;

            // 左右矢印キー
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                direction = -1f;
                Debug.Log("Left key pressed");
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                direction = 1f;
                Debug.Log("Right key pressed");
            }

            // マウス/タッチ入力（ドラッグ操作のみ）
            // GetMouseButton(0)だとUIボタンクリックも含まれるため、
            // マウスが移動している場合のみ位置を更新する
            if (Input.GetMouseButton(0))
            {
                // マウスが移動しているかチェック（ドラッグ判定）
                Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;
                bool isMouseMoving = mouseDelta.magnitude > 1f; // 1ピクセル以上移動

                if (isMouseMoving)
                {
                    // カメラのnullチェック
                    if (Camera.main == null)
                    {
                        // 警告は初回のみ表示
                        if (!_cameraWarningShown)
                        {
                            Debug.LogWarning("Camera.main is null. Mouse input is disabled. Please add a Camera with MainCamera tag to the scene.");
                            _cameraWarningShown = true;
                        }
                    }
                    else
                    {
                        Vector3 mousePos = Input.mousePosition;
                        // スクリーン座標をワールド座標に変換
                        // 2Dの場合、Z座標を0に設定
                        mousePos.z = -Camera.main.transform.position.z;
                        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                        _coinDropUseCase.SetDropPosition(worldPos.x);
                        Debug.Log($"Mouse drag position: {worldPos.x}");
                    }
                }
            }
            else if (Mathf.Abs(direction) > 0.01f)
            {
                // キーボード入力の場合
                float currentPos = _coinDropUseCase.GetCurrentDropPosition();
                _coinDropUseCase.UpdateDropPosition(direction);
                float newPos = _coinDropUseCase.GetCurrentDropPosition();
                Debug.Log($"Drop position updated: {currentPos} -> {newPos}");
            }

            // 前フレームのマウス位置を記録
            _lastMousePosition = Input.mousePosition;
        }

        private async UniTask DropCoinAsync(CoinDefinition coinDef)
        {
            if (coinDef == null)
            {
                Debug.LogWarning("CoinDefinition is null");
                await UniTask.Yield();
                return;
            }

            bool success = _coinDropUseCase.DropCoin(coinDef);

            if (success)
            {
                Debug.Log($"Dropped {coinDef.CoinName}");
            }
            else
            {
                Debug.LogWarning($"Failed to drop {coinDef.CoinName}");
            }

            await UniTask.Yield();
        }

        private async UniTask FoldAsync()
        {
            Debug.Log("Fold button clicked");

            // TODO: フォールド処理の実装
            // - 現在の水位に応じてポイントを獲得
            // - ゲームをリセット

            await UniTask.Yield();
        }

        private void LoadCoinDefinitions()
        {
            // GlobalAssetAssemblyからCoinDefinitionを読み込む
            _normalCoinDef = _assetAssembly.NormalCoinDef;
            _denseCoinDef = _assetAssembly.DenseCoinDef;
            _coolingCoinDef = _assetAssembly.CoolingCoinDef;

            // nullチェック
            if (_normalCoinDef == null || _denseCoinDef == null || _coolingCoinDef == null)
            {
                Debug.LogError("CoinDefinitions are not assigned in GlobalAssetAssembly. Please assign them in the Inspector.");
                Debug.LogError($"NormalCoinDef: {(_normalCoinDef != null ? "OK" : "NULL")}");
                Debug.LogError($"DenseCoinDef: {(_denseCoinDef != null ? "OK" : "NULL")}");
                Debug.LogError($"CoolingCoinDef: {(_coolingCoinDef != null ? "OK" : "NULL")}");

                // フォールバック: ダミーのCoinDefinitionを作成
                Debug.LogWarning("Creating dummy CoinDefinitions as fallback...");
                GameObject coinPrefab = _assetAssembly.CoinPrefab;

                if (_normalCoinDef == null)
                {
                    _normalCoinDef = ScriptableObject.CreateInstance<CoinDefinition>();
                    _normalCoinDef.InitializeForDebug("通常コイン", 10, 0.1f, coinPrefab);
                }
                if (_denseCoinDef == null)
                {
                    _denseCoinDef = ScriptableObject.CreateInstance<CoinDefinition>();
                    _denseCoinDef.InitializeForDebug("高密度コイン", 50, 0.02f, coinPrefab);
                }
                if (_coolingCoinDef == null)
                {
                    _coolingCoinDef = ScriptableObject.CreateInstance<CoinDefinition>();
                    _coolingCoinDef.InitializeForDebug("冷却コイン", 100, -0.05f, coinPrefab);
                }
            }
            else
            {
                Debug.Log("CoinDefinitions loaded successfully from GlobalAssetAssembly.");
                Debug.Log($"Normal: {_normalCoinDef.CoinName}, Dense: {_denseCoinDef.CoinName}, Cooling: {_coolingCoinDef.CoinName}");
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _buttonHandler?.Dispose();
        }
    }
}
