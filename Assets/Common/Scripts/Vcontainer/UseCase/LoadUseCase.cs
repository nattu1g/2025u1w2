// using Common.Features;
// using Common.Features.Save;
// using Common.Settings;
// using Common.Vcontainer.Entity;
// using Cysharp.Threading.Tasks;
// using System;
// using UnityEngine;

// namespace Scripts.Vcontainer.UseCase
// {
//     public class LoadUseCase : IDisposable
//     {
//         readonly VolumeEntity _volumeEntity;
//         // readonly StudentEntity _studentEntity;
//         // readonly TeacherEntity _teacherEntity; // TeacherEntityをインジェクト
//         // readonly EventItemEntity _eventItemEntity;
//         // readonly ClubItemEntity _clubItemEntity;
//         readonly ComponentAssembly _componentAssembly;
//         readonly SaveManager _saveManager;

//         // 各ロード処理に対応するUniTaskCompletionSource
//         private UniTaskCompletionSource<string> _appSettingsLoadTcs;
//         private UniTaskCompletionSource<string> _playerDataLoadTcs;
//         public LoadUseCase(
//             VolumeEntity volumeEntity,
//             // StudentEntity studentEntity,
//             // TeacherEntity teacherEntity, // TeacherEntityをインジェクト
//             // EventItemEntity eventItemEntity,
//             // ClubItemEntity clubItemEntity,
//             ComponentAssembly componentAssembly,
//             SaveManager saveManager
//             )
//         {
//             _volumeEntity = volumeEntity;
//             // _studentEntity = studentEntity;
//             // _teacherEntity = teacherEntity; // TeacherEntityを初期化
//             // _eventItemEntity = eventItemEntity;
//             // _clubItemEntity = clubItemEntity;
//             _componentAssembly = componentAssembly;
//             _saveManager = saveManager;

//             _saveManager.OnDataReady += HandleDataReady;
//         }

//         // IDisposableを実装して、イベント購読を解除できるようにする
//         public void Dispose()
//         {
//             _saveManager.OnDataReady -= HandleDataReady;
//             // TCSが残っていればキャンセルまたはエラーで完了させる
//             _appSettingsLoadTcs?.TrySetCanceled();
//             _playerDataLoadTcs?.TrySetCanceled();
//         }

//         private void HandleDataReady(string key, string jsonData)
//         {
//             // Debug.Log($"[LoadUseCase] handledatareadyを受け取りました. Key: {key}, Data: {(string.IsNullOrEmpty(jsonData) ? "null" : "data present")}");
//             // if (key == GameConstants.AppSettingsSaveKey && _appSettingsLoadTcs != null)
//             // {
//             //     _appSettingsLoadTcs.TrySetResult(jsonData);
//             // }
//             // else if (key == GameConstants.PlayerDataSaveKey && _playerDataLoadTcs != null)
//             // {
//             //     _playerDataLoadTcs.TrySetResult(jsonData);
//             // }
//             // else
//             // {
//             //     Debug.LogWarning($"[LoadUseCase] 未処理のキーまたはnull TCSのデータを受信しました. Key: {key}");
//             // }
//         }
//         public async UniTask LoadAppSettingsData()
//         {
//             _appSettingsLoadTcs = new UniTaskCompletionSource<string>();

//             AppSettingsData loadedSettings;
// #if UNITY_WEBGL && !UNITY_EDITOR
//             // Debug.Log("[LoadUseCase][LoadAppSettingsData] WEBGL ロードしようとしています.");
//             _saveManager.LoadData(GameConstants.AppSettingsSaveKey); // 汎用ロードを呼び出す
//             string jsonData = await _appSettingsLoadTcs.Task;
//             _appSettingsLoadTcs = null; // 使用後にクリア
//             loadedSettings = JsonUtility.FromJson<AppSettingsData>(jsonData);
// #else
//             // Debug.Log("[LoadUseCase][LoadAppSettingsData] NOT WEBGL ロードしようとしています.");
//             // _appSettingsSaveValue を new で初期化します。
//             SaveValue<AppSettingsData, bool?> _appSettingsSaveValue = new SaveValue<AppSettingsData, bool?>(new AppSettingsData(), null);
//             _saveManager.LoadData(GameConstants.AppSettingsSavePath, _appSettingsSaveValue); // 汎用ロードを呼び出す
//             loadedSettings = _appSettingsSaveValue.Val1;
// #endif
//             // loadedSettings と、その中の GameSettings が null でないことを確認
//             if (loadedSettings != null && loadedSettings.GameSettings != null)
//             {
//                 _volumeEntity.SetBGMVolume(loadedSettings.GameSettings._bgmVolume, _componentAssembly.AudioMixer).Forget();
//                 _volumeEntity.SetSEVolume(loadedSettings.GameSettings._seVolume, _componentAssembly.AudioMixer).Forget();
//                 // TODO: loadedSettings.IsTutorialCompleted を適切なEntityや状態に反映する処理
//                 // Debug.Log("[LoadUseCase] APpsettingsDataはロードおよび適用されました.");
//             }
//             else
//             {
//                 Debug.LogWarning($"[LoadUseCase] AppSettingsDataのロードに失敗したか、GameSettingsがnullです。デフォルト値で初期化し保存します. loadedSettings is null: {loadedSettings == null}");
//                 await CreateAndSaveDefaultAppSettings();
//             }
//         }
//         public async UniTask LoadPlayerData()
//         {
//             _playerDataLoadTcs = new UniTaskCompletionSource<string>();

