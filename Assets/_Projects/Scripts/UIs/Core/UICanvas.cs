
using Alchemy.Inspector;
using App.UIs.Views;
using Common.UIs.Core;
using UnityEngine;

namespace App.UIs.Core
{
    public class UICanvas : BaseUICanvas
    {
        [Title("オプション画面")]
        [LabelText("画面本体")]
        [SerializeField] private OptionView _optionView;
        public OptionView OptionView => _optionView;

        [Title("試合画面")]
        [LabelText("画面本体")]
        [SerializeField] private GameView _gameView;
        public GameView GameView => _gameView;

        // public override void Initialize()
        // {
        //     throw new System.NotImplementedException();
        // }
        private void Awake()
        {
            // 各Viewを基底クラスのDictionaryに登録する
            RegisterView(_optionView);
            RegisterView(_gameView);
        }
    }
}
