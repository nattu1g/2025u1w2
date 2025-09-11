/// <summary>
/// Jsonでファイルにセーブ、ロードするクラス
/// </summary>

namespace Common.Features.Save
{
    public static class SaveDataManager
    {
        /// <summary>
        /// セーブ
        /// </summary>
        /// <param name="data">セーブデータ</param>
        /// <param name="saveFile">ファイルパス</param>
        public static void SaveJsonData(ISaveValue data, string saveFile)
        {
            if (data != null) FileControll.WriteFile(saveFile, data.ToJson());
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <param name="data">ロードデータ</param>
        /// <param name="saveFile">ファイルパス</param>
        public static void LoadJsonData(ISaveValue data, string saveFile)
        {
            string json = FileControll.ReadFile(saveFile);
            // ファイルが空でない場合はデータをロード
            if (!"".Equals(json)) data.FromJson(json);
        }
    }
}
