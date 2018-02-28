using MetroFramework.Controls;

namespace GinClientApp.Dialogs
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
            this.chB_keepUserConfig = new MetroCheckBox();
            this.chB_keepLoginInfo = new MetroCheckBox();
            this.chB_keepCheckouts = new MetroCheckBox();
            this.button1 = new MetroButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // chB_keepUserConfig
            // 
            this.chB_keepUserConfig.AutoSize = true;
            this.chB_keepUserConfig.Location = new System.Drawing.Point(49, 63);
            this.chB_keepUserConfig.Name = "chB_keepUserConfig";
            this.chB_keepUserConfig.Size = new System.Drawing.Size(113, 17);
            this.chB_keepUserConfig.TabIndex = 1;
            this.chB_keepUserConfig.Text = "User Configuration";
            this.chB_keepUserConfig.CheckedChanged += new System.EventHandler(this.chB_keepUserConfig_CheckedChanged);
            // 
            // chB_keepLoginInfo
            // 
            this.chB_keepLoginInfo.AutoSize = true;
            this.chB_keepLoginInfo.Location = new System.Drawing.Point(49, 86);
            this.chB_keepLoginInfo.Name = "chB_keepLoginInfo";
            this.chB_keepLoginInfo.Size = new System.Drawing.Size(131, 17);
            this.chB_keepLoginInfo.TabIndex = 2;
            this.chB_keepLoginInfo.Text = "User Login information";
            this.chB_keepLoginInfo.CheckedChanged += new System.EventHandler(this.chB_keepLoginInfo_CheckedChanged);
            // 
            // chB_keepCheckouts
            // 
            this.chB_keepCheckouts.AutoSize = true;
            this.chB_keepCheckouts.Location = new System.Drawing.Point(49, 109);
            this.chB_keepCheckouts.Name = "chB_keepCheckouts";
            this.chB_keepCheckouts.Size = new System.Drawing.Size(77, 17);
            this.chB_keepCheckouts.TabIndex = 3;
            this.chB_keepCheckouts.Text = "Checkouts";
            this.chB_keepCheckouts.CheckedChanged += new System.EventHandler(this.chB_keepCheckouts_CheckedChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(379, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            // 
            // DeleteDataDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 174);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chB_keepCheckouts);
            this.Controls.Add(this.chB_keepLoginInfo);
            this.Controls.Add(this.chB_keepUserConfig);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeleteDataDlg";
            this.Text = "Please mark any data you wish to keep:";
            this.Load += new System.EventHandler(this.DeleteDataDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MetroCheckBox chB_keepUserConfig;
        private MetroCheckBox chB_keepLoginInfo;
        private MetroCheckBox chB_keepCheckouts;
        private MetroButton button1;
    }
}