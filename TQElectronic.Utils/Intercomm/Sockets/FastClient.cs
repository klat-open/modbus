using System.Threading.Tasks;

namespace TQElectronic.Utils.Intercomm.Sockets
{
    public class FastClient : IFastClient
    {
        private readonly int _port;

        public FastClient(int port)
        {
            this._port = port;
        }

        public async Task SendAsync(string topic, object data)
        {
            Client client = new Client(this._port);
            try
            {
                await client.ConnectAsync();
                await client.SendAsync(topic, data);
            }
            finally
            {
                client?.Dispose();
            }
            client = (Client)null;
        }
    }
}