using System;
using System.Windows.Forms;

namespace GinClientApp.Custom_Controls
{
    public partial class FileTransferProgress : UserControl
    {
        public FileTransferProgress()
        {
            InitializeComponent();
        }

        public string Filename
        {
            get => labelFilename.Text;
            set => labelFilename.Text = value;
        }

        public int Progress
        {
            get => progressBar.Value;
            set => progressBar.Value = value;
        }

        public string Speed
        {
            get => labelSpeed.Text;
            set => labelSpeed.Text = value;
        }

        public string State
        {
            get => labelState.Text;
            set => labelState.Text = value;
        }

        private void FileTransferProgress_Load(object sender, EventArgs e)
        {
        }
    }
}