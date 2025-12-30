using App.Features.WaterTank.Coin;
using App.SOs;
using App.Vcontainer.Entity;
using UnityEngine;

namespace App.Vcontainer.UseCase.RunTime
{
    /// <summary>
    /// コイン投下のビジネスロジックを管理するUseCase
    /// </summary>
    public class CoinDropUseCase
    {
        private readonly GameStateEntity _gameState;
        private readonly CoinSpawner _coinSpawner;

        public CoinDropUseCase(GameStateEntity gameState, CoinSpawner coinSpawner)
        {
            _gameState = gameState;
            _coinSpawner = coinSpawner;
        }

        /// <summary>
        /// 投下位置を左右に移動
        /// </summary>
        /// <param name="direction">移動方向（-1: 左, 0: 停止, 1: 右）</param>
        public void UpdateDropPosition(float direction)
        {
            _coinSpawner.UpdatePositionX(direction);
        }

        /// <summary>
        /// 投下位置を直接設定
        /// </summary>
        /// <param name="x">X座標</param>
        public void SetDropPosition(float x)
        {
            _coinSpawner.SetPositionX(x);
        }

        /// <summary>
        /// 現在の投下位置を取得
        /// </summary>
        public float GetCurrentDropPosition()
        {
            return _coinSpawner.CurrentX;
        }

        /// <summary>
        /// コインを投下する
        /// </summary>
        /// <param name="coinDefinition">投下するコインの定義</param>
        /// <returns>投下に成功したかどうか</returns>
        public bool DropCoin(CoinDefinition coinDefinition)
        {
            if (coinDefinition == null)
            {
                Debug.LogError("CoinDropUseCase: coinDefinition is null");
                return false;
            }

            // ゲームオーバー中は投下不可
            if (_gameState.IsGameOver.CurrentValue)
            {
                Debug.LogWarning("CoinDropUseCase: Cannot drop coin. Game is over.");
                return false;
            }

            // ポイントが足りるか確認
            if (!_gameState.SpendPoints(coinDefinition.Price))
            {
                Debug.LogWarning($"CoinDropUseCase: Not enough points to drop {coinDefinition.CoinName}");
                return false;
            }

            // コインを生成
            GameObject coin = _coinSpawner.SpawnCoin(coinDefinition.CoinPrefab);

            if (coin == null)
            {
                // 生成失敗時はポイントを返却
                _gameState.AddPoints(coinDefinition.Price);
                Debug.LogError("CoinDropUseCase: Failed to spawn coin");
                return false;
            }

            // コインの種類を設定
            CoinType coinType = coin.GetComponent<CoinType>();
            if (coinType != null)
            {
                // CoinDefinitionから膨張率を設定
                // 注意: CoinTypeは既にInspectorで設定されている可能性があるため、
                // 必要に応じてCoinDefinitionの値で上書きする
                Debug.Log($"CoinDropUseCase: Dropped {coinDefinition.CoinName} (ExpansionRate: {coinDefinition.ExpansionRate})");
            }

            return true;
        }

        /// <summary>
        /// 無料でコインを投下する（デバッグ用）
        /// </summary>
        public GameObject DropCoinFree(CoinDefinition coinDefinition)
        {
            if (coinDefinition == null)
            {
                Debug.LogError("CoinDropUseCase: coinDefinition is null");
                return null;
            }

            return _coinSpawner.SpawnCoin(coinDefinition.CoinPrefab);
        }
    }
}
