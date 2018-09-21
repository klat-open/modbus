using System;

namespace Code4Bugs.Utils.IO
{
    public interface ICommStream : IDisposable
    {
        int Available { get; }
        int ReadTimeout { get; set; }

        void Write(byte[] buffer, int offset, int length);

        int Read(byte[] buffer, int offset, int length);
    }
}