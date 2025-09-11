using Alchemy.Inspector;
using Common.UIs.Core;
using UnityEngine;

namespace BBSim.UIs.Views
{
    public class PlayerStatusView : BaseUIView
    {
        [Title("プレイヤー側ステータス表示")]
        [LabelText("ステータスカードコンテナ")]
        [SerializeField] private GameObject _statusCardContainer;
        public GameObject StatusCardContainer => _statusCardContainer;
    }
}
