using System.Collections.Generic;

namespace Common.Features.Save
{
    /// <summary>
    /// 任意のアイテムのコレクションをリストとして保持し、Dictionaryとの変換を行う汎用的なクラス
    /// </summary>
    [System.Serializable]
    public class GenericOwnedCollectionSaveData<TItemData>
    {
        public List<GenericOwnedItemEntry<TItemData>> Entries = new List<GenericOwnedItemEntry<TItemData>>();

        public void FromDictionary(Dictionary<string, TItemData> dictionary)
        {
            Entries.Clear();
            if (dictionary == null) return;
            foreach (var kvp in dictionary)
            {
                Entries.Add(new GenericOwnedItemEntry<TItemData>(kvp.Key, kvp.Value));
            }
        }

        public Dictionary<string, TItemData> ToDictionary()
        {
            var dict = new Dictionary<string, TItemData>();
            if (Entries == null) return dict;
            foreach (var entry in Entries)
            {
                // ItemIdがnullや空でなく、Dataがnullでないことを確認 (TItemDataが参照型の場合)
                if (!string.IsNullOrEmpty(entry.ItemId)) // TItemDataが値型なら entry.Data != null は不要
                {
                    dict[entry.ItemId] = entry.Data;
                }
            }
            return dict;
        }
    }
}
