using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Setting
{
    public static class GameConstants
    {
        public const bool IsAddressable = false;
        public const float DebuffPercentage = 0.75f;

        public const int InitialHandCardCount = 5;

        public static readonly Vector2 IRIS_SCALE_IN = Vector2.one;
        public static readonly Vector2 IRIS_SCALE_OUT = new Vector2(50, 50);
        public const float IRIS_TIME = 1f;
        public static readonly string AppSettingsSavePath = Application.persistentDataPath + "/" + "app_settings.json";
        public static readonly string PlayerDataSavePath = Application.persistentDataPath + "/" + "player_data.json";
    }
}