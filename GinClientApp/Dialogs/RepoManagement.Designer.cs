namespace GinClientApp
{
    partial class RepoManagement
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
            this.lvwRepositories = new System.Windows.Forms.ListView();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.txtRepoName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGinCommandline = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMountpoint = new System.Windows.Forms.TextBox();
            this.btnPickMountPnt = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPhysdir = new System.Windows.Forms.TextBox();
            this.btnPickPhysDir = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvwRepositories
            // 
            this.lvwRepositories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwRepositories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwRepositories.Location = new System.Drawing.Point(13, 13);
            this.lvwRepositories.Name = "lvwRepositories";
            this.lvwRepositories.Size = new System.Drawing.Size(209, 339);
            this.lvwRepositories.TabIndex = 0;
            this.lvwRepositories.UseCompatibleStateImageBehavior = false;
            this.lvwRepositories.SelectedIndexChanged += new System.EventHandler(this.lvwRepositories_SelectedIndexChanged);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNew.Location = new System.Drawing.Point(13, 359);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(75, 23);
            this.btnAddNew.TabIndex = 1;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(146, 359);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txtRepoName
            // 
            this.txtRepoName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRepoName.Enabled = false;
            this.txtRepoName.Location = new System.Drawing.Point(343, 12);
            this.txtRepoName.Name = "txtRepoName";
            this.txtRepoName.Size = new System.Drawing.Size(152, 20);
            this.txtRepoName.TabIndex = 3;
            this.txtRepoName.TextChanged += new System.EventHandler(this.txtRepoName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(228, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name:";
            // 
            // txtGinCommandline
            // 
            this.txtGinCommandline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGinCommandline.Enabled = false;
            this.txtGinCommandline.Location = new System.Drawing.Point(343, 38);
            this.txtGinCommandline.Name = "txtGinCommandline";
            this.txtGinCommandline.Size = new System.Drawing.Size(152, 20);
            this.txtGinCommandline.TabIndex = 7;
            this.txtGinCommandline.TextChanged += new System.EventHandler(this.txtGinCommandline_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Gin Commandline:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(228, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Mountpoint:";
            // 
            // txtMountpoint
            // 
            this.txtMountpoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMountpoint.Enabled = false;
            this.txtMountpoint.Location = new System.Drawing.Point(343, 64);
            this.txtMountpoint.Name = "txtMountpoint";
            this.txtMountpoint.Size = new System.Drawing.Size(118, 20);
            this.txtMountpoint.TabIndex = 11;
            // 
            // btnPickMountPnt
            // 
            this.btnPickMountPnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickMountPnt.Enabled = false;
            this.btnPickMountPnt.Location = new System.Drawing.Point(467, 62);
            this.btnPickMountPnt.Name = "btnPickMountPnt";
            this.btnPickMountPnt.Size = new System.Drawing.Size(28, 23);
            this.btnPickMountPnt.TabIndex = 12;
            this.btnPickMountPnt.Text = "...";
            this.btnPickMountPnt.UseVisualStyleBackColor = true;
            this.btnPickMountPnt.Click += new System.EventHandler(this.btnPickMountPnt_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Physical Directory:";
            // 
            // txtPhysdir
            // 
            this.txtPhysdir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhysdir.Enabled = false;
            this.txtPhysdir.Location = new System.Drawing.Point(343, 91);
            this.txtPhysdir.Name = "txtPhysdir";
            this.txtPhysdir.Size = new System.Drawing.Size(118, 20);
            this.txtPhysdir.TabIndex = 14;
            // 
            // btnPickPhysDir
            // 
            this.btnPickPhysDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickPhysDir.Enabled = false;
            this.btnPickPhysDir.Location = new System.Drawing.Point(467, 89);
            this.btnPickPhysDir.Name = "btnPickPhysDir";
            this.btnPickPhysDir.Size = new System.Drawing.Size(28, 23);
            this.btnPickPhysDir.TabIndex = 15;
            this.btnPickPhysDir.Text = "...";
            this.btnPickPhysDir.UseVisualStyleBackColor = true;
            this.btnPickPhysDir.Click += new System.EventHandler(this.btnPickPhysDir_Click);
            // 
            // RepoManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 391);
            this.Controls.Add(this.btnPickPhysDir);
            this.Controls.Add(this.txtPhysdir);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnPickMountPnt);
            this.Controls.Add(this.txtMountpoint);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtGinCommandline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRepoName);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(this.lvwRepositories);
            this.Name = "RepoManagement";
            this.Text = "RepoManagement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RepoManagement_FormClosing);
            this.Load += new System.EventHandler(this.RepoManagement_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwRepositories;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TextBox txtRepoName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGinCommandline;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMountpoint;
        private System.Windows.Forms.Button btnPickMountPnt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPhysdir;
        private System.Windows.Forms.Button btnPickPhysDir;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}