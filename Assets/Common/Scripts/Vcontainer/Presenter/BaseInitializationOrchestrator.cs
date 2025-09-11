using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Common.VContainer.Presenter
{
    public abstract class BaseInitializationOrchestrator : IInitializable
    {
        // このメソッドが初期化処理のテンプレート（骨格）となる
        public async void Initialize()
        {
            await OnPreInitializeAsync();
            await OnInitializeAsync();
            await OnPostInitializeAsync();
        }

        // 各プロジェクトは、これらのメソッドを実装（オーバーライド）して
        // 具体的な処理内容を記述する

        protected virtual UniTask OnPreInitializeAsync() => UniTask.CompletedTask;
        protected abstract UniTask OnInitializeAsync();
        protected virtual UniTask OnPostInitializeAsync() => UniTask.CompletedTask;
    }
}

