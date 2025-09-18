using System.Collections.Generic;
using Alchemy.Inspector;
using BBSim.Models;
using BBSim.UIs.Core;
using BBSim.Vcontainer;
using BBSim.Vcontainer.Entity;
using BBSim.Vcontainer.Presenter;
using BBSim.Vcontainer.UseCase;
using Common.Features.Save;
using Common.Vcontainer.Entity;
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
        // [LabelText("PlayerInputObject")]
        // [SerializeField] private InputBall _inputBall;


        protected override void Configure(IContainerBuilder builder)
        {
            // PubSub
            builder.RegisterMessagePipe();

            // Handler (汎用的なものや、未整理のもの)
            builder.Register<ButtonHandler>(Lifetime.Singleton);

            // Entity
            builder.Register<StudentEntity>(Lifetime.Singleton);
            builder.Register<PlayerClubEntity>(Lifetime.Singleton);
            builder.Register<TrainingOptionEntity>(Lifetime.Singleton);
            builder.Register<CalendarEntity>(Lifetime.Singleton);
            builder.Register<DrawCardEntity>(Lifetime.Singleton);
            builder.Register<OpponentClubEntity>(Lifetime.Transient);
            builder.Register<TeamEntity>(Lifetime.Singleton);

            // UseCase
            builder.Register<AudioUseCase>(Lifetime.Singleton);
            builder.Register<BbsimSaveUseCase>(Lifetime.Singleton);
            builder.Register<MessageUseCase>(Lifetime.Singleton);
            builder.Register<TrainingUseCase>(Lifetime.Singleton);
            builder.Register<BattleUseCase>(Lifetime.Singleton);
            builder.Register<TrainingSelectUseCase>(Lifetime.Singleton);
            builder.Register<MatchSimulateUseCase>(Lifetime.Singleton);

            // BBSim固有のUseCaseを「IInitializableUseCase」として登録する
            builder.Register<TeamInitializeUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();
            builder.Register<CalendarUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase, CalendarUseCase>();
            builder.Register<BbsimLoadUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();

            // Game実行のために初期化後にカードを選択させるためのUseCase
            builder.Register<GameStartTestUseCase>(Lifetime.Singleton)
                .As<IInitializableUseCase>();

            // Presenter (通常のクラスとして登録)
            builder.Register<OptionPresenter>(Lifetime.Singleton)
                 .As<IStartable, OptionPresenter>();
            builder.Register<PlayerClubPresenter>(Lifetime.Singleton)
                 .As<IStartable, PlayerClubPresenter>(); // ★ .As<...> を追加
            builder.Register<SchedulePresenter>(Lifetime.Singleton)
                 .As<IStartable, SchedulePresenter>();

            // Commonから汎用クラスを登録
            builder.Register<GameInitializeUseCase>(Lifetime.Singleton);

            // ヒエラルキー上のコンポーネント
            builder.RegisterComponentInHierarchy<SaveManager>();

            // Serializeしたコンポーネント
            builder.RegisterComponent(_uiCanvas);

            // 初期化の起点となる唯一のEntryPoint
            builder.RegisterEntryPoint<ApplicationInitializer>();
        }
    }
}
