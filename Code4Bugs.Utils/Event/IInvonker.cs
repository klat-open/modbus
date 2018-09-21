using System;

namespace Code4Bugs.Utils.Event
{
    public interface IInvonker : IDisposable
    {
        void Send(Action action);

        void Post(Action action);
    }
}