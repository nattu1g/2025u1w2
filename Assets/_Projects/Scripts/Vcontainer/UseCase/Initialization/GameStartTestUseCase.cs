using App.Events;
using Common.Vcontainer.UseCase.Base;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;

namespace App.Vcontainer.UseCase
{
    public class GameStartTestUseCase : IInitializableUseCase
    {
        private readonly IPublisher<WeekAdvancedEvent> _weekAdvancedPublisher;
        public int Order => 200;
        public GameStartTestUseCase(IPublisher<WeekAdvancedEvent> weekAdvancedPublisher)
        {
            _weekAdvancedPublisher = weekAdvancedPublisher;
        }

        public async UniTask InitializeAsync()
        {
            Debug.Log("[GameStartTestUseCase][InitializeAsync] Start");
            await UniTask.CompletedTask;
        }
    }
}
