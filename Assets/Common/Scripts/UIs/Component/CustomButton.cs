using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UIs.Component
{
    public class CustomButton : MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [Title("ボタンアニメーション用のパラメータ")]
        [LabelText("アニメーション前のスケール")]
        [SerializeField] private Vector3 _scaleFrom = Vector3.one;
        [LabelText("アニメーション後のスケール")]
        [SerializeField] private Vector3 _scaleTowards = Vector3.one * 0.95f;
        [LabelText("アニメーション時間")]
        [SerializeField] private float _duration = 0.24f;
        [LabelText("アニメーション前の透明度")]
        [SerializeField] private float _alphaFrom = 1f;
        [LabelText("アニメーション後の透明度")]
        [SerializeField] private float _alphaTowards = 0.8f;
        public System.Action onClickCallback;

        private bool _isIntractable = true;
        private Image _buttonImage;
        private Color _originalButtonColorRGB; // ボタンの元のRGB値を保存

        private void Awake()
        {
            _buttonImage = GetComponent<Image>();
            if (_buttonImage != null)
            {
                // 初期状態のRGB値を保存
                _originalButtonColorRGB = new Color(_buttonImage.color.r, _buttonImage.color.g, _buttonImage.color.b, 1f);
                // 初期アルファを _alphaFrom に設定（インタラクティブなボタンの静止時アルファ）
                Color currentColor = _buttonImage.color;
                currentColor.a = _alphaFrom;
                _buttonImage.color = currentColor;
            }
            else
            {
                _originalButtonColorRGB = Color.white; // フォールバック
            }
        }

        public void SetIntractable(bool intractable)
        {
            _isIntractable = intractable;

            // 無効化時は色を変更（暗くする）
            if (_buttonImage != null)
            {
                if (intractable)
                {
                    // 元のRGB値と _alphaFrom (静止時アルファ) を使って色を復元
                    _buttonImage.color = new Color(_originalButtonColorRGB.r, _originalButtonColorRGB.g, _originalButtonColorRGB.b, _alphaFrom);
                }
                else
                {
                    // 非インタラクティブ時の色（例: グレーアウト）
                    _buttonImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
        }

        public void ClearClickListeners()
        {
            onClickCallback = null;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isIntractable) return;
            onClickCallback?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isIntractable) return;

            // ボタンを縮小 
            LMotion.Create(_scaleFrom, _scaleTowards, _duration)
                .BindToLocalScale(this.transform);

            // ボタンのアルファ値を変更
            if (_buttonImage != null)
            {
                LMotion.Create(_alphaFrom, _alphaTowards, _duration)
                    .BindToColorA(_buttonImage);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // OnPointerUpは、ボタンが押下中に非インタラクティブになった場合でも発火する可能性があるため、
            // _isIntractable のチェックはここでは必須ではないかもしれません（常に元の状態に戻すため）。
            // ボタンを元に戻す
            LMotion.Create(_scaleTowards, _scaleFrom, _duration)
                .BindToLocalScale(this.transform);
            if (_buttonImage != null)
            {
                LMotion.Create(_alphaTowards, _alphaFrom, _duration)
                    .BindToColorA(_buttonImage);
            }
        }
    }
}
