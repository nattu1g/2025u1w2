using R3;
using UnityEngine;

namespace App.Vcontainer.Entity
{
    /// <summary>
    /// ゲームの状態を管理するEntity
    /// </summary>
    public class GameStateEntity
    {
        // 所持ポイント
        private readonly ReactiveProperty<int> _points = new(0);
        public ReadOnlyReactiveProperty<int> Points => _points;

        // 現在の水位レベル（0.0 ~ 1.0）
        private readonly ReactiveProperty<float> _waterLevel = new(0f);
        public ReadOnlyReactiveProperty<float> WaterLevel => _waterLevel;

        // ゲームオーバー状態
        private readonly ReactiveProperty<bool> _isGameOver = new(false);
        public ReadOnlyReactiveProperty<bool> IsGameOver => _isGameOver;

        // フォールド回数
        private readonly ReactiveProperty<int> _foldCount = new(0);
        public ReadOnlyReactiveProperty<int> FoldCount => _foldCount;

        // 現在のラウンドでの水の膨張回数
        private int _expansionCount = 0;
        public int ExpansionCount => _expansionCount;

        // 含み益ポイント（フォールド時に獲得できるポイント）
        private readonly ReactiveProperty<int> _pendingPoints = new(0);
        public ReadOnlyReactiveProperty<int> PendingPoints => _pendingPoints;

        /// <summary>
        /// ポイントを追加
        /// </summary>
        public void AddPoints(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"GameStateEntity: Negative points amount: {amount}");
                return;
            }

            _points.Value += amount;
            Debug.Log($"GameStateEntity: Points added: {amount}, Total: {_points.Value}");
        }

        /// <summary>
        /// ポイントを消費
        /// </summary>
        /// <returns>消費に成功したかどうか</returns>
        public bool SpendPoints(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"GameStateEntity: Negative points amount: {amount}");
                return false;
            }

            if (_points.Value < amount)
            {
                Debug.LogWarning($"GameStateEntity: Not enough points. Required: {amount}, Current: {_points.Value}");
                return false;
            }

            _points.Value -= amount;
            Debug.Log($"GameStateEntity: Points spent: {amount}, Remaining: {_points.Value}");
            return true;
        }

        /// <summary>
        /// 水位レベルを設定（0.0 ~ 1.0）
        /// </summary>
        public void SetWaterLevel(float level)
        {
            _waterLevel.Value = Mathf.Clamp01(level);
        }

        /// <summary>
        /// ゲームオーバーを設定
        /// </summary>
        public void SetGameOver(bool isGameOver)
        {
            _isGameOver.Value = isGameOver;
            if (isGameOver)
            {
                Debug.Log("GameStateEntity: Game Over!");
            }
        }

        /// <summary>
        /// ゲーム状態をリセット
        /// </summary>
        public void Reset()
        {
            _points.Value = 0;
            _waterLevel.Value = 0f;
            _isGameOver.Value = false;
            _foldCount.Value = 0;
            _expansionCount = 0;
            _pendingPoints.Value = 0;
            Debug.Log("GameStateEntity: Reset");
        }

        /// <summary>
        /// 水の膨張回数をインクリメント
        /// </summary>
        public void IncrementExpansionCount()
        {
            _expansionCount++;
            Debug.Log($"GameStateEntity: Expansion count: {_expansionCount}");
        }

        /// <summary>
        /// フォールド回数をインクリメント
        /// </summary>
        public void IncrementFoldCount()
        {
            _foldCount.Value++;
            Debug.Log($"GameStateEntity: Fold count: {_foldCount.Value}");
        }

        /// <summary>
        /// 膨張回数をリセット（新しいラウンド開始時）
        /// </summary>
        public void ResetExpansionCount()
        {
            _expansionCount = 0;
            _pendingPoints.Value = 0;
            Debug.Log("GameStateEntity: Expansion count and pending points reset");
        }

        /// <summary>
        /// 含み益ポイントを更新
        /// </summary>
        /// <param name="foldCount">現在のフォールド回数</param>
        public void UpdatePendingPoints(int foldCount)
        {
            float multiplier = App.Settings.GameConstants.GetFoldMultiplier(foldCount);
            int points = Mathf.RoundToInt(App.Settings.GameConstants.BasePointsPerExpansion * _expansionCount * multiplier);
            _pendingPoints.Value = points;
        }
    }
}
