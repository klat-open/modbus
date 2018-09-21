using System;
using System.IO;
using System.Threading.Tasks;

namespace Code4Bugs.Utils.Intercomm
{
    public interface IFastServer
    {
        Task HandleAsync(string topic, Action<object, Stream> worker);

        Task HandleAsync(string topic, IWorker worker);

        Task HandleOneAsync(string topic, Action<object, Stream> worker);

        Task HandleOneAsync(string topic, IWorker worker);
    }
}