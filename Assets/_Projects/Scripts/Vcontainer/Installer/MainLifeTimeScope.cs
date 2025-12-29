using Alchemy.Inspector;
using App.UIs.Core;
using App.Vcontainer.EntryPoint;
using App.Vcontainer.Presenter;
using App.Vcontainer.UseCase;
using Common.Features.Save;
using Common.Vcontainer.EntryPoint;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using Common.Vcontainer.UseCase.Base;
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
        // [LabelText("MatchAssembly")]
        // [SerializeField] private MatchAssembly _matchAssembly;
        // [LabelText("PlayerInputObject")]
        // [SerializeField] private InputBall _inputBall;


        protected override void Configure(IContainerBuilder builder)
        {
            // PubSub
            builder.RegisterMessagePipe();

            // Handler (汎用的なものや、未整理のもの)
            builder.Register<ButtonHandler>(Lifetime.Singleton);

            // Entity
            // builder.Register<StudentEntity>(Lifetime.Singleton);

            // UseCase
            builder.Register<AudioUseCase>(Lifetime.Singleton);
            builder.Register<MainGameInitializeUseCase>(Lifetime.Singleton);

            builder.Register<AudioInitializeUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();

            // App固有のUseCaseを「IInitializableUseCase」として登録する
            builder.Register<AppLoadUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();
            builder.Register<AppSaveUseCase>(Lifetime.Singleton);
            // Presenter (通常のクラスとして登録)
            builder.Register<OptionPresenter>(Lifetime.Singleton)
                 .As<IStartable, OptionPresenter>();

            // Commonから汎用クラスを登録
            // builder.Register<GameInitializeUseCase>(Lifetime.Singleton);

            // ヒエラルキー上のコンポーネント
            builder.RegisterComponentInHierarchy<SaveManager>();

            // Serializeしたコンポーネント
            builder.RegisterComponent(_uiCanvas);
            // builder.RegisterComponent(_matchAssembly);

            // 初期化の起点となる唯一のEntryPoint
            builder.RegisterEntryPoint<MainSceneInitializer>();
        }
    }
}
