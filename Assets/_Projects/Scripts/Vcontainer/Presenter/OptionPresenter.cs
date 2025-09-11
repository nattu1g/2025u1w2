using System;
using BBSim.UIs.Core;
using BBSim.Vcontainer.UseCase;
using Common.Vcontainer.Entity;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace BBSim.Vcontainer.Presenter
{
    public class OptionPresenter : IInitializable, IDisposable
    {
        private readonly UICanvas _uiCanvas; // OptionViewを持つUICanvas
        private readonly AudioUseCase _audioUseCase;
        private readonly BbsimSaveUseCase _bbsimSaveUseCase;
        private readonly ButtonHandler _buttonHandler;
        private readonly AudioEntity _audioEntity;
        private readonly CompositeDisposable _disposables = new();

        public OptionPresenter(
            UICanvas uiCanvas,
            AudioUseCase audioUseCase,
            BbsimSaveUseCase saveUseCase,
            ButtonHandler buttonHandler,
            AudioEntity audioEntity)
        {
            _uiCanvas = uiCanvas;
            _audioUseCase = audioUseCase;
            _bbsimSaveUseCase = saveUseCase;
            _buttonHandler = buttonHandler;
            _audioEntity = audioEntity;
        }

        public void Initialize()
        {
            var optionView = _uiCanvas.OptionView;

            // --- UseCaseのデータ変更を購読し、UIを更新する ---
            _audioUseCase.BgmVolume
                .Subscribe(volume => optionView.SetBgmText((int)volume))
                .AddTo(_disposables);

            _audioUseCase.SeVolume
                .Subscribe(volume => optionView.SetSeText((int)volume))
                .AddTo(_disposables);

            // --- UIイベントを購読し、UseCaseを呼び出す ---
            // UseCaseのメソッドがUniTaskを返すようになったため、ラムダ内でawaitするか、メソッドグループを直接渡す
            _buttonHandler.SetupActionButton(optionView.BgmPlusButton, _audioUseCase.BgmUp);
            _buttonHandler.SetupActionButton(optionView.BgmMinusButton, _audioUseCase.BgmDown);

            _buttonHandler.SetupActionButton(optionView.SePlusButton, async () =>
            {
                await _audioUseCase.SeUp();
                await _audioEntity.PlaySE("se1");
            });
            _buttonHandler.SetupActionButton(optionView.SeMinusButton, async () =>
            {
                await _audioUseCase.SeDown();
                await _audioEntity.PlaySE("se1");
            });

            // オプション開閉ボタン
            _buttonHandler.SetupActionButton(optionView.ShowButton, async () =>
            {
                _uiCanvas.Show(optionView);
                await _audioEntity.PlaySE("se1");
                // return UniTask.CompletedTask;
            });

            _buttonHandler.SetupActionButton(optionView.HideButton, async () =>
            {
                _uiCanvas.Hide(optionView);
                await _audioEntity.PlaySE("se1");
                await _bbsimSaveUseCase.SaveAllDataAsync();
            });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
