using Code4Bugs.Utils.DX.Forms;
using System;
using System.Windows.Forms;

namespace Code4Bugs.Utils.DX.Tests
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var f = new AboutForm();
            Application.Run(f);
        }
    }
}