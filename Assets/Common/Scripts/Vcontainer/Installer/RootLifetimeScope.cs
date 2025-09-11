using AudioConductor.Runtime.Core.Models;
using Common.Features;
using Common.Vcontainer.Entity;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Common.Vcontainer.Installer
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

