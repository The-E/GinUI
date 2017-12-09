using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp.Dialogs
{
    public partial class GetGINCmdline : Form
    {
        public string RepositoryName { get; set; }
        public bool CreateNew { get; set; }
        private GinApplicationContext.UserCredentials _credentials;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);


        public GetGINCmdline(GinApplicationContext.UserCredentials credentials)
        {
            InitializeComponent();
            SendMessage(txtRepoName.Handle, 0x1501, 1, "<username>/<repository>");
            _credentials = credentials;
        }

        private void txtCommandLine_TextChanged(object sender, EventArgs e)
        {
            RepositoryName = txtRepoName.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RepositoryName)) return;

            if (!RepositoryName.Contains('/') && !CreateNew)
            {
                var result = MessageBox.Show(
                    "The entered repository name is not valid.\nDo you wish to create a new repository named " +
                    RepositoryName + " under your username?", "Gin Client Warning", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    chbCreateNew.Checked = true;
                }
                else
                {
                    txtRepoName.Text = "";
                }
                return;
            }

            if (RepositoryName.Contains('/') && CreateNew)
            {
                var result =
                    MessageBox.Show(
                        "The entered repository name is not valid.\nDo you wish to check out an existing repository?",
                        "Gin Client Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    chbCreateNew.Checked = false;
                }
                else
                {
                    var repoString = RepositoryName.Substring(RepositoryName.IndexOf('/')).Trim('/');
                    RepositoryName = repoString;
                }
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void GetGINCmdline_Load(object sender, EventArgs e)
        {

        }

        private void chbCreateNew_CheckedChanged(object sender, EventArgs e)
        {
            CreateNew = chbCreateNew.Checked;
        }
    }
}
