using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Code4Bugs.Utils.Timers
{
    public static class TimerUtils
    {
        public static async void ExecuteAfter(this Action action, int delayMillis)
        {
            await Task.Delay(delayMillis);
            action();
        }

        public static async void ExecuteAfter<T>(this Action<T> action, int delayMillis, T state)
        {
            await Task.Delay(delayMillis);
            action(state);
        }

        public static void Loop(this Func<bool> callback, int intervalMillis)
        {
            Timer timer = null;
            timer = new Timer(state =>
            {
                if (!callback())
                {
                    timer?.Dispose();
                    timer = null;
                }
            }, null, 0, intervalMillis);
            GC.KeepAlive(timer);
        }

        public static void LoopFixed(this Func<bool> callback, int intervalMillis)
        {
            Timer timer = null;
            var stopwatch = new Stopwatch();

            timer = new Timer(state =>
            {
                stopwatch?.Restart();

                if (!callback())
                {
                    timer?.Dispose();
                    timer = null;
                    stopwatch = null;
                    return;
                }

                var elapsedMillis = stopwatch != null ? stopwatch.ElapsedMilliseconds : 0;
                timer?.Change(Math.Max(0, intervalMillis - elapsedMillis), Timeout.Infinite);
            }, null, 0, Timeout.Infinite);

            GC.KeepAlive(timer);
            GC.KeepAlive(stopwatch);
        }
    }
}