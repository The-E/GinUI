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
            this.cmbAutoUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAutoUpdates.FormattingEnabled = true;
            this.cmbAutoUpdates.Items.AddRange(new object[] {
            "Never",
            "Every 5 minutes",
            "Every 15 minutes",
            "Every 30 minutes",
            "Every 60 minutes"});
            this.cmbAutoUpdates.Location = new System.Drawing.Point(189, 12);
            this.cmbAutoUpdates.Name = "cmbAutoUpdates";
            this.cmbAutoUpdates.Size = new System.Drawing.Size(215, 21);
            this.cmbAutoUpdates.TabIndex = 0;
            this.cmbAutoUpdates.SelectedIndexChanged += new System.EventHandler(this.cmbAutoUpdates_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Perform automatic updates:";
            // 
            // cmbCheckoutOptions
            // 
            this.cmbCheckoutOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCheckoutOptions.FormattingEnabled = true;
            this.cmbCheckoutOptions.Items.AddRange(new object[] {
            "Leave files in Annex",
            "Download all files"});
            this.cmbCheckoutOptions.Location = new System.Drawing.Point(189, 40);
            this.cmbCheckoutOptions.Name = "cmbCheckoutOptions";
            this.cmbCheckoutOptions.Size = new System.Drawing.Size(215, 21);
            this.cmbCheckoutOptions.TabIndex = 2;
            this.cmbCheckoutOptions.SelectedIndexChanged += new System.EventHandler(this.cmbCheckoutOptions_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "When performing a new checkout:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(329, 70);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(248, 70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // GlobalOptionsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 105);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCheckoutOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAutoUpdates);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GlobalOptionsDlg";
            this.Text = "GIN Client Options";
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