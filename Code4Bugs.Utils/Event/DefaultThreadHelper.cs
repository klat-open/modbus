using System;
using System.Threading;
using System.Threading.Tasks;

namespace Code4Bugs.Utils.Event
{
    public sealed class DefaultThreadHelper : IThreadHelper
    {
        private int _mainThreadId;
        private readonly IInvonker _invonker;

        public bool IsMainThread()
        {
            return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }

        public void NewThread(Action job)
        {
            Precondition.ArgumentNotNull(job, nameof(job));
            Task.Run(job);
        }

        public void RunInMainThread(Action job, bool wait)
        {
            Precondition.ArgumentNotNull(job, nameof(job));

            try
            {
                var invonker = GetInvoker();

                if (wait)
                    invonker.Send(job);
                else
                    invonker.Post(job);
            }
            catch
            {
            }
        }

        private IInvonker GetInvoker()
        {
            if (_invonker == null)
                throw new NotImplementedException("Invoker does not pass yet.");
            return _invonker;
        }

        public DefaultThreadHelper(IInvonker invonker)
        {
            _invonker = invonker;
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Dispose()
        {
            _invonker?.Dispose();
        }
    }
}