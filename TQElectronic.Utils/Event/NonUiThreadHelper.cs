using System;
using System.Threading.Tasks;

namespace TQElectronic.Utils.Event
{
    internal class NonUiThreadHelper : IThreadHelper
    {
        public bool IsMainThread()
        {
            return false;
        }

        public void NewThread(Action job)
        {
            Task.Run(job);
        }

        public void RunInMain(Action job, bool wait)
        {
            if (wait)
                Task.Run(job).Wait();
            else
                Task.Run(job);
        }
    }
}