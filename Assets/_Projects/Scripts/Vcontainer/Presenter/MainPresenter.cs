using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Cysharp.Threading.Tasks;
using GekinatuPackage.SaveJson.Data;
using GekinatuPackage.SaveJson.Json;
using Scripts.Component;
using Scripts.Mono;
using Scripts.Setting;
using Scripts.UI;
using Scripts.Vcontainer.Entity;
using Scripts.Vcontainer.Handler;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Vcontainer.Presenter
{

    public class MainPresenter : IInitializable, ITickable
    {
        // RootLifetimeScopeで登録されたコンポーネント
        readonly ComponentAssembly _componentAssembly;
        readonly VolumeEntity _volumeEntity;
        readonly AudioEntity _audioEntity;
        readonly AudioConductorSettings _audioConductorSettings;
        readonly CueSheetAsset _cueSheetAsset;

        // MainLifetimeScopeで登録されたコンポーネント
        readonly UICanvas _uiCanvas;
        readonly GameInitializationHandler _gameInitHandler;
        readonly AudioHandler _audioHandler;
        readonly SaveLoadHandler _saveLoadHandler;

        public MainPresenter(
            ComponentAssembly componentAssembly,
            VolumeEntity volumeEntity,
            AudioEntity audioEntity,
            AudioConductorSettings audioConductorSettings,
            CueSheetAsset cueSheetAsset,
            UICanvas uiCanvas,
            GameInitializationHandler gameInitHandler,
            AudioHandler audioHandler,
            SaveLoadHandler saveLoadHandler
            )
        {
            _componentAssembly = componentAssembly;
            _volumeEntity = volumeEntity;
            _audioEntity = audioEntity;
            _audioConductorSettings = audioConductorSettings;
            _cueSheetAsset = cueSheetAsset;
            _uiCanvas = uiCanvas;
            _gameInitHandler = gameInitHandler;
            _audioHandler = audioHandler;
            _saveLoadHandler = saveLoadHandler;
        }

        public async void Initialize()
        {
            Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);

            await _saveLoadHandler.LoadAllFile();

            await _audioHandler.Initialize();
            _gameInitHandler.Initialize();

            // 一覧リストの登録
            // _listParentHandler.Initialize();
            // _studentListHandler.Initialize();
            // _studentFilterHandler.Initialize();
            // _studentDetailHandler.Initialize();

            await _audioEntity.PlayBGM("MainBGM");
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {

            }
        }
    }
}