//             PlayerAndTeacherSaveData loadedCombinedData = null; // 新しい結合データ型を使用
// #if UNITY_WEBGL && !UNITY_EDITOR
//             // Debug.Log("[LoadUseCase][LoadPlayerData] WEBGL ロードしようとしています.");
//             _saveManager.LoadData(GameConstants.PlayerDataSaveKey); // 汎用ロードを呼び出す
//             string jsonData = await _playerDataLoadTcs.Task;
//             _playerDataLoadTcs = null; // 使用後にクリア
//             // loadedPlayerData = JsonUtility.FromJson<GenericOwnedCollectionSaveData<CardData>>(jsonData);
//             if (!string.IsNullOrEmpty(jsonData))
//             {
//                  loadedCombinedData = JsonUtility.FromJson<PlayerAndTeacherSaveData>(jsonData); // 結合データ型でデシリアライズ
//             }
// #else
//             // Debug.Log("[LoadUseCase][LoadPlayerData] NOT WEBGL ロードしようとしています.");
//             SaveValue<PlayerAndTeacherSaveData, bool?> _playerSaveData // 結合データ型を使用
//             = new SaveValue<PlayerAndTeacherSaveData, bool?>(new PlayerAndTeacherSaveData(), null);

//             _saveManager.LoadData(GameConstants.PlayerDataSavePath, _playerSaveData); // 汎用ロードを呼び出す
//             loadedCombinedData = _playerSaveData.Val1;
// #endif
//             if (loadedCombinedData != null)
//             {
//                 // ロードしたデータを各Entityに設定
//                 // _studentEntity.SetOwnedStudents(loadedCombinedData.Students?.ToDictionary() ?? new Dictionary<string, CardData>());
//                 // TODO: TeacherEntityにSetOwnedTeachersメソッドを追加し、ロードした先生データを設定する
//                 // _teacherEntity.SetOwnedTeachers(loadedCombinedData.Teachers?.ToDictionary() ?? new Dictionary<string, CardData>());
//                 // _eventItemEntity.SetOwnedEventItems(loadedCombinedData.EventItems?.ToDictionary() ?? new Dictionary<string, CardData>());
//                 // _clubItemEntity.SetOwnedClubItems(loadedCombinedData.ClubItems?.ToDictionary() ?? new Dictionary<string, CardData>());
//                 // Debug.Log("[LoadUseCase] PlayerDataはロードされ、適用されました.");
//             }
//             else
//             {
//                 Debug.LogWarning($"[LoadUseCase] JSONのPlayerDataの脱気体化に失敗しました。デフォルトと保存を使用します.");
//                 await CreateAndSaveDefaultPlayerAndTeacherData(_componentAssembly);
//             }
//         }

//         /// <summary>
//         /// 初めてオプションデータをロードした時に、セーブデータを作成する
//         /// </summary>
//         /// <returns></returns>
//         private async UniTask CreateAndSaveDefaultAppSettings()
//         {
//             var defaultSettings = new AppSettingsData();
//             _volumeEntity.SetBGMVolume(defaultSettings.GameSettings._bgmVolume, _componentAssembly.AudioMixer).Forget();
//             _volumeEntity.SetSEVolume(defaultSettings.GameSettings._seVolume, _componentAssembly.AudioMixer).Forget();

//             string jsonToSave = JsonUtility.ToJson(defaultSettings);

// #if UNITY_WEBGL && !UNITY_EDITOR
//             // Debug.Log("[LoadUseCase][CreateAndSaveDefaultAppSettings] WEBGL 初期データセーブ.");
//             _saveManager.SaveData(GameConstants.AppSettingsSaveKey, jsonToSave);
// #else
//             // Debug.Log("[LoadUseCase][CreateAndSaveDefaultAppSettings] NOT WEBGL 初期データセーブ.");
//             _saveManager.SaveData(GameConstants.AppSettingsSavePath, new SaveValue<AppSettingsData, bool?>(defaultSettings, false));
// #endif
//             await UniTask.CompletedTask;
//         }

