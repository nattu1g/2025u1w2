using Alchemy.Inspector;
using Scripts.UI.Component;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Views
{
    public class BattleView : BaseUIView
    {
        [Title("バトル画面")]
        [LabelText("プレイヤー側点数表示テキスト")]
        [SerializeField] private TextMeshProUGUI _playerScore;
        public TextMeshProUGUI PlayerScore => _playerScore;
        [LabelText("相手側点数表示テキスト")]
        [SerializeField] private TextMeshProUGUI _opponentScore;
        public TextMeshProUGUI OpponentScore => _opponentScore;
        [LabelText("試合数表示テキスト")]
        [SerializeField] private TextMeshProUGUI _battleCount;
        public TextMeshProUGUI BattleCount => _battleCount;
        [LabelText("ドローボタン")]
        [SerializeField] private CustomButton _drawButton;
        public CustomButton DrawButton => _drawButton;
        // [LabelText("ドローしたカード")]
        [LabelText("プレイヤー側スターティングメンバーコンテナ")]
        [SerializeField] private GameObject _playerMemberContainer;
        public GameObject PlayerMemberContainer => _playerMemberContainer;
        [LabelText("相手側スターティングメンバーコンテナ")]
        [SerializeField] private GameObject _opponentMemberContainer;
        public GameObject OpponentMemberContainer => _opponentMemberContainer;
        [LabelText("ドローカードオブジェクト")]
        [SerializeField] private GameObject _drawCard;
        public GameObject DrawCard => _drawCard;
        [LabelText("ドローカード名称テキスト")]
        [SerializeField] private TextMeshProUGUI _name;
        public TextMeshProUGUI Name => _name;
        [LabelText("ドローカード筋力補正テキスト")]
        [SerializeField] private TextMeshProUGUI _correctionPowrText;
        public TextMeshProUGUI CorrectionPowrText => _correctionPowrText;
        [LabelText("ドローカード運命力補正テキスト")]
        [SerializeField] private TextMeshProUGUI _correctionFateText;
        public TextMeshProUGUI CorrectionFateText => _correctionFateText;
        [LabelText("ドローカード持久力補正テキスト")]
        [SerializeField] private TextMeshProUGUI _correctionStaminaText;
        public TextMeshProUGUI CorrectionStaminaText => _correctionStaminaText;
    }
}
