using System;
using System.Threading;
using System.Windows.Forms;
using TQElectronic.Utils.Event;

namespace TQElectronic.Utils.WinForms.Event
{
    public class ThreadHelper : IThreadHelper
    {
        private int _mainThreadId;
        private Control _control;

        public void Initialize()
        {
            this._mainThreadId = Thread.CurrentThread.ManagedThreadId;
            this._control = new Control();
            this._control.CreateControl();
        }

        public bool IsMainThread()
        {
            return Thread.CurrentThread.ManagedThreadId == this._mainThreadId;
        }

        public void NewThread(Action job)
        {
            new Thread((ThreadStart)(() => job())).Start();
        }

        public void RunInMain(Action job, bool wait)
        {
            if (this._control.IsDisposed || this._control.Disposing)
                return;
            try
            {
                if (wait)
                    this._control.Invoke((MethodInvoker)(() => job()));
                else
                    this._control.BeginInvoke((MethodInvoker)(() => job()));
            }
            catch
            {
            }
        }
    }
}