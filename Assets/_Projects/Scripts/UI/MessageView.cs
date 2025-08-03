using Alchemy.Inspector;
using Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MessageView : BaseUIView
    {
        [Title("メッセージ項目")]
        [LabelText("上部メッセージウィンドウ")]
        [SerializeField] private GameObject _messageWindowUp;
        public GameObject MessageWindowUp => _messageWindowUp;
        [LabelText("上部メッセージテキスト")]
        [SerializeField] private TextMeshProUGUI _messageTextUp;
        public TextMeshProUGUI MessageTextUp => _messageTextUp;
        [LabelText("上部サムネイル画像")]
        [SerializeField] private Image _thumbnailUp;
        public Image ThumbnailUp => _thumbnailUp;
        [LabelText("下部メッセージウィンドウ")]
        [SerializeField] private GameObject _messageWindowDown;
        public GameObject MessageWindowDown => _messageWindowDown;
        [LabelText("下部メッセージテキスト")]
        [SerializeField] private TextMeshProUGUI _messageTextDown;
        public TextMeshProUGUI MessageTextDown => _messageTextDown;
        [LabelText("下部サムネイル画像")]
        [SerializeField] private Image _thumbnailDown;
        public Image ThumbnailDown => _thumbnailDown;
    }
}
