using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Code4Bugs.Utils.DX.Properties;
using Code4Bugs.Utils.WinForms.Crash;

namespace Code4Bugs.Utils.DX.Forms
{
    public partial class CrashReporterForm : ChooserForm, IReportWindow
    {
        private Exception _exception;
        private string _productName;

        public CrashReporterForm()
        {
            InitializeComponent();
            GetAcceptButton().Text = "Save...";
            GetAcceptButton().Enabled = false;
        }

        public void Initialize(Exception exception, string productName, string devMail)
        {
            if (string.IsNullOrEmpty(productName))
                productName = Application.ProductName;
            if (string.IsNullOrEmpty(devMail))
                devMail = Resources.devMail;

            var devMailLink = $"<href=mailto:{devMail}>{devMail}</href>";
            labelControl1.Text = string.Format(labelControl1.Text, productName, devMailLink, exception.Message);
            memoEdit1.Text = exception.ToString();
            Text = $"{productName} Crash Reporter";
            GetAcceptButton().Enabled = true;

            _exception = exception;
            _productName = productName;
        }

        protected override void OnAccept()
        {
            if (_exception != null && saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                var reportFileName = saveFileDialog1.FileName;
                var reportContent = CrashHandler.GenerateReport(_exception, _productName);
                File.WriteAllText(reportFileName, reportContent, Encoding.UTF8);
                base.OnAccept();
            }
        }
    }
}