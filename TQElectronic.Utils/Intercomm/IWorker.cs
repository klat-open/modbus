using System.IO;

namespace TQElectronic.Utils.Intercomm
{
    public interface IWorker
    {
        void Handle(object request, Stream ioStream);
    }
}