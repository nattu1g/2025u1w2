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

            builder.Register<AudioInitializeUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();

            // App固有のUseCaseを「IInitializableUseCase」として登録する
            builder.Register<AppLoadUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();
            builder.Register<AppSaveUseCase>(Lifetime.Singleton);

            // TODO: 水槽ゲーム用のUseCase/Presenterをここに追加
            // 例: builder.Register<WaterTankGameUseCase>(Lifetime.Singleton);
            // 例: builder.Register<WaterTankPresenter>(Lifetime.Singleton).As<IStartable>();

            // ヒエラルキー上のコンポーネント
            builder.RegisterComponentInHierarchy<SaveManager>();

            // UI Toolkit版のCanvasを登録
            builder.RegisterComponentInHierarchy<UIToolkitCanvas>();

            // 初期化の起点となる唯一のEntryPoint
            builder.RegisterEntryPoint<MainSceneInitializer>();
        }
    }
}
