using Code4Bugs.Utils.Event;

namespace Code4Bugs.Utils.SystemManagement
{
    public sealed class SingleApplicationRunner : IRunner
    {
        private readonly ApplicationRunner _runner;
        private readonly string _executablePath;

        public bool IsRunning => _runner.IsRunning;

        public SingleApplicationRunner(
            string executablePath,
            string arguments = null,
            bool runInBackground = false)
        {
            Precondition.ArgumentNotNull(executablePath);

            _runner = new ApplicationRunner(executablePath, arguments, runInBackground);
            _executablePath = executablePath;
        }

        public void Start()
        {
            Start(false);
        }

        public void Start(bool dontWait)
        {
            var process = RunnerUtils.FindProcessByExecutablePath(_executablePath);
            if (process != null)
                RunnerUtils.Stop(process, dontWait);
            _runner.Start();
        }

        public void Stop(bool force = false)
        {
            _runner.Stop(force);
        }

        public void WaitForExit(int timeout = int.MaxValue)
        {
            _runner.WaitForExit(timeout);
        }
    }
}