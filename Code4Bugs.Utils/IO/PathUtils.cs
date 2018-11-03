using System;
using System.IO;

namespace Code4Bugs.Utils.IO
{
    public static class PathUtils
    {
        public static bool IsValidPath(string path)
        {
            try
            {
                Path.GetFullPath(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }
    }
}