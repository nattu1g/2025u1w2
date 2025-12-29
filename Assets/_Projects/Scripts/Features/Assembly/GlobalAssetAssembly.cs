using Alchemy.Inspector;
using UnityEngine;

namespace App.Features
{
    /// <summary>
    /// 試合の可視化に必要なコンポーネントをアセンブルするクラス
    /// ヒエラルキーに設置して、LifeTimeScapeに読み込ませる
    /// </summary>
    public class GlobalAssetAssembly : MonoBehaviour
    {
        [Header("Water Tank Game")]
        [LabelText("コインPrefab")]
        [SerializeField] private GameObject _coinPrefab;
        public GameObject CoinPrefab => _coinPrefab;
    }
}