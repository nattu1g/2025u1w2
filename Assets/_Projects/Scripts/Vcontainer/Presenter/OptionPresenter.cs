using System;
using App.UIs.Core;
using App.UIs.Views;
using App.Vcontainer.UseCase;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace App.Vcontainer.Presenter
{
    /// <summary>
    /// オプション画面のPresenter（UI Toolkit版）
    /// </summary>
    public class OptionPresenter : IDisposable, IStartable
    {
        private readonly UIToolkitCanvas _uiToolkitCanvas;
        private readonly AudioUseCase _audioUseCase;
        private readonly AppSaveUseCase _appSaveUseCase;
        private readonly UIToolkitButtonHandler _buttonHandler;
        private readonly CompositeDisposable _disposables = new();

        private OptionViewUIToolkit _optionView;

        // コンストラクタで依存性を注入
        public OptionPresenter(
            UIToolkitCanvas uiToolkitCanvas,
            AudioUseCase audioUseCase,
            AppSaveUseCase saveUseCase,
            UIToolkitButtonHandler buttonHandler)
        {
            _uiToolkitCanvas = uiToolkitCanvas;
            _audioUseCase = audioUseCase;
            _appSaveUseCase = saveUseCase;
            _buttonHandler = buttonHandler;
        }

        public void Start()
        {
            // アーキテクチャガイドラインに従い、Start()でViewを取得
            _optionView = _uiToolkitCanvas.OptionView;

            if (_optionView == null)
            {
                Debug.LogError("OptionView is null in OptionPresenter.Start()");
                return;
            }

            // --- UseCaseのデータ変更を購読し、UIを更新する ---
            _audioUseCase.BgmVolume
                .Subscribe(volume => _optionView.SetBgmText((int)volume))
                .AddTo(_disposables);

            _audioUseCase.SeVolume
                .Subscribe(volume => _optionView.SetSeText((int)volume))
                .AddTo(_disposables);

            // --- UIイベントを購読し、UseCaseを呼び出す ---
            _buttonHandler.SetupActionButton(_optionView.BgmPlusButton, async () =>
            {
                _audioUseCase.BgmUp();
                await UniTask.Yield();
            });

            _buttonHandler.SetupActionButton(_optionView.BgmMinusButton, async () =>
            {
                _audioUseCase.BgmDown();
                await UniTask.Yield();
            });

            _buttonHandler.SetupActionButton(_optionView.SePlusButton, async () =>
            {
                _audioUseCase.SeUp();
                await UniTask.Yield();
            });

            _buttonHandler.SetupActionButton(_optionView.SeMinusButton, async () =>
            {
                _audioUseCase.SeDown();
                await UniTask.Yield();
            });

            // オプション開閉ボタン
            _buttonHandler.SetupActionButton(_optionView.ShowButton, async () =>
            {
                _uiToolkitCanvas.Show(_optionView);
                await _audioUseCase.PlayUISound("se1");
            });

            _buttonHandler.SetupActionButton(_optionView.HideButton, async () =>
            {
                _uiToolkitCanvas.Hide(_optionView);
                await _audioUseCase.PlayUISound("se1");
                await _appSaveUseCase.SaveAllDataAsync();
            });
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _buttonHandler?.Dispose();
        }
    }
}
