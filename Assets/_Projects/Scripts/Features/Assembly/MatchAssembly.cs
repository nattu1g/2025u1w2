using Alchemy.Inspector;
using UnityEngine;

namespace App.Features
{
    /// <summary>
    /// 試合の可視化に必要なコンポーネントをアセンブルするクラス
    /// ヒエラルキーに設置して、LifeTimeScapeに読み込ませる
    /// </summary>
    public class MatchAssembly : MonoBehaviour
    {
        [Header("Match Visualization")]
        [LabelText("プレイヤーPrefab")]
        [SerializeField] private GameObject _playerPrefab;
        public GameObject PlayerPrefab => _playerPrefab;

        [LabelText("OpponentPrefab")]
        [SerializeField] private GameObject _opponentPrefab;
        public GameObject OpponentPrefab => _opponentPrefab;

        [LabelText("プレイヤーの親Transform")]
        [SerializeField] private Transform _playersParent;
        public Transform PlayersParent => _playersParent;

        [LabelText("ボールPrefab")]
        [SerializeField] private GameObject _ballPrefab;
        public GameObject BallPrefab => _ballPrefab;
    }
}