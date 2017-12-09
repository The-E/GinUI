using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp.Dialogs
{
    public partial class GetRepoNameDlg : Form
    {
        public string RepoName { get; set; }

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
