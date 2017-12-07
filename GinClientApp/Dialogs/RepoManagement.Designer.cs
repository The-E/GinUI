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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepoManagement));
            this.lvwRepositories = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvwRepositories
            // 
            resources.ApplyResources(this.lvwRepositories, "lvwRepositories");
            this.lvwRepositories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwRepositories.Name = "lvwRepositories";
            this.lvwRepositories.UseCompatibleStateImageBehavior = false;
            this.lvwRepositories.View = System.Windows.Forms.View.List;
            this.lvwRepositories.SelectedIndexChanged += new System.EventHandler(this.lvwRepositories_SelectedIndexChanged);
            // 
            // btnAddNew
            // 
            resources.ApplyResources(this.btnAddNew, "btnAddNew");
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txtRepoName
            // 
            resources.ApplyResources(this.txtRepoName, "txtRepoName");
            this.txtRepoName.Name = "txtRepoName";
            this.txtRepoName.TextChanged += new System.EventHandler(this.txtRepoName_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtGinCommandline
            // 
            resources.ApplyResources(this.txtGinCommandline, "txtGinCommandline");
            this.txtGinCommandline.Name = "txtGinCommandline";
            this.txtGinCommandline.TextChanged += new System.EventHandler(this.txtGinCommandline_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtMountpoint
            // 
            resources.ApplyResources(this.txtMountpoint, "txtMountpoint");
            this.txtMountpoint.Name = "txtMountpoint";
            // 
            // btnPickMountPnt
            // 
            resources.ApplyResources(this.btnPickMountPnt, "btnPickMountPnt");
            this.btnPickMountPnt.Name = "btnPickMountPnt";
            this.btnPickMountPnt.UseVisualStyleBackColor = true;
            this.btnPickMountPnt.Click += new System.EventHandler(this.btnPickMountPnt_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtPhysdir
            // 
            resources.ApplyResources(this.txtPhysdir, "txtPhysdir");
            this.txtPhysdir.Name = "txtPhysdir";
            // 
            // btnPickPhysDir
            // 
            resources.ApplyResources(this.btnPickPhysDir, "btnPickPhysDir");
            this.btnPickPhysDir.Name = "btnPickPhysDir";
            this.btnPickPhysDir.UseVisualStyleBackColor = true;
            this.btnPickPhysDir.Click += new System.EventHandler(this.btnPickPhysDir_Click);
            // 
            // btnCreate
            // 
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // RepoManagement
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCreate);
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
        private System.Windows.Forms.Button btnCreate;
    }
}