namespace Code4Bugs.Utils.SystemManagement
{
    public interface IRunner
    {
        bool IsRunning { get; }

        void Start();

        void Stop(bool force = false);

        void WaitForExit(int timeout = int.MaxValue);
    }
}