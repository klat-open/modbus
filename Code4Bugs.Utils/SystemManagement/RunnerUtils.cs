using Code4Bugs.Utils.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Code4Bugs.Utils.SystemManagement
{
    internal static class RunnerUtils
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] StringBuilder lpExeName, [In, Out] ref uint lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GenerateConsoleCtrlEvent(int sigevent, int dwProcessGroupId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        private delegate bool ConsoleCtrlDelegate(uint CtrlType);

        public static bool CheckRunning(Process process)
        {
            return process != null && !process.HasExited;
        }

        public static Process Start(string executablePath, string arguments, bool runInBackground)
        {
            if (!File.Exists(executablePath)) return null;

            var startInfo = new ProcessStartInfo(executablePath, arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = runInBackground
            };

            return Process.Start(startInfo);
        }

        public static void Stop(Process process, bool force)
        {
            if (force)
            {
                process.Kill();
            }
            else
            {
                if (AttachConsole((uint)process.Id))
                {
                    SetConsoleCtrlHandler(null, true);
                    try
                    {
                        GenerateConsoleCtrlEvent(0, 0);
                    }
                    finally
                    {
                        FreeConsole();
                        SetConsoleCtrlHandler(null, false);
                    }
                }

                process.CloseMainWindow();
            }

            process.WaitForExit();
            process.Close();
        }

        public static void WaitForExit(Process process, int timeout)
        {
            if (process == null || process.HasExited) return;
            process.WaitForExit(timeout);
        }

        public static Process FindProcessByExecutablePath(string executablePath)
        {
            if (!File.Exists(executablePath)) return null;
            var processName = Path.GetFileNameWithoutExtension(executablePath);
            var foundProcesses = Process.GetProcessesByName(processName);
            var currentNormalizePath = PathUtils.NormalizePath(executablePath);
            return foundProcesses.Single(process => PathUtils.NormalizePath(GetMainModuleFileName(process)) == currentNormalizePath);
        }

        private static string GetMainModuleFileName(Process process, int buffer = 2048)
        {
            var fileNameBuilder = new StringBuilder(buffer);
            uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
            return QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength)
                ? fileNameBuilder.ToString()
                : null;
        }
    }
}