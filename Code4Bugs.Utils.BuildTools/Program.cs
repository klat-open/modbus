using Code4Bugs.Utils.BuildTools.Options;
using CommandLine;
using System;
using System.Collections.Generic;

namespace Code4Bugs.Utils.BuildTools
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default
                .ParseArguments<DXLicensesOptions>(args)
                .MapResult(
                    (DXLicensesOptions options) => RunDXLicensesFixer(options),
                    (IEnumerable<Error> error) => ShowError(error)
                 );
        }

        private static int ShowError(IEnumerable<Error> error)
        {
            Console.WriteLine("Invalid parameters, --help for more details.");
            return 1;
        }

        private static int RunDXLicensesFixer(DXLicensesOptions options)
        {
            var projectDir = string.IsNullOrEmpty(options.ProjectDirectory)
                ? Environment.CurrentDirectory
                : options.ProjectDirectory;

            Console.WriteLine("Fixing DevExpress licenses.licx files in {0}", projectDir);
            var result = DXLicensesFixer.Fix(projectDir);
            if (result.Length == 0)
            {
                Console.WriteLine("Everything is good!");
            }
            else
            {
                foreach (var file in result)
                {
                    Console.WriteLine("Fixed {0}", file);
                }
            }

            return 0;
        }
    }
}