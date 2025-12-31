using UnityEngine;

namespace App.Settings
{
    public static class GameConstants
    {
        public const bool IsAddressable = false;
        public static readonly string AppSettingsSavePath = Application.persistentDataPath + "/" + "app_settings.json";
        public static readonly string PlayerDataSavePath = Application.persistentDataPath + "/" + "player_data.json";
        public static readonly string AppSettingsSaveKey = "app_settings";
        public static readonly string PlayerDataSaveKey = "player_data";

        // ===== ポイントシステム =====

        /// <summary>
        /// 水の膨張1回あたりの基本ポイント
        /// </summary>
        public const int BasePointsPerExpansion = 10;

        /// <summary>
        /// フォールド回数に応じたポイント倍率
        /// 例: 0回目 = 1.5倍, 1回目 = 2.0倍, 2回目 = 2.5倍, 3回目 = 3.0倍
        /// </summary>
        public static readonly float[] FoldMultipliers = { 1.5f, 2.0f, 2.5f, 3.0f };

        /// <summary>
        /// フォールド回数に応じた基準線の高さ（Y座標）
        /// 例: 0回目 = 2.0, 1回目 = 2.5, 2回目 = 3.0, 3回目 = 3.5
        /// </summary>
        public static readonly float[] BaselineHeights = { 2.0f, 2.5f, 3.0f, 3.5f };

        /// <summary>
        /// 最大フォールド回数（これ以上は倍率が上がらない）
        /// </summary>
        public const int MaxFoldCount = 3;

        // ===== Water生成設定 =====

        /// <summary>
        /// CircleWater（丸い水）の最小生成数
        /// </summary>
        public const int MinCircleWaterCount = 70;

        /// <summary>
        /// CircleWater（丸い水）の最大生成数
        /// </summary>
        public const int MaxCircleWaterCount = 90;

        /// <summary>
        /// EllipseWater（楕円の水）の最小生成数
        /// </summary>
        public const int MinEllipseWaterCount = 0;

        /// <summary>
        /// EllipseWater（楕円の水）の最大生成数
        /// </summary>
        public const int MaxEllipseWaterCount = 2;

        /// <summary>
        /// SquareWater（四角い水）の最小生成数
        /// </summary>
        public const int MinSquareWaterCount = 0;

        /// <summary>
        /// SquareWater（四角い水）の最大生成数
        /// </summary>
        public const int MaxSquareWaterCount = 2;

        /// <summary>
        /// Water生成時の最小間隔（CircleWater用）
        /// </summary>
        public const float CircleWaterMinDistance = 0.8f;

        /// <summary>
        /// Water生成時の最小間隔（EllipseWater/SquareWater用）
        /// </summary>
        public const float LargeWaterMinDistance = 1.5f;

        /// <summary>
        /// Water生成範囲の最小X座標
        /// </summary>
        public const float WaterSpawnMinX = -2.5f;

        /// <summary>
        /// Water生成範囲の最大X座標
        /// </summary>
        public const float WaterSpawnMaxX = 2.5f;

        /// <summary>
        /// フォールド回数に応じたポイント倍率を取得
        /// </summary>
        /// <param name="foldCount">現在のフォールド回数</param>
        /// <returns>ポイント倍率</returns>
        public static float GetFoldMultiplier(int foldCount)
        {
            if (foldCount < 0) return 1.0f;
            if (foldCount >= FoldMultipliers.Length) return FoldMultipliers[FoldMultipliers.Length - 1];
            return FoldMultipliers[foldCount];
        }

        /// <summary>
        /// フォールド回数に応じた基準線の高さを取得
        /// </summary>
        /// <param name="foldCount">現在のフォールド回数</param>
        /// <returns>基準線の高さ（Y座標）</returns>
        public static float GetBaselineHeight(int foldCount)
        {
            if (foldCount < 0) return BaselineHeights[0];
            if (foldCount >= BaselineHeights.Length) return BaselineHeights[BaselineHeights.Length - 1];
            return BaselineHeights[foldCount];
        }
    }
}
