using System;

namespace TQElectronic.Utils.Types
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(this string text)
        {
            return (T)Enum.Parse(typeof(T), text, true);
        }
    }
}