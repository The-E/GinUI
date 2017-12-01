using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp
{
    public partial class FileTransferProgress : UserControl
    {
        public FileTransferProgress()
        {
            InitializeComponent();
        }

        private void FileTransferProgress_Load(object sender, EventArgs e)
        {

        }

        public string Filename { get { return labelFilename.Text; } set { labelFilename.Text = value; } }
        public int Progress { get { return progressBar.Value; } set { progressBar.Value = value; } }
        public string Speed { get { return labelSpeed.Text; } set { labelSpeed.Text = value; } }
        public string State { get { return labelState.Text; } set { labelState.Text = value; } }
    }
}
