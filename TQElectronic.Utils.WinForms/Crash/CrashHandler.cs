using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TQElectronic.Utils.SystemManagement;

namespace TQElectronic.Utils.WinForms.Crash
{
    public static class CrashHandler
    {
        private static Type _reportWindow;

        public static string ProductName { get; set; }
        public static string DevMail { get; set; }
        public static string CrashDir { get; set; }

        static CrashHandler()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void Register(Type reportWindow)
        {
            _reportWindow = reportWindow;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                ShowReport(exception);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                ShowReport(e.Exception);
            }
        }

        private static void ShowReport(Exception exception)
        {
            SaveToDisk(exception);
            if (_reportWindow != null)
            {
                var reportWindow = (IReportWindow)Activator.CreateInstance(_reportWindow);
                reportWindow.Initialize(exception, ProductName, DevMail);
                reportWindow.Show();
            }
        }

        private static void SaveToDisk(Exception exception)
        {
            var crashDir = string.IsNullOrEmpty(CrashDir)
                ? Path.Combine(Environment.CurrentDirectory, "crash")
                : CrashDir;

            if (!Directory.Exists(crashDir))
                Directory.CreateDirectory(crashDir);

            var fileName = $"{DateTime.Now:yyyy-MM-dd HH.mm.ss}.txt";
            var filePath = Path.Combine(crashDir, fileName);

            var report = GenerateReport(exception, ProductName);
            File.WriteAllText(filePath, report, Encoding.UTF8);
        }

        public static string GenerateReport(Exception exception, string productName)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Product: {productName}");
            builder.AppendLine($"Report date: {DateTime.Now}");
            builder.AppendLine($"-----------------------------------------------");
            builder.AppendLine($"System Information");
            builder.AppendLine(SystemInfoUtils.GetSystemInfo().ToString());
            builder.AppendLine($"-----------------------------------------------");
            builder.AppendLine($"Process Information");
            builder.AppendLine(ProcessInfoUtils.GetProcessInfo().ToString());
            builder.AppendLine($"-----------------------------------------------");
            builder.AppendLine($"Exception");
            builder.Append(exception);
            return builder.ToString();
        }
    }
}