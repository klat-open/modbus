using Newtonsoft.Json;
using System.IO;

namespace TQElectronic.Utils.Intercomm.Internal
{
    public static class MessageUtils
    {
        public static void SendObject(Stream stream, object objectData)
        {
            new StreamString(stream).WriteString(JsonConvert.SerializeObject(objectData));
        }

        public static T ReceiveObject<T>(Stream stream)
        {
            string str = new StreamString(stream).ReadString();
            return string.IsNullOrEmpty(str) ? default(T) : JsonConvert.DeserializeObject<T>(str);
        }

        public static T Unbox<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw);
        }
    }
}