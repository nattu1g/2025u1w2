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

        [Header("物理設定")]
        [Tooltip("コイン生成時の初期速度（下方向）")]
        [SerializeField] private float _initialVelocity = 0f;

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
    }
}
