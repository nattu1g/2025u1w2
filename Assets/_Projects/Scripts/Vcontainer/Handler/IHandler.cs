using System;

namespace BBSim.Vcontainer.Handler
{
    public interface IHandler : IDisposable
    {
        public void Clear();
    }
}
