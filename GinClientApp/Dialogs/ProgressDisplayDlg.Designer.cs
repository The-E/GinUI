namespace GinClientApp.Dialogs
{
    partial class ProgressDisplayDlg
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
            this.fileTransferProgress1 = new GinClientApp.Custom_Controls.FileTransferProgress();
            this.SuspendLayout();
            // 
            // fileTransferProgress1
            // 
            this.fileTransferProgress1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTransferProgress1.AutoSize = true;
            this.fileTransferProgress1.Filename = "";
            this.fileTransferProgress1.Location = new System.Drawing.Point(12, 13);
            this.fileTransferProgress1.Name = "fileTransferProgress1";
            this.fileTransferProgress1.Progress = 0;
            this.fileTransferProgress1.Size = new System.Drawing.Size(337, 105);
            this.fileTransferProgress1.Speed = "";
            this.fileTransferProgress1.State = "";
            this.fileTransferProgress1.TabIndex = 0;
            // 
            // ProgressDisplayDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(361, 130);
            this.Controls.Add(this.fileTransferProgress1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ProgressDisplayDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gin File Operations in Progress";
            this.Load += new System.EventHandler(this.ProgressDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Custom_Controls.FileTransferProgress fileTransferProgress1;
    }
}