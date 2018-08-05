using System;

namespace TQElectronic.Utils.Limitation
{
    public interface IThrottler<in T> : IDisposable
    {
        void Execute(T state);
    }
}