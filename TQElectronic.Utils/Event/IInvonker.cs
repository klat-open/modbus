using System;

namespace TQElectronic.Utils.Event
{
    public interface IInvonker : IDisposable
    {
        void Send(Action action);

        void Post(Action action);
    }
}