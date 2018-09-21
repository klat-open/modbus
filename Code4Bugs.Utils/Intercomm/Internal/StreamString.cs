using System.IO;
using System.Text;

namespace Code4Bugs.Utils.Intercomm.Internal
{
    public class StreamString
    {
        private readonly Stream _ioStream;
        private readonly UnicodeEncoding _streamEncoding;

        public StreamString(Stream ioStream)
        {
            this._ioStream = ioStream;
            this._streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int count = this._ioStream.ReadByte() * 256 + this._ioStream.ReadByte();
            if (count < 0)
                return (string)null;
            byte[] numArray = new byte[count];
            this._ioStream.Read(numArray, 0, count);
            return this._streamEncoding.GetString(numArray);
        }

        public int WriteString(string outString)
        {
            byte[] bytes = this._streamEncoding.GetBytes(outString);
            int count = bytes.Length;
            if (count > (int)ushort.MaxValue)
                count = (int)ushort.MaxValue;
            this._ioStream.WriteByte((byte)(count / 256));
            this._ioStream.WriteByte((byte)(count & (int)byte.MaxValue));
            this._ioStream.Write(bytes, 0, count);
            this._ioStream.Flush();
            return bytes.Length + 2;
        }
    }
}