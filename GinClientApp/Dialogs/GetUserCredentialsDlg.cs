using System;
using System.ServiceModel;
using System.Windows.Forms;
using GinClientApp.GinService;
using GinClientApp.Properties;

namespace GinClientApp.Dialogs
{
    public partial class GetUserCredentialsDlg : Form
    {
        private GinServiceClient _client;
        private GinApplicationContext _parent;
        public string Username { get; private set; }
        public string Password { get; private set; }

        private void RecreateClient()
        {
            _client = new GinServiceClient(new InstanceContext(_parent));
            _client.InnerChannel.OperationTimeout = TimeSpan.MaxValue;
            _client.InnerDuplexChannel.OperationTimeout = TimeSpan.MaxValue;
        }

        public GetUserCredentialsDlg(GinApplicationContext parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Username)) return;

            RecreateClient();
            var loginResult = _client.Login(Username, Password);
            _client.Close();

            if (!loginResult)
            {
                MessageBox.Show(Resources.GetUserCredentials_Login_Unsuccessful, Resources.GetUserCredentials_Login_Failed,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            Username = txtUsername.Text;
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            Password = txtPassword.Text;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }
    }
}
