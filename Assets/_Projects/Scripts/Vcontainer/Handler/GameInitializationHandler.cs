using Scripts.Features;
using Scripts.Vcontainer.Entity;
using UnityEngine;

namespace Scripts.Vcontainer.Handler
{
    public class GameInitializationHandler : IHandler
    {
        readonly ComponentAssembly _componentAssembly;

        public GameInitializationHandler(
            ComponentAssembly componentAssembly
            )
        {
            _componentAssembly = componentAssembly;
        }
        public void Initialize()
        {
        }


        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}
