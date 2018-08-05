using System;
using System.Diagnostics;
using System.Threading;

namespace TQElectronic.Utils.Limitation
{
    public sealed class TimeoutThrottler<T> : IThrottler<T>, IDisposable
    {
        private readonly int _timeoutInMilliseconds;
        private readonly Timer _timer;
        private volatile bool _isTimerRunning;
        private volatile bool _disposing;
        private volatile int _lastExecutedTime;
        private T _state;
        private bool _hasNewState;

        public TimeoutThrottler(Action<T> callback, int timeoutInMilliseconds)
        {
            this._timeoutInMilliseconds = timeoutInMilliseconds;
            this._timer = new Timer(new TimerCallback(this.OnTimerCallback), (object)callback, -1, -1);
        }

        private void OnTimerCallback(object data)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int timeoutInMilliseconds = this._timeoutInMilliseconds;
            bool disposing = this._disposing;
            T state;
            bool flag;
            lock (this._timer)
            {
                int num = this.GetNowAsMilliseconds() - this._lastExecutedTime;
                state = this._state;
                flag = this._hasNewState && num >= timeoutInMilliseconds && !disposing;
                if (flag)
                    this._hasNewState = false;
            }
            if (flag)
                ((Action<T>)data)(state);
            if (this._disposing)
                return;
            this._timer.Change(Math.Max(0L, (long)timeoutInMilliseconds - stopwatch.ElapsedMilliseconds), -1L);
        }

        public void Execute(T state)
        {
            lock (this._timer)
            {
                this._hasNewState = true;
                this._state = state;
                this._lastExecutedTime = this.GetNowAsMilliseconds();
            }
            if (this._isTimerRunning || this._disposing)
                return;
            this._isTimerRunning = true;
            this._timer.Change(this._timeoutInMilliseconds, -1);
        }

        private int GetNowAsMilliseconds()
        {
            return (int)(DateTime.Now.Ticks / 10000L);
        }

        public void Dispose()
        {
            this._disposing = true;
            this._timer?.Dispose();
        }
    }
}