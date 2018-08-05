using System;
using System.IO.Ports;
using TQElectronic.Utils.DevExpress.Editors;
using TQElectronic.Utils.DevExpress.Forms.Internal;
using TQElectronic.Utils.DevExpress.Forms.Settings;

namespace TQElectronic.Utils.DevExpress.Forms
{
    public partial class PortSettingsForm : _PortSettingsForm
    {
        public Func<int[]> BaudRatesFactory { get; set; } = () => new int[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };

        public PortSettingsForm()
        {
            InitializeComponent();
        }

        protected override void LoadSettingToUi(PortSettings settings)
        {
            base.LoadSettingToUi(settings);

            cbbCOM.Initialize(SerialPort.GetPortNames(), settings.Name);
            cbbBaudRate.Initialize(BaudRatesFactory(), settings.BaudRate);
            cbbParity.Initialize(settings.Parity);
            cbbDataBits.Initialize(6, 8, settings.DataBits);
            cbbStopBits.Initialize(settings.StopBits);
        }

        protected override void LoadSettingsFromUi(PortSettings settings)
        {
            base.LoadSettingsFromUi(settings);

            settings.Name = cbbCOM.GetString();
            settings.BaudRate = cbbBaudRate.GetInt();
            settings.Parity = cbbParity.GetEnum<Parity>();
            settings.DataBits = cbbDataBits.GetInt();
            settings.StopBits = cbbStopBits.GetEnum<StopBits>();
        }
    }
}