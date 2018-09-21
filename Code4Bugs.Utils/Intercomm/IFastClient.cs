using System.Threading.Tasks;

namespace Code4Bugs.Utils.Intercomm
{
    public interface IFastClient
    {
        Task SendAsync(string topic, object data);
    }
}