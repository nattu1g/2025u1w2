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
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Vcontainer.Presenter
{

    public class MainPresenter : IInitializable, IStartable
    {
        // RootLifetimeScopeで登録されたコンポーネント
        readonly ComponentAssembly _componentAssembly;
        readonly VolumeEntity _volumeEntity;
        readonly AudioEntity _audioEntity;
        readonly AudioConductorSettings _audioConductorSettings;
        readonly CueSheetAsset _cueSheetAsset;

        // MainLifetimeScopeで登録されたコンポーネント
        readonly UICanvas _uiCanvas;

        private SaveValue<VolumeSave, bool> _saveValue = new SaveValue<VolumeSave, bool>(new VolumeSave(5, 5), false);

        public MainPresenter(
            ComponentAssembly componentAssembly,
            VolumeEntity volumeEntity,
            AudioEntity audioEntity,
            AudioConductorSettings audioConductorSettings,
            CueSheetAsset cueSheetAsset,
            UICanvas uiCanvas
            )
        {
            _componentAssembly = componentAssembly;
            _volumeEntity = volumeEntity;
            _audioEntity = audioEntity;
            _audioConductorSettings = audioConductorSettings;
            _cueSheetAsset = cueSheetAsset;
            _uiCanvas = uiCanvas;
        }

        public void Initialize()
        {
            // 設定セーブデータのパス
            var savePath = Application.persistentDataPath + "/" + ".savedatasv.json";
            Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);

            // 設定セーブデータの読み込み
            if (System.IO.File.Exists(savePath))
            {
                // ファイルが存在する場合は読み込む
                SaveDataManager.LoadJsonData(_saveValue, savePath);
            }
            else
            {
                // セーブファイルが存在しない場合は、デフォルト値を使用
                Debug.Log("セーブデータが見つかりません。デフォルト値を使用します。");
                // デフォルト値を設定
                // _saveValue = new SaveValue<VolumeSave, bool>(new VolumeSave(5, 5), false);
                // 必要に応じて初期セーブデータを作成
                SaveDataManager.SaveJsonData(_saveValue, savePath);
            }


            // AudioConductorの初期化
            AudioConductorInterface.Setup(_audioConductorSettings);
            // AudioEntityの初期化
            _audioEntity.SetCueSheet(_cueSheetAsset);
            // BGMの再生
            _audioEntity._controllers[(int)Setting.AudioType.BGM].Play("bgm1");


            // オプションの登録
            _uiCanvas.OptionView.OptionShowButton.onClickCallback = () => _uiCanvas.Show(_uiCanvas.OptionView);
            _uiCanvas.OptionView.OptionHideButton.onClickCallback = () =>
            {
                _uiCanvas.Hide(_uiCanvas.OptionView);
                // 画面を閉じたらセーブデータの保存
                _saveValue.Val1._bgmVolume = _volumeEntity.BgmVolumeValue;
                _saveValue.Val1._seVolume = _volumeEntity.SeVolumeValue;
                SaveDataManager.SaveJsonData(_saveValue, savePath);
            };
            // 音量調整
            _uiCanvas.OptionView.BgmMinusButton.onClickCallback = async () =>
            {
                await _volumeEntity.SetBGMVolume(_volumeEntity.BgmVolumeValue - _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
                _uiCanvas.OptionView.SetBgmText(_volumeEntity.BgmVolumeValue);
            };
            _uiCanvas.OptionView.BgmPlusButton.onClickCallback = async () =>
            {
                await _volumeEntity.SetBGMVolume(_volumeEntity.BgmVolumeValue + _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
                _uiCanvas.OptionView.SetBgmText(_volumeEntity.BgmVolumeValue);
            };
            _uiCanvas.OptionView.SeMinusButton.onClickCallback = async () =>
            {
                await _volumeEntity.SetSEVolume(_volumeEntity.SeVolumeValue - _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
                _uiCanvas.OptionView.SetSeText(_volumeEntity.SeVolumeValue);
                _audioEntity._controllers[(int)Setting.AudioType.SE].Play("se1");

            };
            _uiCanvas.OptionView.SePlusButton.onClickCallback = async () =>
            {
                await _volumeEntity.SetSEVolume(_volumeEntity.SeVolumeValue + _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
                _uiCanvas.OptionView.SetSeText(_volumeEntity.SeVolumeValue);
                _audioEntity._controllers[(int)Setting.AudioType.SE].Play("se1");
            };
        }

        public void Start()
        {

            // ボリュームの設定, UIの更新
            _ = _volumeEntity.SetBGMVolume(_saveValue.Val1._bgmVolume, _componentAssembly.AudioMixer);
            _ = _volumeEntity.SetSEVolume(_saveValue.Val1._seVolume, _componentAssembly.AudioMixer);
            _uiCanvas.OptionView.SetBgmText(_volumeEntity.BgmVolumeValue);
            _uiCanvas.OptionView.SetSeText(_volumeEntity.SeVolumeValue);
        }
    }
}
