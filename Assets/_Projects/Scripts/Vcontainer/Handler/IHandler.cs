using System;
using UnityEngine;

namespace Scripts.Vcontainer.Handler
{
    public interface IHandler : IDisposable
    {
        public void Clear();
    }
}
