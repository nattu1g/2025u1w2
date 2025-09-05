using Alchemy.Inspector;
using MessagePipe;
using Scripts.Features.Save;
using Scripts.Setting;
using Scripts.UI;
using Scripts.UI.Core;
using Scripts.Vcontainer.Entity;
using Scripts.Vcontainer.Handler;
using Scripts.Vcontainer.Presenter;
using Scripts.Vcontainer.UseCase;
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
            // Handler
            builder.Register<SaveLoadHandler>(Lifetime.Singleton);
            builder.Register<AudioHandler>(Lifetime.Singleton);
            builder.Register<ButtonHandler>(Lifetime.Singleton);
            builder.Register<GameInitializationHandler>(Lifetime.Singleton);
            builder.Register<PlayerClubHandler>(Lifetime.Singleton);
            builder.Register<TrainingSelectHandler>(Lifetime.Singleton);
            // UseCase
            builder.Register<AudioUseCase>(Lifetime.Singleton);
            builder.Register<LoadUseCase>(Lifetime.Singleton);
            builder.Register<SaveUseCase>(Lifetime.Singleton);
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
            // ヒエラルキー上のコンポーネント
            builder.RegisterComponentInHierarchy<SaveManager>();
            // Serializeしたコンポーネント
            builder.RegisterComponent(_uiCanvas);
            // MonoBehaviorでInjectしているスクリプトは直接指定する
            // builder.RegisterComponent(_inputBall);
            // 起点のとなるエントリーポイント
            builder.RegisterEntryPoint<MainPresenter>();

        }
    }
}
