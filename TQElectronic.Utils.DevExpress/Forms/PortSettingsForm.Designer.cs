namespace TQElectronic.Utils.DevExpress.Forms
{
    partial class PortSettingsForm
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
            this.labelControl1 = new global::DevExpress.XtraEditors.LabelControl();
            this.cbbCOM = new global::DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new global::DevExpress.XtraEditors.LabelControl();
            this.cbbBaudRate = new global::DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new global::DevExpress.XtraEditors.LabelControl();
            this.cbbParity = new global::DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new global::DevExpress.XtraEditors.LabelControl();
            this.cbbDataBits = new global::DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new global::DevExpress.XtraEditors.LabelControl();
            this.cbbStopBits = new global::DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlWorkspaec)).BeginInit();
            this.pnlWorkspaec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbCOM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbBaudRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbParity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbDataBits.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbStopBits.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlWorkspaec
            // 
            this.pnlWorkspaec.Controls.Add(this.cbbStopBits);
            this.pnlWorkspaec.Controls.Add(this.labelControl5);
            this.pnlWorkspaec.Controls.Add(this.cbbDataBits);
            this.pnlWorkspaec.Controls.Add(this.labelControl4);
            this.pnlWorkspaec.Controls.Add(this.cbbParity);
            this.pnlWorkspaec.Controls.Add(this.labelControl3);
            this.pnlWorkspaec.Controls.Add(this.cbbBaudRate);
            this.pnlWorkspaec.Controls.Add(this.labelControl2);
            this.pnlWorkspaec.Controls.Add(this.cbbCOM);
            this.pnlWorkspaec.Controls.Add(this.labelControl1);
            this.pnlWorkspaec.Size = new System.Drawing.Size(304, 229);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(70, 37);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 16);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "COM:";
            // 
            // cbbCOM
            // 
            this.cbbCOM.Location = new System.Drawing.Point(118, 34);
            this.cbbCOM.Name = "cbbCOM";
            this.cbbCOM.Properties.Buttons.AddRange(new global::DevExpress.XtraEditors.Controls.EditorButton[] {
            new global::DevExpress.XtraEditors.Controls.EditorButton(global::DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbCOM.Properties.TextEditStyle = global::DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbCOM.Size = new System.Drawing.Size(146, 22);
            this.cbbCOM.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(39, 68);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(63, 16);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Baud Rate:";
            // 
            // cbbBaudRate
            // 
            this.cbbBaudRate.Location = new System.Drawing.Point(118, 62);
            this.cbbBaudRate.Name = "cbbBaudRate";
            this.cbbBaudRate.Properties.Buttons.AddRange(new global::DevExpress.XtraEditors.Controls.EditorButton[] {
            new global::DevExpress.XtraEditors.Controls.EditorButton(global::DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbBaudRate.Properties.TextEditStyle = global::DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbBaudRate.Size = new System.Drawing.Size(146, 22);
            this.cbbBaudRate.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(65, 93);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(37, 16);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Parity:";
            // 
            // cbbParity
            // 
            this.cbbParity.Location = new System.Drawing.Point(118, 90);
            this.cbbParity.Name = "cbbParity";
            this.cbbParity.Properties.Buttons.AddRange(new global::DevExpress.XtraEditors.Controls.EditorButton[] {
            new global::DevExpress.XtraEditors.Controls.EditorButton(global::DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbParity.Properties.TextEditStyle = global::DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbParity.Size = new System.Drawing.Size(146, 22);
            this.cbbParity.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(47, 121);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 16);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "Data Bits:";
            // 
            // cbbDataBits
            // 
            this.cbbDataBits.Location = new System.Drawing.Point(118, 118);
            this.cbbDataBits.Name = "cbbDataBits";
            this.cbbDataBits.Properties.Buttons.AddRange(new global::DevExpress.XtraEditors.Controls.EditorButton[] {
            new global::DevExpress.XtraEditors.Controls.EditorButton(global::DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbDataBits.Properties.TextEditStyle = global::DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbDataBits.Size = new System.Drawing.Size(146, 22);
            this.cbbDataBits.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(47, 149);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(55, 16);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "Stop Bits:";
            // 
            // cbbStopBits
            // 
            this.cbbStopBits.Location = new System.Drawing.Point(118, 146);
            this.cbbStopBits.Name = "cbbStopBits";
            this.cbbStopBits.Properties.Buttons.AddRange(new global::DevExpress.XtraEditors.Controls.EditorButton[] {
            new global::DevExpress.XtraEditors.Controls.EditorButton(global::DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbStopBits.Properties.TextEditStyle = global::DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbStopBits.Size = new System.Drawing.Size(146, 22);
            this.cbbStopBits.TabIndex = 1;
            // 
            // PortSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 280);
            this.Name = "PortSettingsForm";
            this.Text = "PortSettings";
            ((System.ComponentModel.ISupportInitialize)(this.pnlWorkspaec)).EndInit();
            this.pnlWorkspaec.ResumeLayout(false);
            this.pnlWorkspaec.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbCOM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbBaudRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbParity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbDataBits.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbStopBits.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private global::DevExpress.XtraEditors.ComboBoxEdit cbbCOM;
        private global::DevExpress.XtraEditors.LabelControl labelControl1;
        private global::DevExpress.XtraEditors.ComboBoxEdit cbbStopBits;
        private global::DevExpress.XtraEditors.LabelControl labelControl5;
        private global::DevExpress.XtraEditors.ComboBoxEdit cbbDataBits;
        private global::DevExpress.XtraEditors.LabelControl labelControl4;
        private global::DevExpress.XtraEditors.ComboBoxEdit cbbParity;
        private global::DevExpress.XtraEditors.LabelControl labelControl3;
        private global::DevExpress.XtraEditors.ComboBoxEdit cbbBaudRate;
        private global::DevExpress.XtraEditors.LabelControl labelControl2;
    }
}