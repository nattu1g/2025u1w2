using System.Collections.Generic;
using Common.Events;
using Common.Vcontainer.UseCase.Base;
using MessagePipe;

namespace App.Vcontainer.UseCase
{
    /// <summary>
    /// Mainシーンの初期化処理を統括するUseCase。
    /// </summary>
    public class MainGameInitializeUseCase : AbstractGameInitializeUseCase, IGameInitializeUseCase
    {
        public MainGameInitializeUseCase(IEnumerable<IInitializableUseCase> useCases, IPublisher<GameInitializedEvent> publisher)
            : base(useCases, publisher) { }
    }
}