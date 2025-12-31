using System;
using App.Events;
using App.Vcontainer.Entity;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;

namespace App.Vcontainer.UseCase.RunTime
{
    /// <summary>
    /// ゲームオーバー処理のビジネスロジック
    /// </summary>
    public class GameOverUseCase : IDisposable
    {
        private readonly GameStateEntity _gameState;
        private readonly IDisposable _subscription;

        public GameOverUseCase(
            GameStateEntity gameState,
            ISubscriber<GameOverEvent> gameOverSubscriber)
        {
            _gameState = gameState;

            // ゲームオーバーイベントを購読
            _subscription = gameOverSubscriber.Subscribe(OnGameOver);
        }

        /// <summary>
        /// ゲームオーバー処理
        /// </summary>
        private void OnGameOver(GameOverEvent evt)
        {
            Debug.Log($"GameOverUseCase: Game Over! Final Score: {_gameState.Points.CurrentValue}");

            // ゲームオーバー状態を設定
            _gameState.SetGameOver(true);
        }

        /// <summary>
        /// ゲームをリトライ（リセット）
        /// </summary>
        public async UniTask RetryGame()
        {
            Debug.Log("GameOverUseCase: Retrying game...");

            // ゲーム状態をリセット
            _gameState.Reset();

            // 少し待機（演出用）
            await UniTask.Delay(100);

            Debug.Log("GameOverUseCase: Game reset complete");
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
