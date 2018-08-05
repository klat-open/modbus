using System;
using System.Threading;

namespace TQElectronic.Utils.Intercomm
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