using BBSim.Vcontainer.Handler;
using Common.VContainer.Presenter;
using Cysharp.Threading.Tasks;

namespace BBSim.Vcontainer.Presenter
{
    /// <summary>
    /// 各PresenterやHandlerの初期化順序を制御する専門のEntryPoint
    /// </summary>
    public class InitializationOrchestrator : BaseInitializationOrchestrator
    {
        private readonly SaveLoadPresenter _saveLoadPresenter;
        private readonly MainPresenter _mainPresenter;
        private readonly OptionPresenter _optionPresenter;
        // private readonly GameInitializationHandler _gameInitializationHandler;
        private readonly PlayerClubHandler _playerClubHandler;
        private readonly TrainingSelectHandler _trainingSelectHandler;

        public InitializationOrchestrator(
            SaveLoadPresenter saveLoadPresenter,
            MainPresenter mainPresenter,
            OptionPresenter optionPresenter,
            // GameInitializationHandler gameInitializationHandler,
            PlayerClubHandler playerClubHandler,
            TrainingSelectHandler trainingSelectHandler)
        {
            _saveLoadPresenter = saveLoadPresenter;
            _mainPresenter = mainPresenter;
            _optionPresenter = optionPresenter;
            // _gameInitializationHandler = gameInitializationHandler;
            _playerClubHandler = playerClubHandler;
            _trainingSelectHandler = trainingSelectHandler;
        }

        // 事前初期化（セーブロードなど）の処理をここに記述
        protected override async UniTask OnPreInitializeAsync()
        {
            await _saveLoadPresenter.InitializeAsync();
        }
        protected override async UniTask OnInitializeAsync()
        {
            // 2. 各機能の初期化を行う
            // Note: MainPresenter自身の初期化はここに含まれる
            _mainPresenter.Initialize();
            _optionPresenter.Initialize();
            // _gameInitializationHandler.Initialize();
            _playerClubHandler.Initialize();
            _trainingSelectHandler.Initialize();

            await UniTask.CompletedTask;
        }
        protected override async UniTask OnPostInitializeAsync()
        {
            await UniTask.CompletedTask;
        }
    }
}
