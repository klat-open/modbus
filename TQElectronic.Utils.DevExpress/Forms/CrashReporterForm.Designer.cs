namespace TQElectronic.Utils.DevExpress.Forms
{
    partial class CrashReporterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrashReporterForm));
            this.labelControl1 = new global::DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new global::DevExpress.XtraEditors.MemoEdit();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pnlWorkspaec)).BeginInit();
            this.pnlWorkspaec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlWorkspaec
            // 
            this.pnlWorkspaec.Controls.Add(this.memoEdit1);
            this.pnlWorkspaec.Controls.Add(this.labelControl1);
            this.pnlWorkspaec.Size = new System.Drawing.Size(544, 427);
            // 
            // labelControl1
            // 
            this.labelControl1.AllowHtmlString = true;
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.VAlignment = global::DevExpress.Utils.VertAlignment.Top;
            this.labelControl1.Appearance.TextOptions.WordWrap = global::DevExpress.Utils.WordWrap.Wrap;
            this.labelControl1.AutoSizeMode = global::DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(520, 128);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = resources.GetString("labelControl1.Text");
            // 
            // memoEdit1
            // 
            this.memoEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoEdit1.Location = new System.Drawing.Point(12, 166);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.ReadOnly = true;
            this.memoEdit1.Properties.WordWrap = false;
            this.memoEdit1.Size = new System.Drawing.Size(520, 249);
            this.memoEdit1.TabIndex = 1;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";
            this.saveFileDialog1.Title = "Save Crash Report";
            // 
            // CrashReporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 478);
            this.Name = "CrashReporterForm";
            this.Text = "Crash Reporter";
            ((System.ComponentModel.ISupportInitialize)(this.pnlWorkspaec)).EndInit();
            this.pnlWorkspaec.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private global::DevExpress.XtraEditors.LabelControl labelControl1;
        private global::DevExpress.XtraEditors.MemoEdit memoEdit1;
        private global::System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}