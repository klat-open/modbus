using System;
using System.Threading;

namespace Code4Bugs.Utils.Intercomm
{
    public class BackgroundThreadExecutor : IExecutor
    {
        public void Run(Action executeMe)
        {
            new Thread((ThreadStart)(() => executeMe()))
            {
                IsBackground = true
            }.Start();
        }
    }
}