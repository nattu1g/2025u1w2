using MessagePipe;
using Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class UILifetimeScope : LifetimeScope
{
    [SerializeField] private UIView uiView;
    [SerializeField] private ObjectPublisher objectPublisher;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterMessagePipe();

        builder.RegisterComponent(uiView);
        builder.RegisterComponent(objectPublisher);

        builder.Register<UIPresenter>(Lifetime.Singleton);
        builder.Register<UIModel>(Lifetime.Singleton);
        builder.RegisterEntryPoint<UIPresenter>(Lifetime.Singleton);
        // builder.RegisterEntryPoint<UIModel>(Lifetime.Singleton);

    }
}