//         /// <summary>
//         /// 初めてプレイヤーデータをロードした時に、セーブデータを作成する
//         /// </summary>
//         /// /// <param name="componentAssembly">ComponentAssembly (デフォルトデータ生成に必要)</param>
//         /// <returns></returns>
//         private async UniTask CreateAndSaveDefaultPlayerAndTeacherData(ComponentAssembly componentAssembly) // private に戻す
//         {
//             var combinedSaveData = new PlayerAndTeacherSaveData();
//             Debug.Log("[LoadUseCase] Creating and saving default Player and Teacher data.");

//             // 生徒のデフォルトデータを作成
//             // var defaultStudentOwnedData = new Dictionary<string, CardData>();
//             // if (componentAssembly?.StudentList?.studentCardList != null)
//             // {
//             //     // foreach (var studentCard in _componentAssembly.StudentList.studentCardList)
//             //     foreach (var studentCard in componentAssembly.StudentList.studentCardList)
//             //     {
//             //         if (studentCard != null && !string.IsNullOrEmpty(studentCard.id))
//             //         {
//             //             defaultStudentOwnedData[studentCard.id] = new CardData(0, 0); // デフォルト: 所持数0, 進化回数0
//             //         }
//             //     }
//             // }
//             // combinedSaveData.Students.FromDictionary(defaultStudentOwnedData);
//             // _studentEntity.SetOwnedStudents(defaultStudentOwnedData);

//             // 先生のデフォルトデータを作成
//             // var defaultTeacherOwnedData = new Dictionary<string, CardData>();
//             // if (componentAssembly?.TeacherList?.teacherCardList != null)
//             // {
//             //     foreach (var teacherCard in componentAssembly.TeacherList.teacherCardList)
//             //     {
//             //         if (teacherCard != null && !string.IsNullOrEmpty(teacherCard.id))
//             //         {
//             //             defaultTeacherOwnedData[teacherCard.id] = new CardData(0, 0); // デフォルト: 所持数0, 進化回数0
//             //         }
//             //     }
//             // }
//             // combinedSaveData.Teachers.FromDictionary(defaultTeacherOwnedData);
//             // _teacherEntity.SetOwnedTeachers(defaultTeacherOwnedData);

//             // 行事のデフォルトデータを作成
//             // var defaultEventItemOwnedData = new Dictionary<string, CardData>();
//             // if (componentAssembly?.EventItemList?.eventItemCardList != null)
//             // {
//             //     foreach (var eventItemCard in componentAssembly.EventItemList.eventItemCardList)
//             //     {
//             //         if (eventItemCard != null && !string.IsNullOrEmpty(eventItemCard.id))
//             //         {
//             //             defaultEventItemOwnedData[eventItemCard.id] = new CardData(0, 0); // デフォルト: 所持数0, 進化回数0
//             //         }
//             //     }
//             // }
//             // combinedSaveData.EventItems.FromDictionary(defaultEventItemOwnedData);
//             // _eventItemEntity.SetOwnedEventItems(defaultEventItemOwnedData);

//             // 部活のデフォルトデータを作成
//             // var defaultClubItemOwnedData = new Dictionary<string, CardData>();
//             // if (componentAssembly?.ClubItemList?.clubItemCardList != null)
//             // {
//             //     foreach (var clubItemCard in componentAssembly.ClubItemList.clubItemCardList)
//             //     {
//             //         if (clubItemCard != null && !string.IsNullOrEmpty(clubItemCard.id))
//             //         {
//             //             defaultClubItemOwnedData[clubItemCard.id] = new CardData(0, 0); // デフォルト: 所持数0, 進化回数0
//             //         }
//             //     }
//             // }
//             // combinedSaveData.ClubItems.FromDictionary(defaultClubItemOwnedData);
//             // _clubItemEntity.SetOwnedClubItems(defaultClubItemOwnedData);

//             string jsonToSave = JsonUtility.ToJson(combinedSaveData); // 結合データをJSON化

// #if UNITY_WEBGL && !UNITY_EDITOR
//             // Debug.Log("[LoadUseCase][CreateAndSaveDefaultPlayerAndTeacherData] WEBGL 初期データセーブ.");
//             _saveManager.SaveData(GameConstants.PlayerDataSaveKey, jsonToSave);
// #else
//             // Debug.Log("[LoadUseCase][CreateAndSaveDefaultPlayerAndTeacherData] NOT WEBGL 初期データセーブ.");
//             _saveManager.SaveData(GameConstants.PlayerDataSavePath, new SaveValue<PlayerAndTeacherSaveData, bool?>(combinedSaveData, false));
// #endif
//             await UniTask.CompletedTask;
//         }

//         public async UniTask LoadAllData()
//         {
//             await LoadAppSettingsData();
//             await LoadPlayerData();
//         }
//     }
// }
