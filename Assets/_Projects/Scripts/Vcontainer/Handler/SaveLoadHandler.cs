// using System.Threading.Tasks;
// using Cysharp.Threading.Tasks;
// using Scripts.Vcontainer.Handler;
// using Scripts.Vcontainer.UseCase;
// using UnityEngine;

// namespace Scripts.Vcontainer.Handler
// {
//     public class SaveLoadHandler : IHandler
//     {
//         readonly SaveUseCase _saveUseCase;
//         readonly LoadUseCase _loadUseCase;
//         // readonly LoadAddressableUseCase _loadAddressableUseCase;


//         public SaveLoadHandler(
//             SaveUseCase saveUseCase,
//             LoadUseCase loadUseCase
//              // LoadAddressableUseCase loadAddressableUseCase
//              )
//         {
//             _saveUseCase = saveUseCase;
//             _loadUseCase = loadUseCase;
//             // _loadAddressableUseCase = loadAddressableUseCase;
//         }
//         public async UniTask LoadAddressable()
//         {
//             // await _loadAddressableUseCase.LoadAddressable();
//         }
//         public async UniTask LoadAllFile()
//         {
//             await _loadUseCase.LoadAppSettingsData();
//             await _loadUseCase.LoadPlayerData();
//         }

//         public async UniTask SaveAllFile()
//         {
//             await _saveUseCase.SaveAppSettingsData();
//             await _saveUseCase.SavePlayerData();
//         }

//         public void Clear()
//         {
//         }

//         public void Dispose()
//         {
//         }
//     }
// }
