using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace TQElectronic.Utils.Limitation
{
    public sealed class FrequencyThrottler<T> : IThrottler<T>, IDisposable
    {
        private readonly Timer _timer;
        private readonly ConcurrentQueue<T> _queue;
        private readonly int _intervalInMilliseconds;
        private readonly int _executionsPerInterval;
        private volatile bool _disposing;
        private volatile bool _isTimerRunning;
        private volatile bool _processingQueue;
        private int _executionsSinceIntervalStart;

        public FrequencyThrottler(Action<T> callback, int intervalInMilliseconds, int executionsPerInterval)
        {
            this._intervalInMilliseconds = intervalInMilliseconds;
            this._executionsPerInterval = executionsPerInterval;
            this._timer = new Timer(new TimerCallback(this.OnTimerCallback), (object)callback, -1, -1);
            this._queue = new ConcurrentQueue<T>();
        }

        private void OnTimerCallback(object state)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this._executionsSinceIntervalStart = 0;
            this.TryProcessQueue((Action<T>)state);
            if (this._disposing)
                return;
            this._timer.Change(Math.Max(0L, (long)this._intervalInMilliseconds - stopwatch.ElapsedMilliseconds), -1L);
        }

        private void TryProcessQueue(Action<T> callback)
        {
            if (this._processingQueue)
                return;
            lock (this._timer)
            {
                if (this._processingQueue)
                    return;
                this._processingQueue = true;
                try
                {
                    this.ProcessQueue(callback);
                }
                finally
                {
                    this._processingQueue = false;
                }
            }
        }

        private void ProcessQueue(Action<T> callback)
        {
            T result;
            while (this._executionsSinceIntervalStart < this._executionsPerInterval && this._queue.TryDequeue(out result))
            {
                Interlocked.Increment(ref this._executionsSinceIntervalStart);
                callback(result);
            }
        }

        public void Execute(T state)
        {
            this._queue.Enqueue(state);
            if (this._isTimerRunning || this._disposing)
                return;
            this._isTimerRunning = true;
            this._timer.Change(this._intervalInMilliseconds, -1);
        }

        public void Dispose()
        {
            this._disposing = true;
            this._timer?.Dispose();
        }
    }
}