using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scripts.UI
{
    public abstract class BaseUICanvas : MonoBehaviour
    {
        // 初期化や共通のUI機能を定義
        // public abstract void Initialize();
        [Title("ボタンアニメーション用のパラメータ")]
        [LabelText("アニメーション前のスケール")]
        [SerializeField] private Vector2 _scaleFrom;
        [LabelText("アニメーション後のスケール")]
        [SerializeField] private Vector2 _scaleTowards;
        [LabelText("アニメーション時間")]
        [SerializeField] private float _duration;
        [LabelText("アニメーションのイージング")]
        [SerializeField] private Ease _ease;
        [LabelText("アニメーション前の透明度")]
        [SerializeField] private float _alphaFrom;
        [LabelText("アニメーション後の透明度")]
        [SerializeField] private float _alphaTowards;


        public virtual void Show(BaseUIView uiView)
        {
            // GameObject が既にアクティブの場合は何もしない
            if (uiView.UiBase.gameObject.activeSelf) return;

            // Action to show the main UI content
            System.Action showUiContent = () =>
            {
                uiView.UiBase.gameObject.SetActive(true);
                LMotion.Create(_scaleFrom, _scaleTowards, _duration)
                    .WithEase(_ease)
                    .BindToLocalScaleXY(uiView.UiBase.gameObject.GetComponent<RectTransform>())
                    .AddTo(uiView.UiBase.gameObject);
            };

            if (uiView.UiBackground != null)
            {
                uiView.UiBackground.gameObject.SetActive(true);
                LMotion.Create(_alphaFrom, _alphaTowards, _duration)
                    .WithOnComplete(showUiContent)
                    .Bind(x => uiView.UiBackground.gameObject.GetComponent<CanvasGroup>().alpha = x)
                    .AddTo(uiView.UiBase.gameObject);
            }
            else
            {
                showUiContent();
            }
        }

        public virtual void Hide(BaseUIView uiView)
        {
            uiView.UiBase.gameObject.GetComponent<RectTransform>().localScale = _scaleTowards;

            LMotion.Create(_scaleTowards, _scaleFrom, _duration)
                .WithEase(_ease)
                .WithOnComplete(() =>
                {
                    uiView.UiBase.gameObject.SetActive(false);

                    if (uiView.UiBackground != null)
                    {
                        LMotion.Create(_alphaTowards, _alphaFrom, _duration)
                            .WithEase(_ease)
                            .WithOnComplete(() =>
                            {
                                uiView.UiBackground.gameObject.SetActive(false);
                            })
                            .Bind(x => uiView.UiBackground.gameObject.GetComponent<CanvasGroup>().alpha = x)
                            .AddTo(uiView.UiBase.gameObject);
                    }
                })
                .BindToLocalScaleXY(uiView.UiBase.gameObject.GetComponent<RectTransform>())
                .AddTo(uiView.UiBase.gameObject);
        }
    }
}

