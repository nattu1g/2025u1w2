using Scripts.UI;
using Scripts.Vcontainer.Presenter;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts.Vcontainer.Installer
{
    public class MainLifeTimeScope : LifetimeScope
    {
        [SerializeField] private UICanvas _uiCanvas;

        protected override void Configure(IContainerBuilder builder)
        {
            // ピュアC#
            //builder.Register<TestEntity>(Lifetime.Singleton);
            // ヒエラルキー上のコンポーネント
            //builder.RegisterComponentInHierarchy<BattleComponentAssembly>();
            // Serializeしたコンポーネント
            builder.RegisterComponent(_uiCanvas);
            // 起点のとなるエントリーポイント
            builder.RegisterEntryPoint<MainPresenter>();

        }
    }
}
