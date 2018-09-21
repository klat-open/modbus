using System;

namespace Code4Bugs.Utils.IO
{
    public class DataCorruptedException : Exception
    {
        public DataCorruptedException() : base()
        {
        }

        public DataCorruptedException(string message) : base(message)
        {
        }
    }
}