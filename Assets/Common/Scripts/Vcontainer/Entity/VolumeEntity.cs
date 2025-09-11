using Cysharp.Threading.Tasks;
using System;
using R3;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.Vcontainer.Entity
{
    public class VolumeEntity : IDisposable
    {
        private ReactiveProperty<float> _bgmVolume = new(0);
        public Observable<float> BgmVolume => _bgmVolume;
        public float BgmVolumeValue => _bgmVolume.Value;

        private ReactiveProperty<float> _seVolume = new(0);
        public Observable<float> SeVolume => _seVolume;
        public float SeVolumeValue => _seVolume.Value;

        public readonly float VolumeIncrement = 1;
        private const int VolumeLevel = 10;



        public async UniTask SetBGMVolume(float volume, AudioMixer audioMixer)
        {
            var setVolume = volume;
            if (setVolume < 0) setVolume = 0;
            if (setVolume > VolumeLevel) setVolume = VolumeLevel;
            _bgmVolume.Value = setVolume;

            SetAudioMixerVolume(setVolume, "BGM", audioMixer);

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        public async UniTask SetSEVolume(float volume, AudioMixer audioMixer)
        {
            var setVolume = volume;
            if (setVolume < 0) setVolume = 0;
            if (setVolume > VolumeLevel) setVolume = VolumeLevel;
            _seVolume.Value = setVolume;

            SetAudioMixerVolume(setVolume, "SE", audioMixer);

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        }

        private void SetAudioMixerVolume(float volume, string mixerName, AudioMixer audioMixer)
        {
            // ボリュームレベルで割る
            volume /= VolumeLevel;
            // -80~0に変換
            var vol = Mathf.Clamp(Mathf.Log10(volume) * 20f, -80f, 20f);
            // -4より小さい場合は-4にする
            if (vol < -4) vol = Mathf.Ceil(vol);
            //audioMixerに設定
            audioMixer.SetFloat(mixerName, vol);
        }

        public void Dispose()
        {
        }
    }
}

