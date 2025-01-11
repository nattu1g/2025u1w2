using LitMotion;
using LitMotion.Extensions;
using Scripts.Custom;
using UnityEngine;
using UnityEngine.UI;

public class ScaleModalTween : MonoBehaviour
{
    [SerializeField] private Vector2 _scaleFrom;
    [SerializeField] private Vector2 _scaleTowards;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;

    /// <summary>
    /// 対象のモーダル
    /// </summary>
    [SerializeField] private RectTransform _popup;

    // [SerializeField] private CustomButton _showButton;
    // [SerializeField] private CustomButton _hideButton;


    // public void OnEnable()
    // {
    //     _showButton.onClickCallback = () => TweenScaleUp();
    //     _hideButton.onClickCallback = () => TweenScaleDown();

    //     _popup.localScale = _scaleFrom;
    // }

    // public void OnDisable()
    // {
    //     _showButton.onClickCallback -= TweenScaleUp;
    //     _hideButton.onClickCallback -= TweenScaleDown;
    // }

    public void TweenScaleUp()
    {
        LMotion.Create(_scaleFrom, _scaleTowards, _duration)
            .WithEase(_ease)
            .WithOnComplete(() =>
            {
            })
            .BindToLocalScaleXY(_popup)
            .AddTo(gameObject);

    }

    public void TweenScaleDown()
    {
        _popup.localScale = _scaleTowards;
        LMotion.Create(_scaleTowards, _scaleFrom, _duration)
            .WithEase(_ease)
            .BindToLocalScaleXY(_popup)
            .AddTo(gameObject);
    }
}