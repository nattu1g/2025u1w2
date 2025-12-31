using System;
using App.Events;
using App.Features.WaterTank.Water;
using App.Features.WaterTank.Baseline;
using App.UIs.Core;
using App.UIs.Views;
using App.Vcontainer.Entity;
using App.Vcontainer.UseCase.RunTime;
using Common.Vcontainer.Handler;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace App.Vcontainer.Presenter
{
    /// <summary>
    /// ゲームオーバー画面のPresenter（UI Toolkit版）
    /// </summary>
    public class GameOverPresenterUIToolkit : IStartable, IDisposable
    {
        private readonly UIToolkitCanvas _uiToolkitCanvas;
        private readonly GameStateEntity _gameState;
        private readonly GameOverUseCase _gameOverUseCase;
        private readonly UIToolkitButtonHandler _buttonHandler;
        private readonly WaterSpawner _waterSpawner;
        private readonly BaselineDisplay _baselineDisplay;
        private readonly CompositeDisposable _disposables = new();

        private GameOverViewUIToolkit _gameOverView;

        public GameOverPresenterUIToolkit(
            UIToolkitCanvas uiToolkitCanvas,
            GameStateEntity gameState,
            GameOverUseCase gameOverUseCase,
            UIToolkitButtonHandler buttonHandler,
            WaterSpawner waterSpawner,
            BaselineDisplay baselineDisplay)
        {
            _uiToolkitCanvas = uiToolkitCanvas;
            _gameState = gameState;
            _gameOverUseCase = gameOverUseCase;
            _buttonHandler = buttonHandler;
            _waterSpawner = waterSpawner;
            _baselineDisplay = baselineDisplay;
        }

        public void Start()
        {
            // アーキテクチャガイドラインに従い、Start()でViewを取得
            _gameOverView = _uiToolkitCanvas.GameOverView;

            if (_gameOverView == null)
            {
                Debug.LogError("GameOverView is null in GameOverPresenterUIToolkit.Start()");
                return;
            }

            // GameStateEntity.IsGameOverを購読
            _gameState.IsGameOver
                .Subscribe(isGameOver =>
                {
                    if (isGameOver)
                    {
                        ShowGameOverScreen();
                    }
                })
                .AddTo(_disposables);

            // リトライボタン
            _buttonHandler.SetupActionButton(_gameOverView.RetryButton, async () =>
            {
                await OnRetryClickedAsync();
            });
        }

        /// <summary>
        /// ゲームオーバー画面を表示
        /// </summary>
        private void ShowGameOverScreen()
        {
            Debug.Log("GameOverPresenterUIToolkit: Showing game over screen");

            // 最終スコアを表示
            _gameOverView.SetFinalScore(_gameState.Points.CurrentValue);

            // ゲームオーバー画面を表示
            _uiToolkitCanvas.Show(_gameOverView);
        }

        /// <summary>
        /// リトライボタンクリック時の処理
        /// </summary>
        private async UniTask OnRetryClickedAsync()
        {
            Debug.Log("GameOverPresenterUIToolkit: Retry button clicked");

            // ゲームオーバー画面を非表示
            _uiToolkitCanvas.Hide(_gameOverView);

            // シーン内のすべてのコインを削除
            ClearAllCoins();

            // ゲームをリトライ
            await _gameOverUseCase.RetryGame();

            // 基準線を初期位置にリセット（フォールドカウント0）
            _baselineDisplay.UpdateHeight(0);

            // Waterを再生成
            _waterSpawner.ClearAllWaters();
            _waterSpawner.SpawnWaters();

            Debug.Log("GameOverPresenterUIToolkit: Game restarted");
        }

        /// <summary>
        /// シーン内のすべてのコインを削除
        /// </summary>
        private void ClearAllCoins()
        {
            // "Coin"タグを持つすべてのGameObjectを検索して削除
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            foreach (var coin in coins)
            {
                UnityEngine.Object.Destroy(coin);
            }
            Debug.Log($"GameOverPresenterUIToolkit: Cleared {coins.Length} coins");
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
