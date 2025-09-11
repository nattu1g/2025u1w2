using Alchemy.Inspector;
using BBSim.UIs.Core;
using BBSim.Vcontainer.Entity;
using BBSim.Vcontainer.Handler;
using BBSim.Vcontainer.Presenter;
using BBSim.Vcontainer.UseCase;
using Common.Features.Save;
using Common.Vcontainer.Entity;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts.Vcontainer.Installer
{
    public class MainLifeTimeScope : LifetimeScope
    {
        [Title("LIFETIME")]
        [LabelText("UICanvas")]
        [SerializeField] private UICanvas _uiCanvas;
        // [LabelText("PlayerInputObject")]
        // [SerializeField] private InputBall _inputBall;


        protected override void Configure(IContainerBuilder builder)
        {
            // PubSub
            builder.RegisterMessagePipe();

            // Handler (汎用的なものや、未整理のもの)
            builder.Register<ButtonHandler>(Lifetime.Singleton);
            // builder.Register<GameInitializationHandler>(Lifetime.Singleton);
            builder.Register<PlayerClubHandler>(Lifetime.Singleton); // TODO: 将来的にPresenterに移行する候補
            builder.Register<TrainingSelectHandler>(Lifetime.Singleton); // TODO: 将来的にPresenterに移行する候補

            // UseCase
            builder.Register<AudioUseCase>(Lifetime.Singleton);
            builder.Register<BbsimLoadUseCase>(Lifetime.Singleton);
            builder.Register<BbsimSaveUseCase>(Lifetime.Singleton);
            builder.Register<MessageUseCase>(Lifetime.Singleton);
            builder.Register<TrainingUseCase>(Lifetime.Singleton);
            builder.Register<CalendarUseCase>(Lifetime.Singleton);
            builder.Register<BattleUseCase>(Lifetime.Singleton);

            // Entity
            builder.Register<AudioEntity>(Lifetime.Singleton);
            builder.Register<VolumeEntity>(Lifetime.Singleton);
            builder.Register<StudentEntity>(Lifetime.Singleton);
            builder.Register<PlayerClubEntity>(Lifetime.Singleton);
            builder.Register<TrainingOptionEntity>(Lifetime.Singleton);
            builder.Register<CalendarEntity>(Lifetime.Singleton);
            builder.Register<DrawCardEntity>(Lifetime.Singleton);

            // Presenter (通常のクラスとして登録)
            builder.Register<SaveLoadPresenter>(Lifetime.Singleton);
            builder.Register<MainPresenter>(Lifetime.Singleton);
            builder.Register<OptionPresenter>(Lifetime.Singleton);

            // ヒエラルキー上のコンポーネント
            builder.RegisterComponentInHierarchy<SaveManager>();

            // Serializeしたコンポーネント
            builder.RegisterComponent(_uiCanvas);

            // 初期化の起点となる唯一のEntryPoint
            builder.RegisterEntryPoint<InitializationOrchestrator>();
        }
    }
}
