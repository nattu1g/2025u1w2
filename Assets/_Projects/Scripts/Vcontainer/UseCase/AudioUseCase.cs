using Cysharp.Threading.Tasks;
using Scripts.Mono;
using Scripts.UI;
using Scripts.Vcontainer.Entity;
using UnityEngine;

namespace Scripts.Vcontainer.UseCase
{
    public class AudioUseCase
    {
        readonly VolumeEntity _volumeEntity;
        readonly ComponentAssembly _componentAssembly;
        readonly UICanvas _uiCanvas;

        public AudioUseCase(
            VolumeEntity volumeEntity,
            ComponentAssembly componentAssembly,
            UICanvas uiCanvas
            )
        {
            _volumeEntity = volumeEntity;
            _componentAssembly = componentAssembly;
            _uiCanvas = uiCanvas;
        }

        public async UniTask BgmUp()
        {
            await _volumeEntity.SetBGMVolume(_volumeEntity.BgmVolumeValue + _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
            await SetBgmText();
        }

        public async UniTask BgmDown()
        {
            await _volumeEntity.SetBGMVolume(_volumeEntity.BgmVolumeValue - _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
            await SetBgmText();
        }

        public async UniTask SeUp()
        {
            await _volumeEntity.SetSEVolume(_volumeEntity.SeVolumeValue + _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
            await SetSeText();
        }

        public async UniTask SeDown()
        {
            await _volumeEntity.SetSEVolume(_volumeEntity.SeVolumeValue - _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
            await SetSeText();
        }
        public async UniTask SetBgmText()
        {
            _uiCanvas.OptionView.SetBgmText(_volumeEntity.BgmVolumeValue);
            await UniTask.CompletedTask;
        }
        public async UniTask SetSeText()
        {
            _uiCanvas.OptionView.SetSeText(_volumeEntity.SeVolumeValue);
            await UniTask.CompletedTask;
        }


    }
}
