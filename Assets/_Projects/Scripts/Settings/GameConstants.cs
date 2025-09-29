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
    }
}
