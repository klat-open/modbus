using System;
using System.Threading.Tasks;

namespace TQElectronic.Utils.Intercomm
{
    public interface IClient : IDisposable
    {
        Task ConnectAsync();

        Task SendAsync(string topic, object data);

        Task<T> SendForResultAsync<T>(string topic, object data, int timeout = -1);
    }
}