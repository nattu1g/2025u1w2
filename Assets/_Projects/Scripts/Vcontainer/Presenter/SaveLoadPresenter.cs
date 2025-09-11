using BBSim.Vcontainer.UseCase;
using Cysharp.Threading.Tasks;

namespace BBSim.Vcontainer.Presenter
{
    public class SaveLoadPresenter
    {
        private readonly BbsimLoadUseCase _bbsimLoadUseCase;


        public SaveLoadPresenter(BbsimLoadUseCase bbsimLoadUseCase)
        {
            _bbsimLoadUseCase = bbsimLoadUseCase;
        }

        public async UniTask InitializeAsync()
        {
            // アプリケーション起動時のデータロード処理
            await _bbsimLoadUseCase.LoadAllDataAsync();
        }
    }
}
