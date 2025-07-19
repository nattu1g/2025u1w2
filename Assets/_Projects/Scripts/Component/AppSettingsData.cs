namespace Scripts.Component
{
    [System.Serializable]
    public class AppSettingsData
    {
        public VolumeSave GameSettings;
        public bool IsTutorialCompleted;

        public AppSettingsData()
        {
            GameSettings = new VolumeSave(5, 5); // デフォルト音量
            IsTutorialCompleted = false; // デフォルトは未完了
        }
    }
}