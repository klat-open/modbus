using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Code4Bugs.Utils.Intercomm.Internal;

namespace Code4Bugs.Utils.Intercomm.Sockets
{
    public class Client : IClient, IDisposable
    {
        private readonly int _port;
        private readonly TcpClient _client;
        private volatile bool _keepProcessing;

        public Client(int port)
        {
            this._port = port;
            this._client = new TcpClient();
        }

        public async Task ConnectAsync()
        {
            lock (this._client)
                this._keepProcessing = true;
            await this._client.ConnectAsync(IPAddress.Loopback, this._port);
        }

        public Task SendAsync(string topic, object data)
        {
            return Task.Run((Action)(() =>
           {
               lock (this._client)
               {
                   if (!this._keepProcessing)
                       throw new InvalidOperationException("Client does not connect to server or client is closed.");
               }
               MessageUtils.SendObject((Stream)this._client.GetStream(), (object)new SendingData()
               {
                   Topic = topic,
                   Data = data
               });
           }));
        }

        public async Task<T> SendForResultAsync<T>(string topic, object data, int timeout = -1)
        {
            await this.SendAsync(topic, data);
            this._client.ReceiveTimeout = timeout;
            return MessageUtils.ReceiveObject<T>((Stream)this._client.GetStream());
        }

        public void Dispose()
        {
            lock (this._client)
                this._keepProcessing = false;
            this._client.Close();
        }
    }
}