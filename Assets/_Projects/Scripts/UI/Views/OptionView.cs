using Alchemy.Inspector;
using Scripts.UI;
using Scripts.UI.Component;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Views
{
    public class OptionView : BaseUIView
    {
        [Title("オプション項目")]
        [LabelText("BGMマイナス")]
        [SerializeField] private CustomButton _bgmMinusButton;
        public CustomButton BgmMinusButton => _bgmMinusButton;
        [LabelText("BGMプラス")]
        [SerializeField] private CustomButton _bgmPlusButton;
        public CustomButton BgmPlusButton => _bgmPlusButton;
        [LabelText("SEマイナス")]
        [SerializeField] private CustomButton _seMinusButton;
        public CustomButton SeMinusButton => _seMinusButton;
        [LabelText("SEプラス")]
        [SerializeField] private CustomButton _sePlusButton;
        public CustomButton SePlusButton => _sePlusButton;
        [LabelText("BGM量")]
        [SerializeField] private TextMeshProUGUI _bgmText;
        public void SetBgmText(float value) => _bgmText.text = value.ToString();
        [LabelText("SE量")]
        [SerializeField] private TextMeshProUGUI _seText;
        public void SetSeText(float value) => _seText.text = value.ToString();

    }
}
