using UnityEngine;

namespace Common.Features.Save
{
    /// <summary>
    /// 個々の生徒の所持データをシリアライズするためのクラス
    /// 任意のアイテムのIDとデータをペアで保持する汎用的なエントリークラス
    /// </summary>
    [System.Serializable]
    public class GenericOwnedItemEntry<TItemData>
    {
        public string ItemId;
        public TItemData Data; // ここがジェネリックなデータ型になります

        // JsonUtilityがデシリアライズ時に使用するパラメータなしコンストラクタ
        public GenericOwnedItemEntry() { }

        public GenericOwnedItemEntry(string itemId, TItemData data)
        {
            this.ItemId = itemId;
            this.Data = data;
        }
    }
}
