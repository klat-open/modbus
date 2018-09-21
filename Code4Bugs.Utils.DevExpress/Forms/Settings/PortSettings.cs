using Config4Net.Core;
using System.IO.Ports;

namespace Code4Bugs.Utils.DevExpress.Forms.Settings
{
    [Config(Key = "port")]
    public class PortSettings
    {
        public string Name { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
    }
}