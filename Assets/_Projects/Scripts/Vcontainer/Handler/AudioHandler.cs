using System.Threading.Tasks;
using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Cysharp.Threading.Tasks;
using Scripts.Vcontainer.Entity;
using Scripts.Vcontainer.Handler;
using Scripts.Vcontainer.UseCase;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class AudioHandler : IHandler
    {
        readonly AudioEntity _audioEntity;
        readonly CueSheetAsset _cueSheetAsset;
        readonly AudioConductorSettings _audioConductorSettings;
        readonly ButtonHandler _buttonHandler;
        readonly AudioUseCase _audioUseCase;
        readonly SaveUseCase _saveUseCase;



        public AudioHandler(
            AudioEntity audioEntity,
            CueSheetAsset cueSheetAsset,
            AudioConductorSettings audioConductorSettings,
            ButtonHandler buttonHandler,
            AudioUseCase audioUseCase,
            SaveUseCase saveUseCase
            )
        {
            _audioEntity = audioEntity;
            _cueSheetAsset = cueSheetAsset;
            _audioConductorSettings = audioConductorSettings;
            _buttonHandler = buttonHandler;
            _audioUseCase = audioUseCase;
            _saveUseCase = saveUseCase;
        }

        public async Task Initialize()
        {
            // AudioConductorの初期化
            AudioConductorInterface.Setup(_audioConductorSettings);
            // AudioEntityの初期化
            _audioEntity.SetCueSheet(_cueSheetAsset);
            // BGMの再生
            _buttonHandler.SetBgmUpButton(_audioUseCase.BgmUp, null);
            _buttonHandler.SetBgmDownButton(_audioUseCase.BgmDown, null);
            _buttonHandler.SetSeUpButton(_audioUseCase.SeUp, () => _audioEntity.PlaySE("SmallClick"));
            _buttonHandler.SetSeDownButton(_audioUseCase.SeDown, () => _audioEntity.PlaySE("SmallClick"));

            // オプションの開閉ボタン。もしオプション項目がたくさんあるなら、OptionHandlerを作成する
            _buttonHandler.SetOptionOpenButton(() => _audioEntity.PlaySE("SmallClick"), null);
            _buttonHandler.SetOptionCloseButton(() => _audioEntity.PlaySE("SmallClick"), _saveUseCase.SaveAppSettingsData);

            // UI初期化
            await _audioUseCase.SetBgmText();
            await _audioUseCase.SetSeText();
        }

        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}
