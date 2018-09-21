using System.IO;

namespace Code4Bugs.Utils.Intercomm
{
    public interface IWorker
    {
        void Handle(object request, Stream ioStream);
    }
}