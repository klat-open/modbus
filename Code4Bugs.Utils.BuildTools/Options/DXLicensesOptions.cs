using CommandLine;

namespace Code4Bugs.Utils.BuildTools.Options
{
    [Verb("dx-fix", HelpText = "DevExpress fixes common error.")]
    internal class DXLicensesOptions
    {
        [Option("dir", HelpText = "Project directory", Required = false)]
        public string ProjectDirectory { get; set; }
    }
}