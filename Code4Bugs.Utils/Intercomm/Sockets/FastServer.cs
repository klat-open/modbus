using System;
using System.IO;
using System.Threading.Tasks;
using Code4Bugs.Utils.Intercomm.Internal;

namespace Code4Bugs.Utils.Intercomm.Sockets
{
    public class FastServer : IFastServer
    {
        private readonly int _port;

        public FastServer(int port)
        {
            this._port = port;
        }

        public Task HandleAsync(string topic, Action<object, Stream> worker)
        {
            return this.HandleAsync(topic, (IWorker)new SimpleWorker(worker));
        }

        public Task HandleAsync(string topic, IWorker worker)
        {
            return Task.Run((Action)(() =>
           {
               using (Server server = new Server(this._port))
               {
                   server.RegisterWorker(topic, worker);
                   server.ListenAsync().Wait();
               }
           }));
        }

        public Task HandleOneAsync(string topic, Action<object, Stream> worker)
        {
            return this.HandleOneAsync(topic, (IWorker)new SimpleWorker(worker));
        }

        public Task HandleOneAsync(string topic, IWorker worker)
        {
            return Task.Run((Action)(() =>
           {
               Server server = new Server(this._port);
               try
               {
                   server.RegisterWorker(topic, (Action<object, Stream>)((request, stream) =>
             {
                   worker?.Handle(request, stream);
                   server.Dispose();
               }));
                   server.ListenAsync().Wait();
               }
               finally
               {
                   if (server != null)
                       server.Dispose();
               }
           }));
        }
    }
}