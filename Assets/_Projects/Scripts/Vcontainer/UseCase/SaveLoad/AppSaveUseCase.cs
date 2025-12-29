
using App.Features.Save;
using App.Settings;
using Common.Features.Save;
using Common.Vcontainer.Entity;
using Common.Vcontainer.UseCase.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Vcontainer.UseCase
{
    public class AppSaveUseCase : BaseSaveUseCase
    {
        private readonly VolumeEntity _volumeEntity;
        // private readonly StudentEntity _studentEntity;
        // private readonly TeacherEntity _teacherEntity;
        // private readonly EventItemEntity _eventItemEntity;
        // private readonly ClubItemEntity _clubItemEntity;

        public AppSaveUseCase(
            VolumeEntity volumeEntity,
            SaveManager saveManager
            ) : base(saveManager)
        {
            _volumeEntity = volumeEntity;
        }

        public override async UniTask SaveAllDataAsync()
        {
            await SaveAppSettingsData();
            await SavePlayerData();
        }

        private async UniTask SaveAppSettingsData()
        {
            var appSettings = new AppSettingsData();
            appSettings.GameSettings._bgmVolume = _volumeEntity.BgmVolumeValue;
            appSettings.GameSettings._seVolume = _volumeEntity.SeVolumeValue;

            string jsonToSave = JsonUtility.ToJson(appSettings);

#if UNITY_WEBGL && !UNITY_EDITOR
            SaveManager.SaveData(GameConstants.AppSettingsSaveKey, jsonToSave);
#else
            // SaveValueを使わずに直接FileControllで保存
            FileControll.WriteFile(GameConstants.AppSettingsSavePath, jsonToSave);
#endif
            await UniTask.CompletedTask;
        }

        private async UniTask SavePlayerData()
        {
            var combinedSaveData = new PlayerAndTeacherSaveData();

            // combinedSaveData.Students.FromDictionary(_studentEntity.OwnedStudents);
            // combinedSaveData.Teachers.FromDictionary(_teacherEntity.OwnedTeachers);
            // combinedSaveData.EventItems.FromDictionary(_eventItemEntity.OwnedEventItems);
            // combinedSaveData.ClubItems.FromDictionary(_clubItemEntity.OwnedClubItems);

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
