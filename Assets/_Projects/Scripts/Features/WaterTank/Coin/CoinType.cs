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
        [SerializeField] private float _friction = 0.05f;

        [Tooltip("反発係数（跳ねやすさ）")]
        [Range(0f, 1f)]
        [SerializeField] private float _bounciness = 0.3f;

        [Header("積み上がり防止設定")]
        [Tooltip("コライダーを少し小さくして接触面積を減らす")]
        [Range(0.8f, 1.0f)]
        [SerializeField] private float _colliderScale = 0.95f;

        [Tooltip("ランダムな初期回転を追加（積み上がり防止）")]
        [SerializeField] private bool _randomInitialRotation = true;

        [Header("Rigidbody2D設定")]
        [Tooltip("質量")]
        [SerializeField] private float _mass = 0.5f;

        [Tooltip("線形抵抗（空気抵抗）")]
        [SerializeField] private float _linearDrag = 0.5f;

        [Tooltip("角度抵抗（回転抵抗）")]
        [SerializeField] private float _angularDrag = 0.5f;

        public CoinCategory Category => _category;
        public float ExpansionRate => _expansionRate;
        
        /// <summary>
        /// 膨張率を外部から設定（CoinDefinitionの値を使用する場合）
        /// </summary>
        public void SetExpansionRate(float expansionRate)
        {
            _expansionRate = expansionRate;
            Debug.Log($"CoinType: ExpansionRate set to {_expansionRate}");
        }

        private void Awake()
        {
            SetupPhysics();
        }

        /// <summary>
        /// 物理設定を適用
        /// </summary>
        private void SetupPhysics()
        {
            // ランダムな初期回転を追加（積み上がり防止）
            if (_randomInitialRotation)
            {
                float randomAngle = Random.Range(0f, 360f);
                transform.rotation = Quaternion.Euler(0, 0, randomAngle);
            }

            // PhysicsMaterial2Dを作成（より滑りやすく）
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

                // CircleCollider2Dの場合、半径を少し小さくする
                if (collider is CircleCollider2D circleCollider)
                {
                    float originalRadius = circleCollider.radius;
                    circleCollider.radius = originalRadius * _colliderScale;
                    Debug.Log($"CircleCollider2D radius adjusted: {originalRadius} -> {circleCollider.radius}");
                }
                // BoxCollider2Dの場合、サイズを少し小さくする
                else if (collider is BoxCollider2D boxCollider)
                {
                    Vector2 originalSize = boxCollider.size;
                    boxCollider.size = originalSize * _colliderScale;
                    Debug.Log($"BoxCollider2D size adjusted: {originalSize} -> {boxCollider.size}");
                }
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

                // 補間を有効化（滑らかな動き）
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
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
