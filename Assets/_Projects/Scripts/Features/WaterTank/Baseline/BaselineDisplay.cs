using UnityEngine;
using App.Settings;
using TMPro;

namespace App.Features.WaterTank.Baseline
{
    /// <summary>
    /// BASE LINEの表示を管理するコンポーネント
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BaselineDisplay : MonoBehaviour
    {
        [Header("設定")]
        [Tooltip("現在のフォールド回数")]
        [SerializeField] private int _foldCount = 0;

        [Header("UI")]
        [Tooltip("BASE LINEのラベル（TextMeshPro）")]
        [SerializeField] private TextMeshProUGUI _labelText;

        private SpriteRenderer _spriteRenderer;
        private float _currentHeight;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // ラベルテキストを初期化
            if (_labelText != null)
            {
                UpdateLabelText(1.0f); // 初期倍率は1.0
                _labelText.fontSize = 1;
                _labelText.alignment = TMPro.TextAlignmentOptions.Left;
                _labelText.color = Color.red;
            }

            // 初期の高さを設定
            UpdateHeight(_foldCount);
        }

        /// <summary>
        /// フォールド回数に応じてBASE LINEの高さを更新
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

            // ラベルの位置も更新
            if (_labelText != null)
            {
                Vector3 labelPos = _labelText.transform.position;
                labelPos.y = _currentHeight;
                _labelText.transform.position = labelPos;
            }

            Debug.Log($"BaselineDisplay: Height updated to {_currentHeight} (Fold count: {foldCount})");
        }

        /// <summary>
        /// 現在のBASE LINEの高さを取得
        /// </summary>
        public float CurrentHeight => _currentHeight;

        /// <summary>
        /// ポイント倍率を更新
        /// </summary>
        /// <param name="isAboveBaseline">BASE LINEを超えているか</param>
        /// <param name="foldCount">現在のフォールド回数</param>
        public void UpdateMultiplier(bool isAboveBaseline, int foldCount)
        {
            float multiplier;
            if (isAboveBaseline)
            {
                // BASE LINEを超えている場合、フォールド回数に応じた倍率
                multiplier = GameConstants.GetFoldMultiplier(foldCount);
            }
            else
            {
                // BASE LINE未到達の場合は1.0倍
                multiplier = 1.0f;
            }

            UpdateLabelText(multiplier);
        }

        /// <summary>
        /// ラベルテキストを更新
        /// </summary>
        private void UpdateLabelText(float multiplier)
        {
            if (_labelText != null)
            {
                _labelText.text = $"BASE LINE (×{multiplier:F1})";
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Scene ViewでBASE LINEを可視化
            Gizmos.color = Color.red;
            Vector3 start = transform.position - new Vector3(10f, 0, 0);
            Vector3 end = transform.position + new Vector3(10f, 0, 0);
            Gizmos.DrawLine(start, end);
        }
#endif
    }
}
