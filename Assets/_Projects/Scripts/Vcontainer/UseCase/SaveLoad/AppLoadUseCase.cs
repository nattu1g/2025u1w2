
using System.Threading.Tasks;
using App.Features.Save;
using App.Settings;
using Common.Features;
using Common.Features.Save;
using Common.Vcontainer.Entity;
using Common.Vcontainer.UseCase.Base;
using Common.Vcontainer.UseCase.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
// using Scripts.Features.Save; // AppSettingsData, PlayerAndTeacherSaveData, GenericOwnedCollectionSaveData, CardData のため
// using Scripts.Setting; // GameConstants のため
// using Scripts.Vcontainer.Entity; // VolumeEntity, StudentEntity, TeacherEntity, EventItemEntity, ClubItemEntity のため

namespace App.Vcontainer.UseCase
{
    public class AppLoadUseCase : BaseLoadUseCase, IInitializableUseCase
    {
        public int Order => 0;

        readonly VolumeEntity _volumeEntity;
        readonly ComponentAssembly _componentAssembly;

        public AppLoadUseCase(
            VolumeEntity volumeEntity,
            ComponentAssembly componentAssembly,
            SaveManager saveManager
            ) : base(saveManager)
        {
            _volumeEntity = volumeEntity;
            _componentAssembly = componentAssembly;
        }
        public async UniTask InitializeAsync()
        {
            await LoadAllDataAsync();
        }
        public override async UniTask LoadAllDataAsync()
        {
            await LoadAppSettingsData();
            await LoadPlayerData();
        }

        private async UniTask LoadAppSettingsData()
        {
            AppSettingsData loadedSettings;
            string jsonData = await LoadDataAsync(GameConstants.AppSettingsSaveKey, GameConstants.AppSettingsSavePath);

            if (!string.IsNullOrEmpty(jsonData))
            {
                loadedSettings = JsonUtility.FromJson<AppSettingsData>(jsonData);
            }
            else
            {
                Debug.LogWarning("[AppLoadUseCase] No saved data found, using defaults");
                loadedSettings = new AppSettingsData(); // デフォルト値
            }

            if (loadedSettings != null && loadedSettings.GameSettings != null)
            {
                await _volumeEntity.SetBGMVolume(loadedSettings.GameSettings._bgmVolume, _componentAssembly.AudioMixer);
                await _volumeEntity.SetSEVolume(loadedSettings.GameSettings._seVolume, _componentAssembly.AudioMixer);
            }
            else
            {
                await CreateAndSaveDefaultAppSettings();
            }
        }

        private async UniTask LoadPlayerData()
        {
            PlayerAndTeacherSaveData loadedCombinedData = null;
            string jsonData = await LoadDataAsync(GameConstants.PlayerDataSaveKey, GameConstants.PlayerDataSavePath);

            if (!string.IsNullOrEmpty(jsonData))
            {
                loadedCombinedData = JsonUtility.FromJson<PlayerAndTeacherSaveData>(jsonData);
            }

            if (loadedCombinedData != null)
            {
                // _studentEntity.SetOwnedStudents(loadedCombinedData.Students?.ToDictionary() ?? new Dictionary<string, CardData>());
                // TODO: TeacherEntityにSetOwnedTeachersメソッドを追加し、ロードした先生データを設定する
                // _teacherEntity.SetOwnedTeachers(loadedCombinedData.Teachers?.ToDictionary() ?? new Dictionary<string, CardData>());
                // _eventItemEntity.SetOwnedEventItems(loadedCombinedData.EventItems?.ToDictionary() ?? new Dictionary<string, CardData>());
                // _clubItemEntity.SetOwnedClubItems(loadedCombinedData.ClubItems?.ToDictionary() ?? new Dictionary<string, CardData>());
            }
            else
            {
                await CreateAndSaveDefaultPlayerAndTeacherData(null); // componentAssembly は後で注入
            }
        }

        /// <summary>
        /// 初めてオプションデータをロードした時に、セーブデータを作成する
        /// </summary>
        /// <returns></returns>
        private async UniTask CreateAndSaveDefaultAppSettings()
        {
            var defaultSettings = new AppSettingsData();
            // _volumeEntity.SetBGMVolume(defaultSettings.GameSettings._bgmVolume, _componentAssembly.AudioMixer).Forget();
            // _volumeEntity.SetSEVolume(defaultSettings.GameSettings._seVolume, _componentAssembly.AudioMixer).Forget();

            string jsonToSave = JsonUtility.ToJson(defaultSettings);

#if UNITY_WEBGL && !UNITY_EDITOR
            SaveManager.SaveData(GameConstants.AppSettingsSaveKey, jsonToSave);
#else
            SaveManager.SaveData(GameConstants.AppSettingsSavePath, new SaveValue<AppSettingsData, bool?>(defaultSettings, false));
#endif
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 初めてプレイヤーデータをロードした時に、セーブデータを作成する
        /// </summary>
        /// /// <param name="componentAssembly">ComponentAssembly (デフォルトデータ生成に必要)</param>
        /// <returns></returns>
        private async UniTask CreateAndSaveDefaultPlayerAndTeacherData(ComponentAssembly componentAssembly) // componentAssembly は後で注入
        {
            var combinedSaveData = new PlayerAndTeacherSaveData();

            string jsonToSave = JsonUtility.ToJson(combinedSaveData);

#if UNITY_WEBGL && !UNITY_EDITOR
            SaveManager.SaveData(GameConstants.PlayerDataSaveKey, jsonToSave);
#else
            SaveManager.SaveData(GameConstants.PlayerDataSavePath, new SaveValue<PlayerAndTeacherSaveData, bool?>(combinedSaveData, false));
#endif
            await UniTask.CompletedTask;
        }


    }
}
