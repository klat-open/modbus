using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace Code4Bugs.Utils.DX.Forms
{
    public partial class ChooserForm : XtraForm
    {
        public ChooserForm()
        {
            InitializeComponent();
        }

        protected SimpleButton GetAcceptButton()
        {
            return btnOK;
        }

        protected SimpleButton GetCancelButton()
        {
            return btnCancel;
        }

        protected virtual void OnAccept()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            OnAccept();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}