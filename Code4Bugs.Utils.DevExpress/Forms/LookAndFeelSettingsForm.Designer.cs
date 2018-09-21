namespace Code4Bugs.Utils.DevExpress.Forms
{
    partial class LookAndFeelSettingsForm
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
            this.cbbSkin = new global::DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new global::DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlWorkspaec)).BeginInit();
            this.pnlWorkspaec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbSkin.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlWorkspaec
            // 
            this.pnlWorkspaec.Controls.Add(this.labelControl1);
            this.pnlWorkspaec.Controls.Add(this.cbbSkin);
            this.pnlWorkspaec.Size = new System.Drawing.Size(442, 256);
            // 
            // cbbSkin
            // 
            this.cbbSkin.Location = new System.Drawing.Point(101, 89);
            this.cbbSkin.Name = "cbbSkin";
            this.cbbSkin.Properties.Buttons.AddRange(new global::DevExpress.XtraEditors.Controls.EditorButton[] {
            new global::DevExpress.XtraEditors.Controls.EditorButton(global::DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbSkin.Properties.TextEditStyle = global::DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbSkin.Size = new System.Drawing.Size(278, 22);
            this.cbbSkin.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(53, 92);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(29, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Skin:";
            // 
            // LookAndFeelSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 307);
            this.Name = "LookAndFeelSettingsForm";
            this.Text = "LookAndFeelSettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.pnlWorkspaec)).EndInit();
            this.pnlWorkspaec.ResumeLayout(false);
            this.pnlWorkspaec.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbSkin.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private global::DevExpress.XtraEditors.LabelControl labelControl1;
        private global::DevExpress.XtraEditors.ComboBoxEdit cbbSkin;
    }
}