using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Code4Bugs.Utils.BuildTools
{
    public static class DXLicensesFixer
    {
        public static string[] Fix(string projectDir)
        {
            if (string.IsNullOrEmpty(projectDir) || !Directory.Exists(projectDir))
                return new string[0];

            var licensesFiles = Directory.GetFiles(projectDir, "licenses.licx", SearchOption.AllDirectories);
            var resultList = new List<string>();
            foreach (var licensesFile in licensesFiles)
            {
                var oldContent = File.ReadAllLines(licensesFile);
                var newContent = RemoveIllegalComponents(oldContent);
                File.WriteAllLines(licensesFile, newContent);
                if (IsChanged(oldContent, newContent))
                    resultList.Add(licensesFile);
            }
            return resultList.ToArray();
        }

        private static string[] RemoveIllegalComponents(string[] oldContent)
        {
            return oldContent.Where(line => !line.Contains("InMemoryPatch")).ToArray();
        }

        private static bool IsChanged(string[] oldContent, string[] newContent)
        {
            if (oldContent.Length != newContent.Length)
                return true;

            for (var i = 0; i < oldContent.Length; i++)
            {
                if (oldContent[i] != newContent[i])
                    return true;
            }

            return false;
        }
    }
}