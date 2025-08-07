using AudioConductor.Runtime.Core.Models;
using Scripts.Features;
using Scripts.Vcontainer.Entity;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Scripts.Vcontainer.Installer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioConductorSettings _audioSetting;
        [SerializeField] private CueSheetAsset _cueSheetAsset;
        [SerializeField] private ComponentAssembly _componentAssembly;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_audioSetting);
            builder.RegisterComponent(_cueSheetAsset);
            builder.RegisterComponentInNewPrefab(_componentAssembly, Lifetime.Singleton).DontDestroyOnLoad();
            builder.Register<AudioEntity>(Lifetime.Singleton);
            builder.Register<VolumeEntity>(Lifetime.Singleton);
        }
    }
}

