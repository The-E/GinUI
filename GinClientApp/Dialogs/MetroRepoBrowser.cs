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
    public partial class MetroRepoBrowser : MetroFramework.Forms.MetroForm
    {
        public string SelectedRepository { get; set; }

        public MetroRepoBrowser(GinApplicationContext context)
        {
            InitializeComponent();

            var repositories = context.ServiceClient.GetRemoteRepositoryList();

            //TODO: Parse the list of repositories, construct treenodes
        }

        private void mBtnCheckout_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void trVwRepositories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        private void trVwRepositories_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
