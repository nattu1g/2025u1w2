using Alchemy.Inspector;
using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.UI.Views
{
    public class PlayerStatusView : BaseUIView
    {
        [Title("プレイヤー側ステータス表示")]
        [LabelText("ステータスカードコンテナ")]
        [SerializeField] private GameObject _statusCardContainer;
        public GameObject StatusCardContainer => _statusCardContainer;
    }
}
