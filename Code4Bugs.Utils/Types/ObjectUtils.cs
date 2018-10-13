using Newtonsoft.Json;

namespace Code4Bugs.Utils.Types
{
    public static class ObjectUtils
    {
        public static string ToJson(this object obj, bool pretty = false)
        {
            if (obj == null)
                return "NULL";

            return JsonConvert.SerializeObject(obj, pretty ? Formatting.Indented : Formatting.None);
        }
    }
}