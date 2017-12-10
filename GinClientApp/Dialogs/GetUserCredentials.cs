using System;
using System.Windows.Forms;
using GinClientApp.GinService;
using GinClientApp.Properties;

namespace GinClientApp.Dialogs
{
    public partial class GetUserCredentials : Form
    {
        private readonly GinServiceClient _client;
        public string Username { get; private set; }
        public string Password { get; private set; }
        

        public GetUserCredentials(GinServiceClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Username)) return;

            if (!_client.Login(Username, Password))
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
