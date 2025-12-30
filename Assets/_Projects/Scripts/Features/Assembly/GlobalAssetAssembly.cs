using Alchemy.Inspector;
using App.SOs;
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

        [Header("Coin Definitions")]
        [LabelText("通常コイン")]
        [SerializeField] private CoinDefinition _normalCoinDef;

        [LabelText("高密度コイン")]
        [SerializeField] private CoinDefinition _denseCoinDef;

        [LabelText("冷却コイン")]
        [SerializeField] private CoinDefinition _coolingCoinDef;

        public GameObject CoinPrefab => _coinPrefab;
        public CoinDefinition NormalCoinDef => _normalCoinDef;
        public CoinDefinition DenseCoinDef => _denseCoinDef;
        public CoinDefinition CoolingCoinDef => _coolingCoinDef;
    }
}