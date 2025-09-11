// using Cysharp.Threading.Tasks;
// using GekinatuPackage.SaveJson.Data;
// using Scripts.Features.Save; // AppSettingsData, GenericOwnedCollectionSaveData, CardData のため
// using Scripts.Setting;
// using Scripts.Setting;
// using Scripts.Vcontainer.Entity; // VolumeEntity, StudentEntityのため
// using UnityEngine;

// namespace Scripts.Vcontainer.UseCase
// {
//     public class SaveUseCase
//     {
//         private readonly VolumeEntity _volumeEntity;
//         // private readonly StudentEntity _studentEntity;
//         // private readonly TeacherEntity _teacherEntity; // TeacherEntityをインジェクト
//         // private readonly EventItemEntity _eventItemEntity;
//         // private readonly ClubItemEntity _clubItemEntity;
//         private readonly SaveManager _saveManager;

//         public SaveUseCase(
//             VolumeEntity volumeEntity,
//             // StudentEntity studentEntity,
//             // TeacherEntity teacherEntity, // TeacherEntityをインジェクト
//             // EventItemEntity eventItemEntity,
//             // ClubItemEntity clubItemEntity,
//             SaveManager saveManager
//             )
//         {
//             _volumeEntity = volumeEntity;
//             // _studentEntity = studentEntity;
//             // _teacherEntity = teacherEntity; // TeacherEntityを初期化
//             // _eventItemEntity = eventItemEntity;
//             // _clubItemEntity = clubItemEntity;
//             _saveManager = saveManager;
//         }

//         public async UniTask SaveAppSettingsData()
//         {
//             var appSettings = new AppSettingsData(); // 保存するデータオブジェクトを作成
//             appSettings.GameSettings._bgmVolume = _volumeEntity.BgmVolumeValue;
//             appSettings.GameSettings._seVolume = _volumeEntity.SeVolumeValue;

//             string jsonToSave = JsonUtility.ToJson(appSettings);

// #if UNITY_WEBGL && !UNITY_EDITOR
//             // Debug.Log("[SaveUseCase][SaveAppSettingsData] WEBGL 保存しようとしています.");
//             _saveManager.SaveData(GameConstants.AppSettingsSaveKey, jsonToSave);
// #else
//             // Debug.Log("[SaveUseCase][SaveAppSettingsData] NOT WEBGL 保存しようとしています.");
//             _saveManager.SaveData(GameConstants.AppSettingsSavePath, new SaveValue<AppSettingsData, bool?>(appSettings, false));
// #endif
//             // Debug.Log($"[SaveUseCase] appsettingsdata indexeddbへの保存. Key: {GameConstants.AppSettingsSaveKey}, Data: {jsonToSave}");
//             await UniTask.CompletedTask;
//         }

//         public async UniTask SavePlayerData()
//         {
//             // 生徒データと先生データをまとめるコンテナを作成
//             var combinedSaveData = new PlayerAndTeacherSaveData();

//             // 各Entityからデータを取得し、コンテナに設定
//             // combinedSaveData.Students.FromDictionary(_studentEntity.OwnedStudents);
//             // combinedSaveData.Teachers.FromDictionary(_teacherEntity.OwnedTeachers);
//             // combinedSaveData.EventItems.FromDictionary(_eventItemEntity.OwnedEventItems);
//             // combinedSaveData.ClubItems.FromDictionary(_clubItemEntity.OwnedClubItems);

//             string jsonToSave = JsonUtility.ToJson(combinedSaveData);
//             // Debug.Log($"[SaveUseCase] playerdata indexeddbへの保存. Key: {GameConstants.PlayerDataSaveKey}, Data: {jsonToSave}");


// #if UNITY_WEBGL && !UNITY_EDITOR
//             // Debug.Log("[SaveUseCase][SavePlayerData] WEBGL 保存しようとしています.");
//             _saveManager.SaveData(GameConstants.PlayerDataSaveKey, jsonToSave);
// #else
//             // Debug.Log("[SaveUseCase][SavePlayerData] NOT WEBGL 保存しようとしています.");
//             _saveManager.SaveData(GameConstants.PlayerDataSavePath, new SaveValue<PlayerAndTeacherSaveData, bool?>(combinedSaveData, false));
// #endif
//             await UniTask.CompletedTask;
//         }

//         public async UniTask SaveAllData()
//         {
//             await SaveAppSettingsData();
//             await SavePlayerData();
//         }
//     }
// }
