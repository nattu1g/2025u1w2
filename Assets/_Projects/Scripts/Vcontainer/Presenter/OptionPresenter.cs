using System;
using App.UIs.Core;
using App.Vcontainer.UseCase;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace App.Vcontainer.Presenter
{
    // IInitializableを削除し、IDisposableのみを実装する
    public class OptionPresenter : IDisposable, IStartable
    {
        private readonly UICanvas _uiCanvas;
        private readonly AudioUseCase _audioUseCase;
        private readonly AppSaveUseCase _AppSaveUseCase;
        private readonly ButtonHandler _buttonHandler;
        private readonly CompositeDisposable _disposables = new();

        // コンストラクタで全ての初期化処理を行う
        public OptionPresenter(
            UICanvas uiCanvas,
            AudioUseCase audioUseCase,
            AppSaveUseCase saveUseCase,
            ButtonHandler buttonHandler)
        {
            _uiCanvas = uiCanvas;
            _audioUseCase = audioUseCase;
            _AppSaveUseCase = saveUseCase;
            _buttonHandler = buttonHandler;

            var optionView = _uiCanvas.OptionView;

            // --- UseCaseのデータ変更を購読し、UIを更新する ---
            _audioUseCase.BgmVolume
                .Subscribe(volume => optionView.SetBgmText((int)volume))
                .AddTo(_disposables);

            _audioUseCase.SeVolume
                .Subscribe(volume => optionView.SetSeText((int)volume))
                .AddTo(_disposables);

            // --- UIイベントを購読し、UseCaseを呼び出す ---
            _buttonHandler.SetupActionButton(optionView.BgmPlusButton, _audioUseCase.BgmUp);
            _buttonHandler.SetupActionButton(optionView.BgmMinusButton, _audioUseCase.BgmDown);

            // SE再生はUseCaseの責務になったため、Presenterはメソッドを呼び出すだけ
            _buttonHandler.SetupActionButton(optionView.SePlusButton, _audioUseCase.SeUp);
            _buttonHandler.SetupActionButton(optionView.SeMinusButton, _audioUseCase.SeDown);

            // オプション開閉ボタン
            _buttonHandler.SetupActionButton(optionView.ShowButton, async () =>
            {
                _uiCanvas.Show(optionView);
                // UI効果音の再生もUseCaseに依頼する
                await _audioUseCase.PlayUISound("se1");
            });

            _buttonHandler.SetupActionButton(optionView.HideButton, async () =>
            {
                _uiCanvas.Hide(optionView);
                await _audioUseCase.PlayUISound("se1");
                await _AppSaveUseCase.SaveAllDataAsync();
            });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Start()
        {
        }
    }
}
