using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TQElectronic.Utils.Types
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
    }
}