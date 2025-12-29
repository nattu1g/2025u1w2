using UnityEngine;

namespace App.Features.WaterTank.Coin
{
    /// <summary>
    /// コインの種類と特性を定義するコンポーネント
    /// </summary>
    public class CoinType : MonoBehaviour
    {
        [Header("コインの種類")]
        [SerializeField] private CoinCategory _category = CoinCategory.Normal;
        
        [Header("膨張率設定")]
        [Tooltip("水の膨張率（正の値で膨張、負の値で収縮）")]
        [SerializeField] private float _expansionRate = 0.01f;
        
        public CoinCategory Category => _category;
        public float ExpansionRate => _expansionRate;
    }
    
    /// <summary>
    /// コインのカテゴリー
    /// </summary>
    public enum CoinCategory
    {
        Normal,      // 通常コイン（安価、膨張率大）
        HighDensity, // 高密度コイン（高価、膨張率小）
        Cooling      // 冷却コイン（特別、体積減少効果）
    }
}
