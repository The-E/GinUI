using System;
using System.Windows.Forms;

namespace GinClientApp.Dialogs
{
    public partial class GetRepoNameDlg : Form
    {
        public string RepoName { get; private set; }

        public GetRepoNameDlg()
        {
            InitializeComponent();
        }

        private void GetRepoNameDlg_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RepoName = textBox1.Text;
        }
    }
}
