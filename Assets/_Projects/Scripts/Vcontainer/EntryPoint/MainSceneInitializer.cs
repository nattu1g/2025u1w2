using App.Vcontainer.UseCase;
using Common.Vcontainer.EntryPoint;

namespace App.Vcontainer.EntryPoint
{
    /// <summary>
    /// Mainシーンの起動起点。MainGameInitializeUseCaseを呼び出す。
    /// </summary>
    public class MainSceneInitializer : AbstractApplicationInitializer<MainGameInitializeUseCase>
    {
        public MainSceneInitializer(MainGameInitializeUseCase initializeUseCase) : base(initializeUseCase) { }
    }
}