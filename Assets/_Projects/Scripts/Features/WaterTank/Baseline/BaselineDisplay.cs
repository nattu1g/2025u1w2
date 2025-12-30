using UnityEngine;
using App.Settings;

namespace App.Features.WaterTank.Baseline
{
    /// <summary>
    /// 基準線の表示を管理するコンポーネント
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BaselineDisplay : MonoBehaviour
    {
        [Header("設定")]
        [Tooltip("現在のフォールド回数")]
        [SerializeField] private int _foldCount = 0;
        
        private SpriteRenderer _spriteRenderer;
        private float _currentHeight;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // 初期の高さを設定
            UpdateHeight(_foldCount);
        }

        /// <summary>
        /// フォールド回数に応じて基準線の高さを更新
        /// </summary>
        /// <param name="foldCount">フォールド回数</param>
        public void UpdateHeight(int foldCount)
        {
            _foldCount = foldCount;
            _currentHeight = GameConstants.GetBaselineHeight(foldCount);
            
            // Y座標を更新
            Vector3 pos = transform.position;
            pos.y = _currentHeight;
            transform.position = pos;
            
            Debug.Log($"BaselineDisplay: Height updated to {_currentHeight} (Fold count: {foldCount})");
        }

        /// <summary>
        /// 現在の基準線の高さを取得
        /// </summary>
        public float CurrentHeight => _currentHeight;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Scene Viewで基準線を可視化
            Gizmos.color = Color.red;
            Vector3 start = transform.position - new Vector3(10f, 0, 0);
            Vector3 end = transform.position + new Vector3(10f, 0, 0);
            Gizmos.DrawLine(start, end);
        }
#endif
    }
}
