using System;

namespace Code4Bugs.Utils.Limitation
{
    public interface IThrottler<in T> : IDisposable
    {
        void Execute(T state);
    }
}