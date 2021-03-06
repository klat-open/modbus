using System.Text;

namespace Code4Bugs.Utils.SystemManagement
{
    public class ProcessInfo
    {
        public string TotalMemoryUsed { get; set; }
        public string RunningTime { get; set; }
        public string CommandLine { get; set; }
        public string ExecutablePath { get; set; }
        public string WorkingDirectory { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Total Memory Used: " + TotalMemoryUsed);
            builder.AppendLine("Running Time: " + RunningTime);
            builder.AppendLine("Command Line: " + CommandLine);
            builder.AppendLine("Executable Path: " + ExecutablePath);
            builder.Append("Working Directory: " + WorkingDirectory);
            return builder.ToString();
        }
    }
}