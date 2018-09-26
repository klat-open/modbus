using DevExpress.XtraEditors;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Code4Bugs.Utils.DX.Forms
{
    public partial class BorderlessForm : XtraForm
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private Control _titleBar;
        private bool _enableMovableOnAllControls;

        protected bool EnableMovableOnAllControls
        {
            get => _enableMovableOnAllControls;
            set
            {
                _enableMovableOnAllControls = value;
                if (_enableMovableOnAllControls)
                    EnableMovableOnControl(this);
                else
                    DisableMovableOnControl(this);
            }
        }

        public override string Text
        {
            get => base.Text; set
            {
                base.Text = value;
                UpdateTitleBarText();
            }
        }

        public BorderlessForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_enableMovableOnAllControls)
                DisableMovableOnControl(this);

            base.OnFormClosing(e);
        }

        protected void EnableMovableOnControl(Control control)
        {
            IterateControl(control, c => { c.MouseDown += _control_MouseDown; });
        }

        protected void DisableMovableOnControl(Control control)
        {
            IterateControl(control, c => { c.MouseDown -= _control_MouseDown; });
        }

        protected void IterateControl(Control control, Action<Control> onControl)
        {
            onControl(control);
            foreach (Control child in control.Controls)
            {
                IterateControl(child, onControl);
            }
        }

        protected void RegisterTitleBar(Control titleBar)
        {
            if (_titleBar != null)
                DisableMovableOnControl(_titleBar);

            _titleBar = titleBar;

            if (_titleBar != null)
                EnableMovableOnControl(_titleBar);

            UpdateTitleBarText();
        }

        private void UpdateTitleBarText()
        {
            if (_titleBar != null)
            {
                if (_titleBar is Label label)
                    label.Text = Text;
                else if (_titleBar is LabelControl labelControl)
                    labelControl.Text = Text;
            }
        }

        private void _control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}