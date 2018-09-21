using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Code4Bugs.Utils.Intercomm.Internal;

namespace Code4Bugs.Utils.Intercomm.Sockets
{
    public class Server : IServer, IDisposable
    {
        private readonly TcpListener _listener;
        private readonly Dictionary<string, IWorker> _workers;
        private IExecutor _executor;
        private volatile bool _keepProcessing;

        public Server(int port)
        {
            this._listener = new TcpListener(IPAddress.Loopback, port);
            this._workers = new Dictionary<string, IWorker>();
            this._executor = (IExecutor)new BackgroundThreadExecutor();
        }

        public async Task ListenAsync()
        {
            this._keepProcessing = true;
            this._listener.Start();
            while (this._keepProcessing)
            {
                try
                {
                    TcpClient tcpClient = await this._listener.AcceptTcpClientAsync();
                    TcpClient client = tcpClient;
                    tcpClient = (TcpClient)null;
                    this.HandleClientAsync(client);
                    client = (TcpClient)null;
                }
                catch (Exception)
                {
                    if (!this._keepProcessing)
                        break;
                    throw;
                }
            }
        }

        private void HandleClientAsync(TcpClient client)
        {
            this._executor.Run((Action)(() =>
           {
               try
               {
                   this.HandleClient(client);
               }
               catch (SocketException)
               {
               }
               finally
               {
                   client.Close();
               }
           }));
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            while (true)
            {
                SendingData sendingData;
                string topic;
                do
                {
                    sendingData = MessageUtils.ReceiveObject<SendingData>((Stream)stream);
                    if (sendingData != null)
                        topic = sendingData.Topic;
                    else
                        goto label_2;
                }
                while (!this._workers.ContainsKey(topic));
                this._workers[topic].Handle(sendingData.Data, (Stream)stream);
            }
            label_2:;
        }

        public void RegisterWorker(string topic, IWorker worker)
        {
            if (this._workers.ContainsKey(topic))
                this._workers.Remove(topic);
            this._workers.Add(topic, worker);
        }

        public void RegisterWorker(string topic, Action<object, Stream> worker)
        {
            this.RegisterWorker(topic, (IWorker)new SimpleWorker(worker));
        }

        public void SetExecutor(IExecutor executor)
        {
            this._executor = executor;
        }

        public void Dispose()
        {
            this._keepProcessing = false;
            this._listener.Stop();
            this._workers.Clear();
        }
    }
}