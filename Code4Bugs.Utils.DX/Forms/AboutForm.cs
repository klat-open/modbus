using System;
using System.ComponentModel;
using System.Drawing;

namespace Code4Bugs.Utils.DX.Forms
{
    public partial class AboutForm : BorderlessForm
    {
        [Browsable(false)]
        public Image CoverImage => pictureBox1.Image;

        [Browsable(false)]
        public string AboutInfo => labelControl1.Text;

        public AboutForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IterateControl(this, control =>
            {
                control.Click += Control_Click;
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IterateControl(this, control =>
            {
                control.Click -= Control_Click;
            });
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}