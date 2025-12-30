using Alchemy.Inspector;
using App.Features;
using App.Features.WaterTank.Baseline;
using App.Features.WaterTank.Coin;
using App.Features.WaterTank.Water;
using App.UIs.Core;
using App.Vcontainer.Entity;
using App.Vcontainer.EntryPoint;
using App.Vcontainer.Presenter;
using App.Vcontainer.UseCase;
using App.Vcontainer.UseCase.RunTime;
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
        // uGUI版（段階的移行中）
        // [LabelText("UICanvas")]
        // [SerializeField] private UICanvas _uiCanvas;

        // [LabelText("MatchAssembly")]
        // [SerializeField] private MatchAssembly _matchAssembly;
        // [LabelText("PlayerInputObject")]
        // [SerializeField] private InputBall _inputBall;


        protected override void Configure(IContainerBuilder builder)
        {
            // PubSub
            builder.RegisterMessagePipe();

            // Handler (汎用的なものや、未整理のもの)
            // uGUI版（段階的移行中）
            // builder.Register<ButtonHandler>(Lifetime.Singleton);

            // UI Toolkit版
            builder.Register<UIToolkitButtonHandler>(Lifetime.Singleton);

            // Entity
            // builder.Register<StudentEntity>(Lifetime.Singleton);

            // UseCase - 基本システム
            builder.Register<AudioUseCase>(Lifetime.Singleton);
            builder.Register<MainGameInitializeUseCase>(Lifetime.Singleton);

            builder.Register<AudioInitializeUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();

            // App固有のUseCaseを「IInitializableUseCase」として登録する
            builder.Register<AppLoadUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();
            builder.Register<AppSaveUseCase>(Lifetime.Singleton);

            // Entity - ゲーム状態管理
            builder.Register<GameStateEntity>(Lifetime.Singleton);

            // UseCase - ゲームロジック
            builder.Register<CoinDropUseCase>(Lifetime.Singleton);
            builder.Register<FoldUseCase>(Lifetime.Singleton);

            // Presenter
            builder.Register<OptionPresenter>(Lifetime.Singleton)
                .As<IStartable>();
            builder.Register<GamePresenterUIToolkit>(Lifetime.Singleton)
                .AsImplementedInterfaces(); // IStartable と ITickable の両方を登録

            // ヒエラルキー上のコンポーネント
            builder.RegisterComponentInHierarchy<SaveManager>();
            builder.RegisterComponentInHierarchy<CoinSpawner>();
            builder.RegisterComponentInHierarchy<GlobalAssetAssembly>();
            builder.RegisterComponentInHierarchy<BaselineDisplay>();
            builder.RegisterComponentInHierarchy<WaterLevelChecker>();

            // UI Toolkit版のCanvasを登録
            builder.RegisterComponentInHierarchy<UIToolkitCanvas>();

            // 初期化の起点となる唯一のEntryPoint
            builder.RegisterEntryPoint<MainSceneInitializer>();
            builder.RegisterEntryPoint<WaterLevelCheckerInitializer>();
        }
    }
}
