using System.IO.Ports;

namespace Code4Bugs.Utils.IO
{
    public class SerialStream : ICommStream
    {
        private readonly SerialPort _serialPort;

        public int Available
        {
            get => _serialPort?.BytesToRead ?? 0;
        }

        public int ReadTimeout
        {
            get => _serialPort?.ReadTimeout ?? 0;

            set
            {
                if (_serialPort != null)
                    _serialPort.ReadTimeout = value;
            }
        }

        public int Read(byte[] buffer, int offset, int length)
        {
            if (_serialPort != null)
                return _serialPort.Read(buffer, offset, length);
            return -1;
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            if (_serialPort != null)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.BaseStream.Flush();
                _serialPort.Write(buffer, offset, length);
                _serialPort.BaseStream.Flush();
            }
        }

        public SerialStream(SerialPort serialPort)
        {
            _serialPort = serialPort;
            serialPort.DiscardInBuffer();
        }

        public void Dispose()
        {
            _serialPort?.Close();
        }
    }
}