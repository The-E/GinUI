using System;
using System.Windows.Forms;
using GinClientApp.Properties;
using MetroFramework.Forms;

namespace GinClientApp.Dialogs
{
    public partial class MetroGetUserCredentialsDlg : MetroForm
    {
        private readonly GinApplicationContext _parentContext;

        public MetroGetUserCredentialsDlg(GinApplicationContext parentContext)
        {
            InitializeComponent();
            metroLabel1.TabStop = false;
            metroLabel2.TabStop = false;

            mLblWarning.Visible = false;

            _parentContext = parentContext;

            mTxBUsername.Text = UserCredentials.Instance.Username;
            mTxBPassword.Text = UserCredentials.Instance.Password;
        }

        private bool AttemptLogin()
        {
            if (string.IsNullOrEmpty(mTxBUsername.Text) || string.IsNullOrEmpty(mTxBPassword.Text)) return false;

            _parentContext.ServiceClient.Logout();

            return _parentContext.ServiceClient.Login(mTxBUsername.Text, mTxBPassword.Text);
        }

        private void mBtnOk_Click(object sender, EventArgs e)
        {
            mLblWarning.Visible = false;

            if (AttemptLogin())
            {
                UserCredentials.Instance.Password = mTxBPassword.Text;
                UserCredentials.Instance.Username = mTxBUsername.Text;

                UserCredentials.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                mLblWarning.Text = Resources.GetUserCredentials_The_entered_Username_Password_combination_is_invalid;
                mLblWarning.Visible = true;
            }
        }

        private void mBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}