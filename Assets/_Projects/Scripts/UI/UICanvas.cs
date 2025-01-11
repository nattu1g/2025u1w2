
using Alchemy.Inspector;
using Scripts.Custom;
using UnityEngine;

namespace Scripts.UI
{
    public class UICanvas : BaseUICanvas
    {
        [Title("オプション画面")]
        [LabelText("画面本体")]
        [SerializeField] private OptionView _optionView;
        public OptionView OptionView => _optionView;



        // public override void Initialize()
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}
