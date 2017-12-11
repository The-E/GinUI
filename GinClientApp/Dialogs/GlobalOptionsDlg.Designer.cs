namespace GinClientApp.Dialogs
{
    partial class GlobalOptionsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalOptionsDlg));
            this.cmbAutoUpdates = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCheckoutOptions = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbAutoUpdates
            // 
            resources.ApplyResources(this.cmbAutoUpdates, "cmbAutoUpdates");
            this.cmbAutoUpdates.FormattingEnabled = true;
            this.cmbAutoUpdates.Items.AddRange(new object[] {
            resources.GetString("cmbAutoUpdates.Items"),
            resources.GetString("cmbAutoUpdates.Items1"),
            resources.GetString("cmbAutoUpdates.Items2"),
            resources.GetString("cmbAutoUpdates.Items3"),
            resources.GetString("cmbAutoUpdates.Items4")});
            this.cmbAutoUpdates.Name = "cmbAutoUpdates";
            this.cmbAutoUpdates.SelectedIndexChanged += new System.EventHandler(this.cmbAutoUpdates_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbCheckoutOptions
            // 
            resources.ApplyResources(this.cmbCheckoutOptions, "cmbCheckoutOptions");
            this.cmbCheckoutOptions.FormattingEnabled = true;
            this.cmbCheckoutOptions.Items.AddRange(new object[] {
            resources.GetString("cmbCheckoutOptions.Items"),
            resources.GetString("cmbCheckoutOptions.Items1")});
            this.cmbCheckoutOptions.Name = "cmbCheckoutOptions";
            this.cmbCheckoutOptions.SelectedIndexChanged += new System.EventHandler(this.cmbCheckoutOptions_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // GlobalOptionsDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCheckoutOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAutoUpdates);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GlobalOptionsDlg";
            this.Load += new System.EventHandler(this.GlobalOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbAutoUpdates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCheckoutOptions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}