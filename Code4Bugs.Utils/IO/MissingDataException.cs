using System;

namespace Code4Bugs.Utils.IO
{
    public class MissingDataException : Exception
    {
        public MissingDataException() : base()
        {
        }

        public MissingDataException(string message) : base(message)
        {
        }
    }
}