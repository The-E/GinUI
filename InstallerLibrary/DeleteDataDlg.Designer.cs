namespace InstallerLibrary
{
    partial class DeleteDataDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteDataDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.chB_keepUserConfig = new System.Windows.Forms.CheckBox();
            this.chB_keepLoginInfo = new System.Windows.Forms.CheckBox();
            this.chB_keepCheckouts = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please mark any data you wish to keep:";
            // 
            // chB_keepUserConfig
            // 
            this.chB_keepUserConfig.AutoSize = true;
            this.chB_keepUserConfig.Location = new System.Drawing.Point(25, 31);
            this.chB_keepUserConfig.Name = "chB_keepUserConfig";
            this.chB_keepUserConfig.Size = new System.Drawing.Size(113, 17);
            this.chB_keepUserConfig.TabIndex = 1;
            this.chB_keepUserConfig.Text = "User Configuration";
            this.chB_keepUserConfig.UseVisualStyleBackColor = true;
            this.chB_keepUserConfig.CheckedChanged += new System.EventHandler(this.chB_keepUserConfig_CheckedChanged);
            // 
            // chB_keepLoginInfo
            // 
            this.chB_keepLoginInfo.AutoSize = true;
            this.chB_keepLoginInfo.Location = new System.Drawing.Point(25, 54);
            this.chB_keepLoginInfo.Name = "chB_keepLoginInfo";
            this.chB_keepLoginInfo.Size = new System.Drawing.Size(131, 17);
            this.chB_keepLoginInfo.TabIndex = 2;
            this.chB_keepLoginInfo.Text = "User Login information";
            this.chB_keepLoginInfo.UseVisualStyleBackColor = true;
            this.chB_keepLoginInfo.CheckedChanged += new System.EventHandler(this.chB_keepLoginInfo_CheckedChanged);
            // 
            // chB_keepCheckouts
            // 
            this.chB_keepCheckouts.AutoSize = true;
            this.chB_keepCheckouts.Location = new System.Drawing.Point(25, 77);
            this.chB_keepCheckouts.Name = "chB_keepCheckouts";
            this.chB_keepCheckouts.Size = new System.Drawing.Size(77, 17);
            this.chB_keepCheckouts.TabIndex = 3;
            this.chB_keepCheckouts.Text = "Checkouts";
            this.chB_keepCheckouts.UseVisualStyleBackColor = true;
            this.chB_keepCheckouts.CheckedChanged += new System.EventHandler(this.chB_keepCheckouts_CheckedChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(237, 91);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // DeleteDataDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 126);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chB_keepCheckouts);
            this.Controls.Add(this.chB_keepLoginInfo);
            this.Controls.Add(this.chB_keepUserConfig);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeleteDataDlg";
            this.Load += new System.EventHandler(this.DeleteDataDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chB_keepUserConfig;
        private System.Windows.Forms.CheckBox chB_keepLoginInfo;
        private System.Windows.Forms.CheckBox chB_keepCheckouts;
        private System.Windows.Forms.Button button1;
    }
}