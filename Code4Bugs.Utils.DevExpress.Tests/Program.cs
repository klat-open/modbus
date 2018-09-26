using Code4Bugs.Utils.DevExpress.Forms;
using System;
using System.Windows.Forms;

namespace Code4Bugs.Utils.DevExpress.Tests
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
            var form = new LookAndFeelSettingsForm();
            form.UseFlatSkin = true;
            form.Settings = new Forms.Settings.LookAndFeelSettings
            {
                SkinName = ""
            };
            Application.Run(form);
        }
    }
}