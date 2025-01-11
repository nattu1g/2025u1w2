using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Custom
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

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickCallback?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // ボタンを縮小 
            LMotion.Create(_scaleFrom, _scaleTowards, _duration)
                .BindToLocalScale(this.transform);

            // ボタンを透明に
            LMotion.Create(_alphaFrom, _alphaTowards, _duration)
                .BindToColorA(this.GetComponent<Image>());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // ボタンを元に戻す
            LMotion.Create(_scaleTowards, _scaleFrom, _duration)
                .BindToLocalScale(this.transform);
            LMotion.Create(_alphaTowards, _alphaFrom, _duration)
                .BindToColorA(this.GetComponent<Image>());
        }
    }
}
