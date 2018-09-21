using System.Net.Sockets;

namespace Code4Bugs.Utils.IO
{
    public class TcpStream : ICommStream
    {
        private TcpClient _client;
        private NetworkStream _networkStream;

        public int Available
        {
            get => _client.Available;
        }

        public int ReadTimeout
        {
            get => _networkStream.ReadTimeout;
            set => _networkStream.ReadTimeout = value;
        }

        public int Read(byte[] buffer, int offset, int length)
        {
            return _networkStream.Read(buffer, offset, length);
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            while (_networkStream.DataAvailable)
                _networkStream.ReadByte();
            _networkStream.Write(buffer, offset, length);
            _networkStream.Flush();
        }

        public TcpStream(TcpClient client)
        {
            _client = client;
            _networkStream = client.GetStream();
        }

        public void Dispose()
        {
            _networkStream.Close();
            _client.Close();
        }
    }
}