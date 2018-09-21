using System;
using System.Threading;
using System.Threading.Tasks;
using Code4Bugs.Utils.Event;

namespace Code4Bugs.Utils.Tests.Event
{
    internal class DummyThreadHelper : IThreadHelper
    {
        private readonly int _mainThreadId;

        public DummyThreadHelper(int mainThreadId)
        {
            _mainThreadId = mainThreadId;
        }

        public void Dispose()
        {
        }

        public bool IsMainThread()
        {
            return _mainThreadId == Thread.CurrentThread.ManagedThreadId;
        }

        public void NewThread(Action job)
        {
            Task.Run(job);
        }

        public void RunInMainThread(Action job, bool wait)
        {
            job();
        }
    }
}