using Alchemy.Inspector;
using App.SOs;
using UnityEngine;

namespace App.Features.Assembly
{
    /// <summary>
    /// 試合の可視化に必要なコンポーネントをアセンブルするクラス
    /// ヒエラルキーに設置して、LifeTimeScapeに読み込ませる
    /// </summary>
    public class GlobalAssetAssembly : MonoBehaviour
    {
        [Header("Coin")]
        public GameObject CoinPrefab;

        [Header("Coin Definitions")]
        public CoinDefinition NormalCoinDefinition;
        public CoinDefinition DenseCoinDefinition;
        public CoinDefinition CoolingCoinDefinition;

        [Header("Water Prefabs")]
        public GameObject CircleWaterPrefab;   // 丸い水
        public GameObject EllipseWaterPrefab;  // 楕円の水
        public GameObject SquareWaterPrefab;   // 四角い水
    }
}