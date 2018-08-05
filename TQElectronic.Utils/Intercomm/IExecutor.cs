using System;

namespace TQElectronic.Utils.Intercomm
{
    public interface IExecutor
    {
        void Run(Action executeMe);
    }
}