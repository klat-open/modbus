using Code4Bugs.Utils.Timers;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;

namespace Code4Bugs.Utils.Tests.Timers
{
    [TestFixture]
    internal class TimerUtilsTest
    {
        [Test]
        public void ExecuteAfter_should_delay_an_action_after_a_period_of_time()
        {
            var stopwatch = new Stopwatch();
            var ellapsedMillis = 0L;
            Action action = () =>
            {
                ellapsedMillis = stopwatch.ElapsedMilliseconds;
            };
            action.ExecuteAfter(1000);
            stopwatch.Start();
            Thread.Sleep(1100);
            Assert.GreaterOrEqual(ellapsedMillis, 1000);
        }

        [Test]
        public void Loop_should_stop_when_callback_return_false()
        {
            var maxLoop = 10;
            var loopCount = 0;
            Func<bool> callback = () =>
            {
                loopCount++;
                return loopCount < maxLoop;
            };
            callback.Loop(10);
            Thread.Sleep(maxLoop * 10 + 200);
            Assert.AreEqual(maxLoop, loopCount);
        }

        [Test]
        public void LoopFixed_should_stop_when_callback_return_false()
        {
            var maxLoop = 10;
            var loopCount = 0;
            Func<bool> callback = () =>
            {
                loopCount++;
                Thread.Sleep(5);
                return loopCount < maxLoop;
            };
            callback.LoopFixed(10);
            Thread.Sleep(maxLoop * 10 + 200);
            Assert.AreEqual(maxLoop, loopCount);
        }
    }
}