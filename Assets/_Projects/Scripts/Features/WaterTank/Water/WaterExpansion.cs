using UnityEngine;
using App.Features.WaterTank.Coin;

namespace App.Features.WaterTank.Water
{
    /// <summary>
    /// 水の膨張ロジックを管理するコンポーネント
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WaterExpansion : MonoBehaviour
    {
        [Header("膨張設定")]
        [Tooltip("最大スケール（これ以上膨張しない）")]
        [SerializeField] private float _maxScale = 5.0f;
        
        [Tooltip("最小スケール（これ以下収縮しない）")]
        [SerializeField] private float _minScale = 0.5f;
        
        private Vector3 _initialScale;
        
        private void Awake()
        {
            _initialScale = transform.localScale;
            
            // Colliderがトリガーであることを確認
            var collider = GetComponent<Collider>();
            if (!collider.isTrigger)
            {
                Debug.LogWarning("WaterExpansion: Colliderをトリガーに設定してください");
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // コインとの接触を検知
            if (other.CompareTag("Coin"))
            {
                var coinType = other.GetComponent<CoinType>();
                if (coinType != null)
                {
                    ExpandWater(coinType.ExpansionRate);
                }
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
        }
    }
}
