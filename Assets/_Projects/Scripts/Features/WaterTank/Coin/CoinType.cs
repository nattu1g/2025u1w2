using UnityEngine;

namespace App.Features.WaterTank.Coin
{
    /// <summary>
    /// コインの種類と特性を定義するコンポーネント
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CoinType : MonoBehaviour
    {
        [Header("コインの種類")]
        [SerializeField] private CoinCategory _category = CoinCategory.Normal;
        
        [Header("膨張率設定")]
        [Tooltip("水の膨張率（正の値で膨張、負の値で収縮）")]
        [SerializeField] private float _expansionRate = 0.01f;
        
        [Header("物理設定")]
        [Tooltip("摩擦係数（0に近いほど滑りやすい）")]
        [Range(0f, 1f)]
        [SerializeField] private float _friction = 0.1f;
        
        [Tooltip("反発係数（0に近いほど跳ねない）")]
        [Range(0f, 1f)]
        [SerializeField] private float _bounciness = 0.2f;
        
        [Header("Rigidbody2D設定")]
        [Tooltip("質量")]
        [SerializeField] private float _mass = 0.5f;
        
        [Tooltip("線形抵抗（空気抵抗）")]
        [SerializeField] private float _linearDrag = 0.5f;
        
        [Tooltip("角度抵抗（回転抵抗）")]
        [SerializeField] private float _angularDrag = 0.5f;
        
        public CoinCategory Category => _category;
        public float ExpansionRate => _expansionRate;
        
        private void Awake()
        {
            SetupPhysics();
        }
        
        /// <summary>
        /// 物理設定を適用
        /// </summary>
        private void SetupPhysics()
        {
            // PhysicsMaterial2Dを作成
            PhysicsMaterial2D physicsMaterial = new PhysicsMaterial2D("CoinMaterial")
            {
                friction = _friction,
                bounciness = _bounciness
            };
            
            // Collider2Dに適用
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.sharedMaterial = physicsMaterial;
            }
            
            // Rigidbody2Dの設定
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.mass = _mass;
                rb.linearDamping = _linearDrag;
                rb.angularDamping = _angularDrag;
                rb.gravityScale = 1.0f;
                
                // 連続衝突検出を有効化（高速移動時の貫通防止）
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
        }
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
