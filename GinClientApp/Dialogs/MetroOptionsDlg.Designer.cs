namespace GinClientApp.Dialogs
{
    partial class MetroOptionsDlg
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
            this.mTabCtrl = new MetroFramework.Controls.MetroTabControl();
            this.tpUser = new System.Windows.Forms.TabPage();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.mLblStatus = new MetroFramework.Controls.MetroLabel();
            this.mTxBPassword = new MetroFramework.Controls.MetroTextBox();
            this.mTxBUsername = new MetroFramework.Controls.MetroTextBox();
            this.mTxBServerAddress = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.tpGlobalOptions = new System.Windows.Forms.TabPage();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            this.mTglDownloadAnnex = new MetroFramework.Controls.MetroToggle();
            this.mBtnPickDefaultMountpointDir = new MetroFramework.Controls.MetroButton();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.mTxBDefaultMountpoint = new MetroFramework.Controls.MetroTextBox();
            this.mBtnPickDefaultCheckoutDir = new MetroFramework.Controls.MetroButton();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.mTxBDefaultCheckout = new MetroFramework.Controls.MetroTextBox();
            this.mCBxRepoUpdates = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.tpRepositories = new System.Windows.Forms.TabPage();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.mBtnCreateNew = new MetroFramework.Controls.MetroButton();
            this.mBtnCheckout = new MetroFramework.Controls.MetroButton();
            this.mBtnRemove = new MetroFramework.Controls.MetroButton();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.mLVwRepositories = new MetroFramework.Controls.MetroListView();
            this.colHdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdMountpoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdCheckoutDir = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.About = new System.Windows.Forms.TabPage();
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.mTxBLicense = new MetroFramework.Controls.MetroTextBox();
            this.mTxBGinService = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.mTxBGinCliVersion = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.mBtnOK = new MetroFramework.Controls.MetroButton();
            this.mBtnCancel = new MetroFramework.Controls.MetroButton();
            this.mProgWorking = new MetroFramework.Controls.MetroProgressSpinner();
            this.mLblWorking = new MetroFramework.Controls.MetroLabel();
            this.mTabCtrl.SuspendLayout();
            this.tpUser.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.tpGlobalOptions.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.tpRepositories.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            this.About.SuspendLayout();
            this.metroPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTabCtrl
            // 
            this.mTabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mTabCtrl.Controls.Add(this.tpUser);
            this.mTabCtrl.Controls.Add(this.tpGlobalOptions);
            this.mTabCtrl.Controls.Add(this.tpRepositories);
            this.mTabCtrl.Controls.Add(this.About);
            this.mTabCtrl.Location = new System.Drawing.Point(24, 64);
            this.mTabCtrl.Name = "mTabCtrl";
            this.mTabCtrl.SelectedIndex = 1;
            this.mTabCtrl.Size = new System.Drawing.Size(492, 294);
            this.mTabCtrl.TabIndex = 0;
            this.mTabCtrl.UseSelectable = true;
            // 
            // tpUser
            // 
            this.tpUser.Controls.Add(this.metroPanel1);
            this.tpUser.Location = new System.Drawing.Point(4, 38);
            this.tpUser.Name = "tpUser";
            this.tpUser.Size = new System.Drawing.Size(484, 252);
            this.tpUser.TabIndex = 0;
            this.tpUser.Text = "Login";
            this.tpUser.Visible = false;
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.mLblStatus);
            this.metroPanel1.Controls.Add(this.mTxBPassword);
            this.metroPanel1.Controls.Add(this.mTxBUsername);
            this.metroPanel1.Controls.Add(this.mTxBServerAddress);
            this.metroPanel1.Controls.Add(this.metroLabel3);
            this.metroPanel1.Controls.Add(this.metroLabel2);
            this.metroPanel1.Controls.Add(this.metroLabel1);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(484, 252);
            this.metroPanel1.TabIndex = 0;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // mLblStatus
            // 
            this.mLblStatus.AutoSize = true;
            this.mLblStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.mLblStatus.Location = new System.Drawing.Point(14, 106);
            this.mLblStatus.Name = "mLblStatus";
            this.mLblStatus.Size = new System.Drawing.Size(250, 19);
            this.mLblStatus.TabIndex = 8;
            this.mLblStatus.Text = "This place is reserved for status messages";
            this.mLblStatus.UseCustomForeColor = true;
            // 
            // mTxBPassword
            // 
            this.mTxBPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBPassword.CustomButton.Image = null;
            this.mTxBPassword.CustomButton.Location = new System.Drawing.Point(318, 1);
            this.mTxBPassword.CustomButton.Name = "";
            this.mTxBPassword.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBPassword.CustomButton.TabIndex = 1;
            this.mTxBPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBPassword.CustomButton.UseSelectable = true;
            this.mTxBPassword.CustomButton.Visible = false;
            this.mTxBPassword.Lines = new string[] {
        "metroTextBox3"};
            this.mTxBPassword.Location = new System.Drawing.Point(141, 70);
            this.mTxBPassword.MaxLength = 32767;
            this.mTxBPassword.Name = "mTxBPassword";
            this.mTxBPassword.PasswordChar = '*';
            this.mTxBPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBPassword.SelectedText = "";
            this.mTxBPassword.SelectionLength = 0;
            this.mTxBPassword.SelectionStart = 0;
            this.mTxBPassword.ShortcutsEnabled = true;
            this.mTxBPassword.Size = new System.Drawing.Size(340, 23);
            this.mTxBPassword.TabIndex = 7;
            this.mTxBPassword.Text = "metroTextBox3";
            this.mTxBPassword.UseSelectable = true;
            this.mTxBPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mTxBPassword.Leave += new System.EventHandler(this.mTxBPassword_Leave);
            // 
            // mTxBUsername
            // 
            this.mTxBUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBUsername.CustomButton.Image = null;
            this.mTxBUsername.CustomButton.Location = new System.Drawing.Point(318, 1);
            this.mTxBUsername.CustomButton.Name = "";
            this.mTxBUsername.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBUsername.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBUsername.CustomButton.TabIndex = 1;
            this.mTxBUsername.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBUsername.CustomButton.UseSelectable = true;
            this.mTxBUsername.CustomButton.Visible = false;
            this.mTxBUsername.Lines = new string[] {
        "metroTextBox2"};
            this.mTxBUsername.Location = new System.Drawing.Point(141, 41);
            this.mTxBUsername.MaxLength = 32767;
            this.mTxBUsername.Name = "mTxBUsername";
            this.mTxBUsername.PasswordChar = '\0';
            this.mTxBUsername.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBUsername.SelectedText = "";
            this.mTxBUsername.SelectionLength = 0;
            this.mTxBUsername.SelectionStart = 0;
            this.mTxBUsername.ShortcutsEnabled = true;
            this.mTxBUsername.Size = new System.Drawing.Size(340, 23);
            this.mTxBUsername.TabIndex = 6;
            this.mTxBUsername.Text = "metroTextBox2";
            this.mTxBUsername.UseSelectable = true;
            this.mTxBUsername.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBUsername.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mTxBUsername.Leave += new System.EventHandler(this.mTxBUsername_Leave);
            // 
            // mTxBServerAddress
            // 
            this.mTxBServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBServerAddress.CustomButton.Image = null;
            this.mTxBServerAddress.CustomButton.Location = new System.Drawing.Point(318, 1);
            this.mTxBServerAddress.CustomButton.Name = "";
            this.mTxBServerAddress.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBServerAddress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBServerAddress.CustomButton.TabIndex = 1;
            this.mTxBServerAddress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBServerAddress.CustomButton.UseSelectable = true;
            this.mTxBServerAddress.CustomButton.Visible = false;
            this.mTxBServerAddress.Lines = new string[] {
        "gin.g-node.org"};
            this.mTxBServerAddress.Location = new System.Drawing.Point(141, 12);
            this.mTxBServerAddress.MaxLength = 32767;
            this.mTxBServerAddress.Name = "mTxBServerAddress";
            this.mTxBServerAddress.PasswordChar = '\0';
            this.mTxBServerAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBServerAddress.SelectedText = "";
            this.mTxBServerAddress.SelectionLength = 0;
            this.mTxBServerAddress.SelectionStart = 0;
            this.mTxBServerAddress.ShortcutsEnabled = true;
            this.mTxBServerAddress.Size = new System.Drawing.Size(340, 23);
            this.mTxBServerAddress.TabIndex = 5;
            this.mTxBServerAddress.Text = "gin.g-node.org";
            this.mTxBServerAddress.UseSelectable = true;
            this.mTxBServerAddress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBServerAddress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(14, 70);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(63, 19);
            this.metroLabel3.TabIndex = 4;
            this.metroLabel3.Text = "Password";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(14, 41);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(68, 19);
            this.metroLabel2.TabIndex = 3;
            this.metroLabel2.Text = "Username";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(14, 12);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(121, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Gin Server Address";
            // 
            // tpGlobalOptions
            // 
            this.tpGlobalOptions.Controls.Add(this.metroPanel2);
            this.tpGlobalOptions.Location = new System.Drawing.Point(4, 38);
            this.tpGlobalOptions.Name = "tpGlobalOptions";
            this.tpGlobalOptions.Size = new System.Drawing.Size(484, 252);
            this.tpGlobalOptions.TabIndex = 1;
            this.tpGlobalOptions.Text = "Global Options";
            this.tpGlobalOptions.Visible = false;
            // 
            // metroPanel2
            // 
            this.metroPanel2.Controls.Add(this.metroLabel11);
            this.metroPanel2.Controls.Add(this.mTglDownloadAnnex);
            this.metroPanel2.Controls.Add(this.mBtnPickDefaultMountpointDir);
            this.metroPanel2.Controls.Add(this.metroLabel9);
            this.metroPanel2.Controls.Add(this.mTxBDefaultMountpoint);
            this.metroPanel2.Controls.Add(this.mBtnPickDefaultCheckoutDir);
            this.metroPanel2.Controls.Add(this.metroLabel8);
            this.metroPanel2.Controls.Add(this.mTxBDefaultCheckout);
            this.metroPanel2.Controls.Add(this.mCBxRepoUpdates);
            this.metroPanel2.Controls.Add(this.metroLabel7);
            this.metroPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(0, 0);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(484, 252);
            this.metroPanel2.TabIndex = 0;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroLabel11
            // 
            this.metroLabel11.AutoSize = true;
            this.metroLabel11.Location = new System.Drawing.Point(13, 103);
            this.metroLabel11.Name = "metroLabel11";
            this.metroLabel11.Size = new System.Drawing.Size(173, 19);
            this.metroLabel11.TabIndex = 11;
            this.metroLabel11.Text = "Download all Annex\'ed data";
            // 
            // mTglDownloadAnnex
            // 
            this.mTglDownloadAnnex.AutoSize = true;
            this.mTglDownloadAnnex.Location = new System.Drawing.Point(201, 105);
            this.mTglDownloadAnnex.Name = "mTglDownloadAnnex";
            this.mTglDownloadAnnex.Size = new System.Drawing.Size(80, 17);
            this.mTglDownloadAnnex.TabIndex = 10;
            this.mTglDownloadAnnex.Text = "Off";
            this.mTglDownloadAnnex.UseSelectable = true;
            this.mTglDownloadAnnex.CheckedChanged += new System.EventHandler(this.mTglDownloadAnnex_CheckedChanged);
            // 
            // mBtnPickDefaultMountpointDir
            // 
            this.mBtnPickDefaultMountpointDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mBtnPickDefaultMountpointDir.Location = new System.Drawing.Point(457, 74);
            this.mBtnPickDefaultMountpointDir.Name = "mBtnPickDefaultMountpointDir";
            this.mBtnPickDefaultMountpointDir.Size = new System.Drawing.Size(24, 23);
            this.mBtnPickDefaultMountpointDir.TabIndex = 9;
            this.mBtnPickDefaultMountpointDir.Text = "...";
            this.mBtnPickDefaultMountpointDir.UseSelectable = true;
            this.mBtnPickDefaultMountpointDir.Click += new System.EventHandler(this.mBtnPickDefaultMountpointDir_Click);
            // 
            // metroLabel9
            // 
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.Location = new System.Drawing.Point(13, 74);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(180, 19);
            this.metroLabel9.TabIndex = 8;
            this.metroLabel9.Text = "Default Mountpoint Directory";
            // 
            // mTxBDefaultMountpoint
            // 
            this.mTxBDefaultMountpoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBDefaultMountpoint.CustomButton.Image = null;
            this.mTxBDefaultMountpoint.CustomButton.Location = new System.Drawing.Point(228, 1);
            this.mTxBDefaultMountpoint.CustomButton.Name = "";
            this.mTxBDefaultMountpoint.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBDefaultMountpoint.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBDefaultMountpoint.CustomButton.TabIndex = 1;
            this.mTxBDefaultMountpoint.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBDefaultMountpoint.CustomButton.UseSelectable = true;
            this.mTxBDefaultMountpoint.CustomButton.Visible = false;
            this.mTxBDefaultMountpoint.Lines = new string[] {
        "metroTextBox1"};
            this.mTxBDefaultMountpoint.Location = new System.Drawing.Point(201, 74);
            this.mTxBDefaultMountpoint.MaxLength = 32767;
            this.mTxBDefaultMountpoint.Name = "mTxBDefaultMountpoint";
            this.mTxBDefaultMountpoint.PasswordChar = '\0';
            this.mTxBDefaultMountpoint.ReadOnly = true;
            this.mTxBDefaultMountpoint.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBDefaultMountpoint.SelectedText = "";
            this.mTxBDefaultMountpoint.SelectionLength = 0;
            this.mTxBDefaultMountpoint.SelectionStart = 0;
            this.mTxBDefaultMountpoint.ShortcutsEnabled = true;
            this.mTxBDefaultMountpoint.Size = new System.Drawing.Size(250, 23);
            this.mTxBDefaultMountpoint.TabIndex = 7;
            this.mTxBDefaultMountpoint.Text = "metroTextBox1";
            this.mTxBDefaultMountpoint.UseSelectable = true;
            this.mTxBDefaultMountpoint.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBDefaultMountpoint.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mBtnPickDefaultCheckoutDir
            // 
            this.mBtnPickDefaultCheckoutDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mBtnPickDefaultCheckoutDir.Location = new System.Drawing.Point(457, 45);
            this.mBtnPickDefaultCheckoutDir.Name = "mBtnPickDefaultCheckoutDir";
            this.mBtnPickDefaultCheckoutDir.Size = new System.Drawing.Size(24, 23);
            this.mBtnPickDefaultCheckoutDir.TabIndex = 6;
            this.mBtnPickDefaultCheckoutDir.Text = "...";
            this.mBtnPickDefaultCheckoutDir.UseSelectable = true;
            this.mBtnPickDefaultCheckoutDir.Click += new System.EventHandler(this.mBtnPickDefaultCheckoutDir_Click);
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(13, 45);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(166, 19);
            this.metroLabel8.TabIndex = 5;
            this.metroLabel8.Text = "Default Checkout Directory";
            // 
            // mTxBDefaultCheckout
            // 
            this.mTxBDefaultCheckout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBDefaultCheckout.CustomButton.Image = null;
            this.mTxBDefaultCheckout.CustomButton.Location = new System.Drawing.Point(228, 1);
            this.mTxBDefaultCheckout.CustomButton.Name = "";
            this.mTxBDefaultCheckout.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBDefaultCheckout.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBDefaultCheckout.CustomButton.TabIndex = 1;
            this.mTxBDefaultCheckout.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBDefaultCheckout.CustomButton.UseSelectable = true;
            this.mTxBDefaultCheckout.CustomButton.Visible = false;
            this.mTxBDefaultCheckout.Lines = new string[] {
        "metroTextBox1"};
            this.mTxBDefaultCheckout.Location = new System.Drawing.Point(201, 45);
            this.mTxBDefaultCheckout.MaxLength = 32767;
            this.mTxBDefaultCheckout.Name = "mTxBDefaultCheckout";
            this.mTxBDefaultCheckout.PasswordChar = '\0';
            this.mTxBDefaultCheckout.ReadOnly = true;
            this.mTxBDefaultCheckout.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBDefaultCheckout.SelectedText = "";
            this.mTxBDefaultCheckout.SelectionLength = 0;
            this.mTxBDefaultCheckout.SelectionStart = 0;
            this.mTxBDefaultCheckout.ShortcutsEnabled = true;
            this.mTxBDefaultCheckout.Size = new System.Drawing.Size(250, 23);
            this.mTxBDefaultCheckout.TabIndex = 4;
            this.mTxBDefaultCheckout.Text = "metroTextBox1";
            this.mTxBDefaultCheckout.UseSelectable = true;
            this.mTxBDefaultCheckout.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBDefaultCheckout.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mCBxRepoUpdates
            // 
            this.mCBxRepoUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mCBxRepoUpdates.FormattingEnabled = true;
            this.mCBxRepoUpdates.ItemHeight = 23;
            this.mCBxRepoUpdates.Items.AddRange(new object[] {
            "Never",
            "Every 5 minutes",
            "Every 15 minutes",
            "Every 30 minutes",
            "Every 60 minutes"});
            this.mCBxRepoUpdates.Location = new System.Drawing.Point(201, 10);
            this.mCBxRepoUpdates.Name = "mCBxRepoUpdates";
            this.mCBxRepoUpdates.Size = new System.Drawing.Size(280, 29);
            this.mCBxRepoUpdates.TabIndex = 3;
            this.mCBxRepoUpdates.UseSelectable = true;
            this.mCBxRepoUpdates.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(13, 14);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(182, 19);
            this.metroLabel7.TabIndex = 2;
            this.metroLabel7.Text = "Check for Repository updates";
            // 
            // tpRepositories
            // 
            this.tpRepositories.Controls.Add(this.metroPanel3);
            this.tpRepositories.Location = new System.Drawing.Point(4, 38);
            this.tpRepositories.Name = "tpRepositories";
            this.tpRepositories.Size = new System.Drawing.Size(484, 252);
            this.tpRepositories.TabIndex = 2;
            this.tpRepositories.Text = "Repositories";
            this.tpRepositories.Visible = false;
            // 
            // metroPanel3
            // 
            this.metroPanel3.Controls.Add(this.mBtnCreateNew);
            this.metroPanel3.Controls.Add(this.mBtnCheckout);
            this.metroPanel3.Controls.Add(this.mBtnRemove);
            this.metroPanel3.Controls.Add(this.metroLabel10);
            this.metroPanel3.Controls.Add(this.mLVwRepositories);
            this.metroPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(0, 0);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(484, 252);
            this.metroPanel3.TabIndex = 0;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // mBtnCreateNew
            // 
            this.mBtnCreateNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnCreateNew.Location = new System.Drawing.Point(105, 226);
            this.mBtnCreateNew.Name = "mBtnCreateNew";
            this.mBtnCreateNew.Size = new System.Drawing.Size(75, 23);
            this.mBtnCreateNew.TabIndex = 16;
            this.mBtnCreateNew.Text = "Create New";
            this.mBtnCreateNew.UseSelectable = true;
            this.mBtnCreateNew.Click += new System.EventHandler(this.mBtnCreateNew_Click);
            // 
            // mBtnCheckout
            // 
            this.mBtnCheckout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnCheckout.Location = new System.Drawing.Point(24, 226);
            this.mBtnCheckout.Name = "mBtnCheckout";
            this.mBtnCheckout.Size = new System.Drawing.Size(75, 23);
            this.mBtnCheckout.TabIndex = 15;
            this.mBtnCheckout.Text = "Checkout";
            this.mBtnCheckout.UseSelectable = true;
            this.mBtnCheckout.Click += new System.EventHandler(this.mBtnCheckout_Click);
            // 
            // mBtnRemove
            // 
            this.mBtnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnRemove.Location = new System.Drawing.Point(188, 226);
            this.mBtnRemove.Name = "mBtnRemove";
            this.mBtnRemove.Size = new System.Drawing.Size(75, 23);
            this.mBtnRemove.TabIndex = 14;
            this.mBtnRemove.Text = "Remove";
            this.mBtnRemove.UseSelectable = true;
            this.mBtnRemove.Click += new System.EventHandler(this.mBtnRemove_Click);
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.Location = new System.Drawing.Point(24, 11);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(140, 19);
            this.metroLabel10.TabIndex = 3;
            this.metroLabel10.Text = "Managed Repositories";
            // 
            // mLVwRepositories
            // 
            this.mLVwRepositories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mLVwRepositories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHdName,
            this.colHdMountpoint,
            this.colHdCheckoutDir,
            this.colHdAddress});
            this.mLVwRepositories.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.mLVwRepositories.FullRowSelect = true;
            this.mLVwRepositories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mLVwRepositories.LabelWrap = false;
            this.mLVwRepositories.Location = new System.Drawing.Point(24, 33);
            this.mLVwRepositories.Name = "mLVwRepositories";
            this.mLVwRepositories.OwnerDraw = true;
            this.mLVwRepositories.Size = new System.Drawing.Size(457, 187);
            this.mLVwRepositories.TabIndex = 2;
            this.mLVwRepositories.UseCompatibleStateImageBehavior = false;
            this.mLVwRepositories.UseSelectable = true;
            this.mLVwRepositories.View = System.Windows.Forms.View.List;
            // 
            // colHdName
            // 
            this.colHdName.Text = "Name";
            // 
            // colHdMountpoint
            // 
            this.colHdMountpoint.Text = "Mountpoint";
            // 
            // colHdCheckoutDir
            // 
            this.colHdCheckoutDir.Text = "Checkout Directory";
            // 
            // colHdAddress
            // 
            this.colHdAddress.Text = "Address";
            // 
            // About
            // 
            this.About.Controls.Add(this.metroPanel4);
            this.About.Location = new System.Drawing.Point(4, 38);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(484, 252);
            this.About.TabIndex = 3;
            this.About.Text = "About";
            // 
            // metroPanel4
            // 
            this.metroPanel4.Controls.Add(this.metroLabel6);
            this.metroPanel4.Controls.Add(this.mTxBLicense);
            this.metroPanel4.Controls.Add(this.mTxBGinService);
            this.metroPanel4.Controls.Add(this.metroLabel5);
            this.metroPanel4.Controls.Add(this.mTxBGinCliVersion);
            this.metroPanel4.Controls.Add(this.metroLabel4);
            this.metroPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(0, 0);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(484, 252);
            this.metroPanel4.TabIndex = 0;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(16, 74);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(122, 19);
            this.metroLabel6.TabIndex = 7;
            this.metroLabel6.Text = "License Information";
            // 
            // mTxBLicense
            // 
            this.mTxBLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBLicense.CustomButton.Image = null;
            this.mTxBLicense.CustomButton.Location = new System.Drawing.Point(313, 1);
            this.mTxBLicense.CustomButton.Name = "";
            this.mTxBLicense.CustomButton.Size = new System.Drawing.Size(151, 151);
            this.mTxBLicense.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBLicense.CustomButton.TabIndex = 1;
            this.mTxBLicense.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBLicense.CustomButton.UseSelectable = true;
            this.mTxBLicense.CustomButton.Visible = false;
            this.mTxBLicense.Lines = new string[] {
        "metroTextBox1"};
            this.mTxBLicense.Location = new System.Drawing.Point(16, 96);
            this.mTxBLicense.MaxLength = 32767;
            this.mTxBLicense.Multiline = true;
            this.mTxBLicense.Name = "mTxBLicense";
            this.mTxBLicense.PasswordChar = '\0';
            this.mTxBLicense.ReadOnly = true;
            this.mTxBLicense.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBLicense.SelectedText = "";
            this.mTxBLicense.SelectionLength = 0;
            this.mTxBLicense.SelectionStart = 0;
            this.mTxBLicense.ShortcutsEnabled = true;
            this.mTxBLicense.Size = new System.Drawing.Size(465, 153);
            this.mTxBLicense.TabIndex = 6;
            this.mTxBLicense.Text = "metroTextBox1";
            this.mTxBLicense.UseSelectable = true;
            this.mTxBLicense.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBLicense.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mTxBGinService
            // 
            this.mTxBGinService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBGinService.CustomButton.Image = null;
            this.mTxBGinService.CustomButton.Location = new System.Drawing.Point(325, 1);
            this.mTxBGinService.CustomButton.Name = "";
            this.mTxBGinService.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBGinService.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBGinService.CustomButton.TabIndex = 1;
            this.mTxBGinService.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBGinService.CustomButton.UseSelectable = true;
            this.mTxBGinService.CustomButton.Visible = false;
            this.mTxBGinService.Lines = new string[] {
        "metroTextBox1"};
            this.mTxBGinService.Location = new System.Drawing.Point(133, 40);
            this.mTxBGinService.MaxLength = 32767;
            this.mTxBGinService.Name = "mTxBGinService";
            this.mTxBGinService.PasswordChar = '\0';
            this.mTxBGinService.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBGinService.SelectedText = "";
            this.mTxBGinService.SelectionLength = 0;
            this.mTxBGinService.SelectionStart = 0;
            this.mTxBGinService.ShortcutsEnabled = true;
            this.mTxBGinService.Size = new System.Drawing.Size(347, 23);
            this.mTxBGinService.TabIndex = 5;
            this.mTxBGinService.Text = "metroTextBox1";
            this.mTxBGinService.UseSelectable = true;
            this.mTxBGinService.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBGinService.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(16, 40);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(116, 19);
            this.metroLabel5.TabIndex = 4;
            this.metroLabel5.Text = "GinService Version";
            // 
            // mTxBGinCliVersion
            // 
            this.mTxBGinCliVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.mTxBGinCliVersion.CustomButton.Image = null;
            this.mTxBGinCliVersion.CustomButton.Location = new System.Drawing.Point(325, 1);
            this.mTxBGinCliVersion.CustomButton.Name = "";
            this.mTxBGinCliVersion.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mTxBGinCliVersion.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mTxBGinCliVersion.CustomButton.TabIndex = 1;
            this.mTxBGinCliVersion.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mTxBGinCliVersion.CustomButton.UseSelectable = true;
            this.mTxBGinCliVersion.CustomButton.Visible = false;
            this.mTxBGinCliVersion.Lines = new string[] {
        "metroTextBox1"};
            this.mTxBGinCliVersion.Location = new System.Drawing.Point(133, 11);
            this.mTxBGinCliVersion.MaxLength = 32767;
            this.mTxBGinCliVersion.Name = "mTxBGinCliVersion";
            this.mTxBGinCliVersion.PasswordChar = '\0';
            this.mTxBGinCliVersion.ReadOnly = true;
            this.mTxBGinCliVersion.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mTxBGinCliVersion.SelectedText = "";
            this.mTxBGinCliVersion.SelectionLength = 0;
            this.mTxBGinCliVersion.SelectionStart = 0;
            this.mTxBGinCliVersion.ShortcutsEnabled = true;
            this.mTxBGinCliVersion.Size = new System.Drawing.Size(347, 23);
            this.mTxBGinCliVersion.TabIndex = 3;
            this.mTxBGinCliVersion.Text = "metroTextBox1";
            this.mTxBGinCliVersion.UseSelectable = true;
            this.mTxBGinCliVersion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mTxBGinCliVersion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(16, 11);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(111, 19);
            this.metroLabel4.TabIndex = 2;
            this.metroLabel4.Text = "Gin Client Version";
            // 
            // mBtnOK
            // 
            this.mBtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnOK.Location = new System.Drawing.Point(28, 360);
            this.mBtnOK.Name = "mBtnOK";
            this.mBtnOK.Size = new System.Drawing.Size(75, 23);
            this.mBtnOK.TabIndex = 1;
            this.mBtnOK.Text = "OK";
            this.mBtnOK.UseSelectable = true;
            this.mBtnOK.Click += new System.EventHandler(this.mBtnOK_Click);
            // 
            // mBtnCancel
            // 
            this.mBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBtnCancel.Location = new System.Drawing.Point(110, 360);
            this.mBtnCancel.Name = "mBtnCancel";
            this.mBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.mBtnCancel.TabIndex = 2;
            this.mBtnCancel.Text = "Cancel";
            this.mBtnCancel.UseSelectable = true;
            this.mBtnCancel.Click += new System.EventHandler(this.mBtnCancel_Click);
            // 
            // mProgWorking
            // 
            this.mProgWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mProgWorking.Location = new System.Drawing.Point(486, 360);
            this.mProgWorking.Maximum = 100;
            this.mProgWorking.Name = "mProgWorking";
            this.mProgWorking.Size = new System.Drawing.Size(23, 23);
            this.mProgWorking.TabIndex = 17;
            this.mProgWorking.UseSelectable = true;
            this.mProgWorking.Value = 50;
            // 
            // mLblWorking
            // 
            this.mLblWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mLblWorking.AutoSize = true;
            this.mLblWorking.Location = new System.Drawing.Point(413, 360);
            this.mLblWorking.Name = "mLblWorking";
            this.mLblWorking.Size = new System.Drawing.Size(67, 19);
            this.mLblWorking.TabIndex = 18;
            this.mLblWorking.Text = "Working...";
            // 
            // MetroOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 397);
            this.Controls.Add(this.mLblWorking);
            this.Controls.Add(this.mBtnCancel);
            this.Controls.Add(this.mProgWorking);
            this.Controls.Add(this.mBtnOK);
            this.Controls.Add(this.mTabCtrl);
            this.Name = "MetroOptions";
            this.Text = "Options";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.mTabCtrl.ResumeLayout(false);
            this.tpUser.ResumeLayout(false);
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.tpGlobalOptions.ResumeLayout(false);
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.tpRepositories.ResumeLayout(false);
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            this.About.ResumeLayout(false);
            this.metroPanel4.ResumeLayout(false);
            this.metroPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl mTabCtrl;
        private System.Windows.Forms.TabPage tpUser;
        private System.Windows.Forms.TabPage tpGlobalOptions;
        private System.Windows.Forms.TabPage tpRepositories;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroLabel mLblStatus;
        private MetroFramework.Controls.MetroTextBox mTxBPassword;
        private MetroFramework.Controls.MetroTextBox mTxBUsername;
        private MetroFramework.Controls.MetroTextBox mTxBServerAddress;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton mBtnOK;
        private MetroFramework.Controls.MetroButton mBtnCancel;
        private System.Windows.Forms.TabPage About;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private MetroFramework.Controls.MetroPanel metroPanel4;
        private MetroFramework.Controls.MetroTextBox mTxBLicense;
        private MetroFramework.Controls.MetroTextBox mTxBGinService;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroTextBox mTxBGinCliVersion;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroButton mBtnPickDefaultMountpointDir;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroTextBox mTxBDefaultMountpoint;
        private MetroFramework.Controls.MetroButton mBtnPickDefaultCheckoutDir;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroTextBox mTxBDefaultCheckout;
        private MetroFramework.Controls.MetroComboBox mCBxRepoUpdates;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroButton mBtnCreateNew;
        private MetroFramework.Controls.MetroButton mBtnCheckout;
        private MetroFramework.Controls.MetroButton mBtnRemove;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private MetroFramework.Controls.MetroListView mLVwRepositories;
        private System.Windows.Forms.ColumnHeader colHdName;
        private System.Windows.Forms.ColumnHeader colHdMountpoint;
        private System.Windows.Forms.ColumnHeader colHdCheckoutDir;
        private System.Windows.Forms.ColumnHeader colHdAddress;
        private MetroFramework.Controls.MetroLabel metroLabel11;
        private MetroFramework.Controls.MetroToggle mTglDownloadAnnex;
        private MetroFramework.Controls.MetroLabel mLblWorking;
        private MetroFramework.Controls.MetroProgressSpinner mProgWorking;
    }
}