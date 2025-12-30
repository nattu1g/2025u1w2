using UnityEngine;

namespace App.Features.WaterTank.Background
{
    /// <summary>
    /// カメラのサイズに合わせて背景を自動調整するコンポーネント
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundScaler : MonoBehaviour
    {
        [Header("設定")]
        [Tooltip("参照するカメラ（nullの場合はMain Cameraを使用）")]
        [SerializeField] private Camera _targetCamera;

        [Tooltip("背景の余白（カメラより少し大きくする）")]
        [SerializeField] private float _padding = 0.5f;

        private SpriteRenderer _spriteRenderer;
        private Vector2 _lastCameraSize;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_targetCamera == null)
            {
                _targetCamera = Camera.main;
            }

            if (_targetCamera == null)
            {
                Debug.LogError("BackgroundScaler: Camera not found!");
                return;
            }

            // 初回調整
            AdjustBackgroundSize();
        }

        private void LateUpdate()
        {
            // カメラサイズが変わったら再調整
            Vector2 currentCameraSize = GetCameraSize();
            if (currentCameraSize != _lastCameraSize)
            {
                AdjustBackgroundSize();
            }
        }

        /// <summary>
        /// 背景のサイズをカメラに合わせて調整
        /// </summary>
        private void AdjustBackgroundSize()
        {
            if (_targetCamera == null || _spriteRenderer == null || _spriteRenderer.sprite == null)
            {
                return;
            }

            // カメラの表示範囲を取得
            float cameraHeight = _targetCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * _targetCamera.aspect;

            // 余白を追加
            cameraHeight += _padding * 2f;
            cameraWidth += _padding * 2f;

            // スプライトのサイズを取得
            Sprite sprite = _spriteRenderer.sprite;
            float spriteWidth = sprite.bounds.size.x;
            float spriteHeight = sprite.bounds.size.y;

            // スケールを計算（カメラ全体を覆うように）
            float scaleX = cameraWidth / spriteWidth;
            float scaleY = cameraHeight / spriteHeight;

            // 大きい方のスケールを使用（画面全体を覆う）
            float scale = Mathf.Max(scaleX, scaleY);

            transform.localScale = new Vector3(scale, scale, 1f);

            // カメラサイズを記録
            _lastCameraSize = new Vector2(cameraWidth, cameraHeight);
        }

        /// <summary>
        /// 現在のカメラサイズを取得
        /// </summary>
        private Vector2 GetCameraSize()
        {
            if (_targetCamera == null) return Vector2.zero;

            float height = _targetCamera.orthographicSize * 2f;
            float width = height * _targetCamera.aspect;
            return new Vector2(width, height);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_targetCamera == null) return;

            // カメラの表示範囲を可視化
            float height = _targetCamera.orthographicSize * 2f;
            float width = height * _targetCamera.aspect;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_targetCamera.transform.position, new Vector3(width, height, 0));
        }
#endif
    }
}
