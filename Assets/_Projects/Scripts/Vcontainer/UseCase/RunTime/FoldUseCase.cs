using UnityEngine;
using App.Vcontainer.Entity;
using App.Settings;
using App.Features.WaterTank.Baseline;
using App.Features.WaterTank.Water;
using Cysharp.Threading.Tasks;

namespace App.Vcontainer.UseCase.RunTime
{
    /// <summary>
    /// フォールド処理のビジネスロジック
    /// </summary>
    public class FoldUseCase
    {
        private readonly GameStateEntity _gameState;
        private readonly WaterLevelChecker _waterLevelChecker;
        private readonly BaselineDisplay _baselineDisplay;
        private readonly WaterSpawner _waterSpawner;

        public FoldUseCase(
            GameStateEntity gameState,
            WaterLevelChecker waterLevelChecker,
            BaselineDisplay baselineDisplay,
            WaterSpawner waterSpawner)
        {
            _gameState = gameState;
            _waterLevelChecker = waterLevelChecker;
            _baselineDisplay = baselineDisplay;
            _waterSpawner = waterSpawner;
        }

        /// <summary>
        /// フォールド可能かチェック
        /// </summary>
        public bool CanFold()
        {
            return _waterLevelChecker.IsAboveBaseline;
        }

        /// <summary>
        /// フォールド処理を実行
        /// </summary>
        public async UniTask ExecuteFold()
        {
            if (!CanFold())
            {
                Debug.LogWarning("FoldUseCase: Cannot fold - water level is below baseline");
                return;
            }

            // ポイント計算
            int expansionCount = _gameState.ExpansionCount;
            int foldCount = _gameState.FoldCount.CurrentValue;
            float multiplier = GameConstants.GetFoldMultiplier(foldCount);
            int points = Mathf.RoundToInt(GameConstants.BasePointsPerExpansion * expansionCount * multiplier);

            Debug.Log($"FoldUseCase: Calculating points - Expansions: {expansionCount}, Fold count: {foldCount}, Multiplier: {multiplier}x, Points: {points}");

            // ポイント加算
            _gameState.AddPoints(points);

            // フォールド回数をインクリメント
            _gameState.IncrementFoldCount();

            // 全てのWater/Coinオブジェクトを削除
            await ClearGameObjects();

            // 膨張回数をリセット
            _gameState.ResetExpansionCount();

            // 基準線の高さを更新
            int newFoldCount = _gameState.FoldCount.CurrentValue;
            _baselineDisplay.UpdateHeight(newFoldCount);

            // 水オブジェクトを初期状態で再生成
            await RespawnWater();

            Debug.Log($"FoldUseCase: Fold completed! Points earned: {points}, New fold count: {newFoldCount}");
        }

        /// <summary>
        /// 全てのWater/Coinオブジェクトを削除
        /// </summary>
        private async UniTask ClearGameObjects()
        {
            // 全てのWaterオブジェクトを削除
            _waterSpawner.ClearAllWaters();

            // 全てのCoinオブジェクトを削除
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            foreach (GameObject coin in coins)
            {
                Object.Destroy(coin);
            }
            Debug.Log($"FoldUseCase: Destroyed {coins.Length} coins");

            await UniTask.Yield();
        }

        /// <summary>
        /// 水オブジェクトを初期状態で再生成
        /// </summary>
        private async UniTask RespawnWater()
        {
            // WaterSpawnerで新しいWaterを生成
            _waterSpawner.SpawnWaters();

            Debug.Log("FoldUseCase: Water respawned");
            await UniTask.Yield();
        }
    }
}
