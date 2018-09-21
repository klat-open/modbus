using System;

namespace Code4Bugs.Utils.Intercomm
{
    public interface IExecutor
    {
        void Run(Action executeMe);
    }
}