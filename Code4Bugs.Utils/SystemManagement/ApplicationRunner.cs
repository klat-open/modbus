using Code4Bugs.Utils.Event;
using System.Diagnostics;

namespace Code4Bugs.Utils.SystemManagement
{
    public sealed class ApplicationRunner : IRunner
    {
        private readonly string _executablePath;
        private readonly string _arguments;
        private readonly bool _runInBackground;
        private Process _currentProcess;

        public bool IsRunning
        {
            get
            {
                var process = _currentProcess;
                return RunnerUtils.CheckRunning(process);
            }
        }

        public ApplicationRunner(
            string executablePath,
            string arguments = null,
            bool runInBackground = false)
        {
            Precondition.ArgumentNotNull(executablePath);

            _executablePath = executablePath;
            _arguments = arguments;
            _runInBackground = runInBackground;
        }

        public void Start()
        {
            _currentProcess = RunnerUtils.Start(_executablePath, _arguments, _runInBackground);
        }

        public void Stop(bool force = false)
        {
            var process = _currentProcess;
            if (process == null || process.HasExited) return;
            _currentProcess = null;
            RunnerUtils.Stop(process, force);
        }

        public void WaitForExit(int timeout = int.MaxValue)
        {
            RunnerUtils.WaitForExit(_currentProcess, timeout);
        }
    }
}