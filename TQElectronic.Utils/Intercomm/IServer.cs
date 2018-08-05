﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace TQElectronic.Utils.Intercomm
{
    public interface IServer : IDisposable
    {
        Task ListenAsync();

        void RegisterWorker(string topic, IWorker worker);

        void RegisterWorker(string topic, Action<object, Stream> worker);

        void SetExecutor(IExecutor executor);
    }
}