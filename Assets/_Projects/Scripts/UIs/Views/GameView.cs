using Alchemy.Inspector;
using Common.UIs.Core;
using TMPro;
using UnityEngine;

namespace App.UIs.Views
{
    public class GameView : BaseUIView
    {
        [Title("試合の可視化UI")]
        [LabelText("プレイヤー点数")]
        [SerializeField] private TextMeshProUGUI _playerScore;
        public TextMeshProUGUI PlayerScore => _playerScore;

        [LabelText("相手点数")]
        [SerializeField] private TextMeshProUGUI _opponentScore;
        public TextMeshProUGUI OpponentScore => _opponentScore;
        [LabelText("時間")]
        [SerializeField] private TextMeshProUGUI _timeText;
        public TextMeshProUGUI TimeText => _timeText;
        [LabelText("プレイヤーイベント")]
        [SerializeField] private TextMeshProUGUI _playerEvent;
        public TextMeshProUGUI PlayerEvent => _playerEvent;
        [LabelText("相手イベント")]
        [SerializeField] private TextMeshProUGUI _opponentEvent;
        public TextMeshProUGUI OpponentEvent => _opponentEvent;


        /// テスト的UI
        [LabelText("プレイヤーマークマン")]
        [SerializeField] private TextMeshProUGUI _playerMarkman;
        public TextMeshProUGUI PlayerMarkman => _playerMarkman;
        [LabelText("相手マークマン")]
        [SerializeField] private TextMeshProUGUI _opponentMarkman;
        public TextMeshProUGUI OpponentMarkman => _opponentMarkman;


    }
}
