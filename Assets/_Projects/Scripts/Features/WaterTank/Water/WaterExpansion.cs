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

        // 静的イベント：水が膨張した時に発行
        public static event System.Action OnWaterExpanded;

        private void Awake()
        {
            // 親オブジェクト（Water）の初期スケールを記録
            Transform waterTransform = transform.parent != null ? transform.parent : transform;
            _initialScale = waterTransform.localScale;



            // 親オブジェクトのCollider2Dに摩擦0のPhysicsMaterial2Dを設定
            SetupWaterPhysics(waterTransform);

            // Colliderがトリガーであることを確認
            var collider = GetComponent<Collider2D>();
            if (collider == null)
            {
                Debug.LogError("WaterExpansion: Collider2D component not found!");
            }
            else if (!collider.isTrigger)
            {
                Debug.LogWarning("WaterExpansion: Colliderをトリガーに設定してください");
            }
        }

        /// <summary>
        /// 水の物理設定（摩擦をなくす）
        /// </summary>
        private void SetupWaterPhysics(Transform waterTransform)
        {
            // 親オブジェクトのCollider2Dを取得
            Collider2D waterCollider = waterTransform.GetComponent<Collider2D>();

            if (waterCollider != null && !waterCollider.isTrigger)
            {
                // 摩擦0のPhysicsMaterial2Dを作成
                PhysicsMaterial2D waterMaterial = new PhysicsMaterial2D("WaterMaterial")
                {
                    friction = 0f,      // 摩擦なし
                    bounciness = 0f     // 反発なし
                };

                waterCollider.sharedMaterial = waterMaterial;

            }
            else
            {
                Debug.LogWarning($"WaterExpansion: Collider2D not found on {waterTransform.name} or is a trigger");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {


            // コインとの接触を検知
            if (other.CompareTag("Coin"))
            {


                // 既に接触済みのコインは無視
                if (_touchedCoins.Contains(other.gameObject))
                {

                    return;
                }

                var coinType = other.GetComponent<CoinType>();
                if (coinType != null)
                {

                    ExpandWater(coinType.ExpansionRate);

                    // 接触済みとして記録
                    _touchedCoins.Add(other.gameObject);


                }
                else
                {
                    Debug.LogWarning($"WaterExpansion: CoinType component not found on {other.gameObject.name}");
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 水を膨張（または収縮）させる
        /// </summary>
        /// <param name="rate">膨張率（正の値で膨張、負の値で収縮）</param>
        private void ExpandWater(float rate)
        {
            Vector3 expansion = new Vector3(rate, rate, rate);

            // 親オブジェクト（Water）のスケールを変更
            Transform waterTransform = transform.parent != null ? transform.parent : transform;
            Vector3 newScale = waterTransform.localScale + expansion;

            // スケールの制限
            float maxScaleValue = _initialScale.x * _maxScale;
            float minScaleValue = _initialScale.x * _minScale;

            newScale.x = Mathf.Clamp(newScale.x, minScaleValue, maxScaleValue);
            newScale.y = Mathf.Clamp(newScale.y, minScaleValue, maxScaleValue);
            newScale.z = Mathf.Clamp(newScale.z, minScaleValue, maxScaleValue);

            waterTransform.localScale = newScale;



            // 膨張イベントを発行
            OnWaterExpanded?.Invoke();
        }

        /// <summary>
        /// 水のスケールをリセット
        /// </summary>
        public void ResetWater()
        {
            // 親オブジェクト（Water）のスケールをリセット
            Transform waterTransform = transform.parent != null ? transform.parent : transform;
            waterTransform.localScale = _initialScale;
            _touchedCoins.Clear(); // 接触記録もクリア

        }
    }
}
