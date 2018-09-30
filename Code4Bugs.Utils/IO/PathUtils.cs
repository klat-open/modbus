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
    }
}