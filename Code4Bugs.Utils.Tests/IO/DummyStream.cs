using Code4Bugs.Utils.IO;
using System;

namespace Code4Bugs.Utils.Tests.IO
{
    internal class DummyStream : ICommStream
    {
        private byte[] _buffer;

        public DummyStream(byte[] buffer = null)
        {
            _buffer = buffer;
        }

        public int Available { get; set; }

        public int ReadTimeout { get; set; }

        public void Dispose()
        {
        }

        public int Read(byte[] buffer, int offset, int length)
        {
            var readLength = Math.Min(_buffer.Length, length);
            Array.Copy(_buffer, 0, buffer, offset, readLength);
            return readLength;
        }

        public void Write(byte[] buffer, int offset, int length)
        {
        }
    }
}