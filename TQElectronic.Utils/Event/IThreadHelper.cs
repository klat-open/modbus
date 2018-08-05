using System;

namespace TQElectronic.Utils.Event
{
    public interface IThreadHelper
    {
        bool IsMainThread();

        void NewThread(Action job);

        void RunInMain(Action job, bool wait);
    }
}