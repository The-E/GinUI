namespace GinClientApp.Dialogs
{
    partial class MetroRepoBrowser
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
            this.trVwRepositories = new System.Windows.Forms.TreeView();
            this.mBtnCheckout = new MetroFramework.Controls.MetroButton();
            this.mBtnCancel = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.mTxBRepoDescription = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // trVwRepositories
            // 
            this.trVwRepositories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trVwRepositories.FullRowSelect = true;
            this.trVwRepositories.HotTracking = true;
            this.trVwRepositories.Location = new System.Drawing.Point(24, 82);
            this.trVwRepositories.Name = "trVwRepositories";
            this.trVwRepositories.Size = new System.Drawing.Size(270, 181);
            this.trVwRepositories.TabIndex = 0;
            this.trVwRepositories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trVwRepositories_AfterSelect);
            this.trVwRepositories.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trVwRepositories_NodeMouseClick);
            // 
            // mBtnCheckout
            // 
            this.mBtnCheckout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnCheckout.Location = new System.Drawing.Point(24, 385);
            this.mBtnCheckout.Name = "mBtnCheckout";
            this.mBtnCheckout.Size = new System.Drawing.Size(75, 23);
            this.mBtnCheckout.TabIndex = 1;
            this.mBtnCheckout.Text = "Checkout";
            this.mBtnCheckout.UseSelectable = true;
            this.mBtnCheckout.Click += new System.EventHandler(this.mBtnCheckout_Click);
            // 
            // mBtnCancel
            // 
            this.mBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mBtnCancel.Location = new System.Drawing.Point(106, 385);
            this.mBtnCancel.Name = "mBtnCancel";
            this.mBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.mBtnCancel.TabIndex = 2;
            this.mBtnCancel.Text = "Cancel";
            this.mBtnCancel.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 60);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(137, 19);
            this.metroLabel1.TabIndex = 3;
            this.metroLabel1.Text = "Available Repositories";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(24, 267);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(77, 19);
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "Description:";
            // 
            // mTxBRepoDescription
            // 
            this.mTxBRepoDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBRepoDescription.CustomButton.Image = null;
            this.mTxBRepoDescription.CustomButton.Location = new System.Drawing.Point(182, 1);
            this.mTxBRepoDescription.CustomButton.Name = "";
            this.mTxBRepoDescription.CustomButton.Size = new System.Drawing.Size(87, 87);
            this.mTxBRepoDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBRepoDescription.CustomButton.TabIndex = 1;
            this.mTxBRepoDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBRepoDescription.CustomButton.UseSelectable = true;
            this.mTxBRepoDescription.CustomButton.Visible = false;
            this.mTxBRepoDescription.Lines = new string[] {
        "metroTextBox1"};
            this.mTxBRepoDescription.Location = new System.Drawing.Point(24, 290);
            this.mTxBRepoDescription.MaxLength = 32767;
            this.mTxBRepoDescription.Multiline = true;
            this.mTxBRepoDescription.Name = "mTxBRepoDescription";
            this.mTxBRepoDescription.PasswordChar = '\0';
            this.mTxBRepoDescription.PromptText = "Select a repository to see its description";
            this.mTxBRepoDescription.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBRepoDescription.SelectedText = "";
            this.mTxBRepoDescription.SelectionLength = 0;
            this.mTxBRepoDescription.SelectionStart = 0;
            this.mTxBRepoDescription.ShortcutsEnabled = true;
            this.mTxBRepoDescription.Size = new System.Drawing.Size(270, 89);
            this.mTxBRepoDescription.TabIndex = 5;
            this.mTxBRepoDescription.Text = "metroTextBox1";
            this.mTxBRepoDescription.UseSelectable = true;
            this.mTxBRepoDescription.WaterMark = "Select a repository to see its description";
            this.mTxBRepoDescription.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBRepoDescription.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // MetroRepoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 419);
            this.Controls.Add(this.mTxBRepoDescription);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.mBtnCancel);
            this.Controls.Add(this.mBtnCheckout);
            this.Controls.Add(this.trVwRepositories);
            this.Name = "MetroRepoBrowser";
            this.Text = "Repository Browser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trVwRepositories;
        private MetroFramework.Controls.MetroButton mBtnCheckout;
        private MetroFramework.Controls.MetroButton mBtnCancel;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox mTxBRepoDescription;
    }
}