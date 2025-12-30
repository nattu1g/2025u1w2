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

            // マウス/タッチ入力（画面をクリック/タッチした位置に移動）
            // 注意: 2Dゲームの場合、カメラが正しく設定されている必要があります
            if (Input.GetMouseButton(0))
            {
                // カメラのnullチェック
                if (Camera.main == null)
                {
                    Debug.LogWarning("Camera.main is null. Mouse input is disabled. Please add a Camera with MainCamera tag to the scene.");
                }
                else
                {
                    Vector3 mousePos = Input.mousePosition;
                    // スクリーン座標をワールド座標に変換
                    // 2Dの場合、Z座標を0に設定
                    mousePos.z = -Camera.main.transform.position.z;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    _coinDropUseCase.SetDropPosition(worldPos.x);
                    Debug.Log($"Mouse position: {worldPos.x}");
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
            // TODO: CoinDefinitionをScriptableObjectから読み込む
            // 現時点ではデバッグ用にダミーのCoinDefinitionを作成
            Debug.LogWarning("Using dummy CoinDefinitions for debugging. Please create ScriptableObjects later.");

            // GlobalAssetAssemblyからコインプレハブを取得
            GameObject coinPrefab = _assetAssembly.CoinPrefab;

            if (coinPrefab == null)
            {
                Debug.LogError("Coin prefab is null in GlobalAssetAssembly. Please assign the Coin prefab in the Inspector.");
            }

            // ダミーのCoinDefinitionを作成（デバッグ用）
            _normalCoinDef = ScriptableObject.CreateInstance<CoinDefinition>();
            _normalCoinDef.InitializeForDebug("通常コイン", 10, 0.1f, coinPrefab);

            _denseCoinDef = ScriptableObject.CreateInstance<CoinDefinition>();
            _denseCoinDef.InitializeForDebug("高密度コイン", 50, 0.02f, coinPrefab);

            _coolingCoinDef = ScriptableObject.CreateInstance<CoinDefinition>();
            _coolingCoinDef.InitializeForDebug("冷却コイン", 100, -0.05f, coinPrefab);

            Debug.Log($"Dummy CoinDefinitions created with coin prefab: {(coinPrefab != null ? "OK" : "NULL")}");
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _buttonHandler?.Dispose();
        }
    }
}
