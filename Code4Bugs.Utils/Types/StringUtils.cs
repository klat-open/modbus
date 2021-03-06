using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Code4Bugs.Utils.Types
{
    public static class StringUtils
    {
        private static readonly Random _random = new Random();

        public static string SplitCamelCase(this string st)
        {
            return string.IsNullOrEmpty(st)
                ? ""
                : Regex.Replace(Regex.Replace(st, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static bool IsInt(this string st)
        {
            return int.TryParse(st, out var test);
        }

        public static bool IsFloat(this string st)
        {
            return float.TryParse(st, out var test);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static int ToInt(this string st, int defaultValue = 0)
        {
            return st.IsInt() ? int.Parse(st) : defaultValue;
        }

        public static float ToFloat(this string st, float defaultValue = 0)
        {
            return st.IsFloat() ? float.Parse(st) : defaultValue;
        }

        public static byte[] ToBytes(this string st, char split, NumberStyles numberStyles)
        {
            if (string.IsNullOrEmpty(st) || string.IsNullOrWhiteSpace(st))
                return new byte[] { };

            var parts = st.Trim().Split(new char[] { split }, StringSplitOptions.RemoveEmptyEntries);
            return parts
                .Select(part => byte.Parse(part, numberStyles))
                .ToArray();
        }

        public static byte[] ToBytes(this string st, char split)
        {
            return ToBytes(st, split, NumberStyles.HexNumber);
        }

        public static byte[] ToBytes(this string st)
        {
            return ToBytes(st, ' ', NumberStyles.HexNumber);
        }
    }
}