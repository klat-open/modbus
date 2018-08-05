using System;
using System.IO;

namespace TQElectronic.Utils.Intercomm.Internal
{
    public class SimpleWorker : IWorker
    {
        private readonly Action<object, Stream> _handler;

        public SimpleWorker(Action<object, Stream> handler)
        {
            this._handler = handler;
        }

        public void Handle(object request, Stream ioStream)
        {
            this._handler(request, ioStream);
        }
    }
}