using UnityEngine;
using Alchemy.Inspector;

namespace App.SOs
{
    /// <summary>
    /// コインのデータを定義するScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "CoinDefinition", menuName = "WaterTank/CoinDefinition")]
    public class CoinDefinition : ScriptableObject
    {
        [Title("基本情報")]
        [LabelText("コイン名")]
        [SerializeField] private string _coinName;
        
        [LabelText("説明")]
        [TextArea(2, 4)]
        [SerializeField] private string _description;
        
        [Title("ゲームパラメータ")]
        [LabelText("価格")]
        [SerializeField] private int _price;
        
        [LabelText("膨張率")]
        [Tooltip("水の膨張率（正の値で膨張、負の値で収縮）")]
        [SerializeField] private float _expansionRate;
        
        [Title("ビジュアル")]
        [LabelText("コインプレハブ")]
        [SerializeField] private GameObject _coinPrefab;
        
        [LabelText("アイコン")]
        [SerializeField] private Sprite _icon;
        
        // プロパティ
        public string CoinName => _coinName;
        public string Description => _description;
        public int Price => _price;
        public float ExpansionRate => _expansionRate;
        public GameObject CoinPrefab => _coinPrefab;
        public Sprite Icon => _icon;
    }
}
