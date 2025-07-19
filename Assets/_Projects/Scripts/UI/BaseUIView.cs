using Alchemy.Inspector;
using Scripts.Custom;
using UnityEngine;

namespace Scripts.UI
{
    public abstract class BaseUIView : MonoBehaviour
    {
        [Title("UI画面")]
        [LabelText("UI本体")]
        [SerializeField] private CanvasGroup _uiBase;
        public CanvasGroup UiBase => _uiBase;
        [LabelText("UI背景")]
        [SerializeField] private CanvasGroup _uiBackground;
        public CanvasGroup UiBackground => _uiBackground;
        [LabelText("表示ボタン")]
        [SerializeField] private CustomButton _showButton;
        public CustomButton ShowButton => _showButton;
        [LabelText("非表示ボタン")]
        [SerializeField] private CustomButton _hideButton;
        public CustomButton HideButton => _hideButton;
    }
}
