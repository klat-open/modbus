using System.Threading.Tasks;

namespace TQElectronic.Utils.Intercomm
{
    public interface IFastClient
    {
        Task SendAsync(string topic, object data);
    }
}