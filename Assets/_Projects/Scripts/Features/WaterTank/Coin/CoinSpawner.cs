using UnityEngine;

namespace App.Features.WaterTank.Coin
{
    /// <summary>
    /// コインを生成・投下するコンポーネント
    /// </summary>
    public class CoinSpawner : MonoBehaviour
    {
        [Header("生成設定")]
        [SerializeField] private Transform _spawnPoint;

        [Header("移動設定")]
        [Tooltip("X軸方向の移動範囲（±この値の範囲で移動可能）")]
        [SerializeField] private float _moveRangeX = 5.0f;
        
        [Tooltip("移動速度（秒間の移動量）")]
        [SerializeField] private float _moveSpeed = 5.0f;

        [Header("物理設定")]
        [Tooltip("コイン生成時の初期速度（下方向）")]
        [SerializeField] private float _initialVelocity = 0f;
        
        /// <summary>
        /// 現在のX位置
        /// </summary>
        public float CurrentX => _spawnPoint != null ? _spawnPoint.position.x : transform.position.x;
        
        /// <summary>
        /// 移動範囲の最小X座標
        /// </summary>
        public float MinX => (_spawnPoint != null ? _spawnPoint.position.x : transform.position.x) - _moveRangeX;
        
        /// <summary>
        /// 移動範囲の最大X座標
        /// </summary>
        public float MaxX => (_spawnPoint != null ? _spawnPoint.position.x : transform.position.x) + _moveRangeX;

        /// <summary>
        /// X位置を設定（絶対座標）
        /// </summary>
        /// <param name="x">設定するX座標</param>
        public void SetPositionX(float x)
        {
            if (_spawnPoint == null) return;
            
            Vector3 pos = _spawnPoint.position;
            float centerX = transform.position.x;
            pos.x = Mathf.Clamp(x, centerX - _moveRangeX, centerX + _moveRangeX);
            _spawnPoint.position = pos;
        }

        /// <summary>
        /// X位置を相対的に移動
        /// </summary>
        /// <param name="deltaX">移動量</param>
        public void MoveX(float deltaX)
        {
            SetPositionX(CurrentX + deltaX);
        }
        
        /// <summary>
        /// 移動速度を考慮してX位置を更新（Update内で呼び出し用）
        /// </summary>
        /// <param name="direction">移動方向（-1: 左, 0: 停止, 1: 右）</param>
        public void UpdatePositionX(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return;
            
            float deltaX = direction * _moveSpeed * Time.deltaTime;
            MoveX(deltaX);
        }

        /// <summary>
        /// コインを生成して投下する
        /// </summary>
        /// <param name="coinPrefab">生成するコインのプレハブ</param>
        /// <returns>生成されたコインのGameObject</returns>
        public GameObject SpawnCoin(GameObject coinPrefab)
        {
            if (coinPrefab == null)
            {
                Debug.LogError("CoinSpawner: coinPrefab is null");
                return null;
            }

            Vector3 spawnPosition = _spawnPoint != null ? _spawnPoint.position : transform.position;

            // コインを生成
            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            // 初期速度を設定
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null && _initialVelocity > 0)
            {
                rb.linearVelocity = Vector2.down * _initialVelocity;
            }

            Debug.Log($"CoinSpawner: Spawned coin at {spawnPosition}");

            return coin;
        }

        /// <summary>
        /// 指定位置にコインを生成
        /// </summary>
        public GameObject SpawnCoinAtPosition(GameObject coinPrefab, Vector3 position)
        {
            if (coinPrefab == null)
            {
                Debug.LogError("CoinSpawner: coinPrefab is null");
                return null;
            }

            GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

            // 初期速度を設定
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null && _initialVelocity > 0)
            {
                rb.linearVelocity = Vector2.down * _initialVelocity;
            }

            return coin;
        }
        
        private void OnDrawGizmosSelected()
        {
            // 移動範囲を可視化
            if (_spawnPoint == null) return;
            
            Vector3 center = transform.position;
            Vector3 leftEdge = new Vector3(center.x - _moveRangeX, _spawnPoint.position.y, _spawnPoint.position.z);
            Vector3 rightEdge = new Vector3(center.x + _moveRangeX, _spawnPoint.position.y, _spawnPoint.position.z);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(leftEdge + Vector3.up * 0.5f, leftEdge - Vector3.up * 0.5f);
            Gizmos.DrawLine(rightEdge + Vector3.up * 0.5f, rightEdge - Vector3.up * 0.5f);
            Gizmos.DrawLine(leftEdge, rightEdge);
        }
    }
}
