using UnityEngine;
using App.Features.WaterTank.Coin;
using System.Collections.Generic;

namespace App.Features.WaterTank.Water
{
    /// <summary>
    /// 水の膨張ロジックを管理するコンポーネント
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class WaterExpansion : MonoBehaviour
    {
        [Header("膨張設定")]
        [Tooltip("最大スケール（これ以上膨張しない）")]
        [SerializeField] private float _maxScale = 5.0f;

        [Tooltip("最小スケール（これ以下収縮しない）")]
        [SerializeField] private float _minScale = 0.5f;

        private Vector3 _initialScale;

        // 既に接触したコインを記録（初回のみ処理するため）
        private HashSet<GameObject> _touchedCoins = new HashSet<GameObject>();

        private void Awake()
        {
            _initialScale = transform.localScale;

            // Colliderがトリガーであることを確認
            var collider = GetComponent<Collider2D>();
            if (!collider.isTrigger)
            {
                Debug.LogWarning("WaterExpansion: Colliderをトリガーに設定してください");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"WaterExpansion: OnTriggerEnter2D called with {other.gameObject.name}, Tag: {other.tag}");

            // コインとの接触を検知
            if (other.CompareTag("Coin"))
            {
                Debug.Log($"WaterExpansion: Coin detected! {other.gameObject.name}");

                // 既に接触済みのコインは無視
                if (_touchedCoins.Contains(other.gameObject))
                {
                    Debug.Log($"WaterExpansion: Coin already touched, ignoring. {other.gameObject.name}");
                    return;
                }

                var coinType = other.GetComponent<CoinType>();
                if (coinType != null)
                {
                    Debug.Log($"WaterExpansion: CoinType found! ExpansionRate: {coinType.ExpansionRate}");
                    ExpandWater(coinType.ExpansionRate);

                    // 接触済みとして記録
                    _touchedCoins.Add(other.gameObject);

                    Debug.Log($"コイン初回接触: {other.gameObject.name}");
                }
                else
                {
                    Debug.LogWarning($"WaterExpansion: CoinType component not found on {other.gameObject.name}");
                }
            }
            else
            {
                Debug.Log($"WaterExpansion: Not a Coin. Tag is '{other.tag}' (expected 'Coin')");
            }
        }

        /// <summary>
        /// 水を膨張（または収縮）させる
        /// </summary>
        /// <param name="rate">膨張率（正の値で膨張、負の値で収縮）</param>
        private void ExpandWater(float rate)
        {
            Vector3 expansion = new Vector3(rate, rate, rate);
            Vector3 newScale = transform.localScale + expansion;

            // スケールの制限
            float maxScaleValue = _initialScale.x * _maxScale;
            float minScaleValue = _initialScale.x * _minScale;

            newScale.x = Mathf.Clamp(newScale.x, minScaleValue, maxScaleValue);
            newScale.y = Mathf.Clamp(newScale.y, minScaleValue, maxScaleValue);
            newScale.z = Mathf.Clamp(newScale.z, minScaleValue, maxScaleValue);

            transform.localScale = newScale;

            Debug.Log($"水が膨張しました: {transform.localScale}");
        }

        /// <summary>
        /// 水のスケールをリセット
        /// </summary>
        public void ResetWater()
        {
            transform.localScale = _initialScale;
            _touchedCoins.Clear(); // 接触記録もクリア
            Debug.Log("水をリセットしました");
        }
    }
}
