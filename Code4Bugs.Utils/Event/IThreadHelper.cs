using System;

namespace Code4Bugs.Utils.Event
{
    public interface IThreadHelper : IDisposable
    {
        bool IsMainThread();

        void NewThread(Action job);

        void RunInMainThread(Action job, bool wait);
    }
}