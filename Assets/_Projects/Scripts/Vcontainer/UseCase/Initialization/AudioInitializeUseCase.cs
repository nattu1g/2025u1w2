using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Common.Features;
using Common.Vcontainer.Entity;
using Common.Vcontainer.UseCase.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Vcontainer.UseCase
{
    /// <summary>
    /// AudioConductorの初期化を担当するUseCase。
    /// AudioConductorInterface.Setupを実行し、オーディオシステムを準備する。
    /// </summary>
    public class AudioInitializeUseCase : IInitializableUseCase
    {
        private readonly AudioConductorSettings _audioConductorSettings;
        private readonly CueSheetAsset _cueSheetAsset;
        private readonly AudioEntity _audioEntity;
        private readonly VolumeEntity _volumeEntity;
        private readonly ComponentAssembly _componentAssembly;

        // 初期化順序を最優先に設定（AudioEntityや他のUseCaseより前に実行される必要がある）
        public int Order => -100;

        public AudioInitializeUseCase(
            AudioConductorSettings audioConductorSettings,
            CueSheetAsset cueSheetAsset,
            AudioEntity audioEntity,
            VolumeEntity volumeEntity,
            ComponentAssembly componentAssembly)
        {
            _audioConductorSettings = audioConductorSettings;
            _cueSheetAsset = cueSheetAsset;
            _audioEntity = audioEntity;
            _volumeEntity = volumeEntity;
            _componentAssembly = componentAssembly;
        }

        public async UniTask InitializeAsync()
        {
            // AudioConductorの初期化
            if (_audioConductorSettings != null)
            {
                AudioConductorInterface.Setup(_audioConductorSettings);
            }
            else
            {
                Debug.LogError("[AudioInitializeUseCase] AudioConductorSettings is null!");
            }

            // CueSheetAssetの確認
            if (_cueSheetAsset == null)
            {
                Debug.LogError("[AudioInitializeUseCase] CueSheetAsset is null!");
            }

            // AudioEntityのコントローラーを初期化（AudioConductorInterface.Setup()の後に実行）
            _audioEntity.Initialize();

            await _audioEntity.PlayBGM("bgm1");

            await UniTask.CompletedTask;
        }
    }
}
