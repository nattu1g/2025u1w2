
using Alchemy.Inspector;
using BBSim.UIs.Views;
using Common.UIs.Core;
using Scripts.UI.Views;
using UnityEngine;

namespace BBSim.UIs.Core
{
    public class UICanvas : BaseUICanvas
    {
        [Title("オプション画面")]
        [LabelText("画面本体")]
        [SerializeField] private OptionView _optionView;
        public OptionView OptionView => _optionView;

        [Title("メッセージ")]
        [LabelText("画面本体")]
        [SerializeField] private MessageView _messageView;
        public MessageView MessageView => _messageView;

        [Title("トレーニングカード画面")]
        [LabelText("画面本体")]
        [SerializeField] private TrainingSelectView _trainingSelectView;
        public TrainingSelectView TrainingSelectView => _trainingSelectView;
        [Title("プレイヤーステータス画面")]
        [LabelText("画面本体")]
        [SerializeField] private PlayerStatusView _playerStatusView;
        public PlayerStatusView PlayerStatusView => _playerStatusView;
        [Title("カレンダー画面")]
        [LabelText("画面本体")]
        [SerializeField] private CalendarView _calendarView;
        public CalendarView CalendarView => _calendarView;
        [Title("試合画面")]
        [LabelText("画面本体")]
        [SerializeField] private GameView _gameView;
        public GameView GameView => _gameView;

        [Title("バトル画面")]
        [LabelText("画面本体")]
        [SerializeField] private BattleView _battleView;
        public BattleView BattleView => _battleView;

        // public override void Initialize()
        // {
        //     throw new System.NotImplementedException();
        // }
        private void Awake()
        {
            // 各Viewを基底クラスのDictionaryに登録する
            RegisterView(_optionView);
            RegisterView(_gameView);
            RegisterView(_battleView);
            RegisterView(_calendarView);
            RegisterView(_messageView);
            RegisterView(_playerStatusView);
            RegisterView(_trainingSelectView);

        }
    }
}
