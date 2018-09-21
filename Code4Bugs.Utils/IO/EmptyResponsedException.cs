using System;

namespace Code4Bugs.Utils.IO
{
    public class EmptyResponsedException : Exception
    {
        public EmptyResponsedException() : base()
        {
        }

        public EmptyResponsedException(string message) : base(message)
        {
        }
    }
}