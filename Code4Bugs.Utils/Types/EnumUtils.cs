using System;

namespace Code4Bugs.Utils.Types
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(this string text)
        {
            return (T)Enum.Parse(typeof(T), text, true);
        }
    }
}