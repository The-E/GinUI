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
    public partial class UploadFilesDlg : Form
    {
        private IEnumerable<KeyValuePair<string, GinClientLibrary.GinRepository.FileStatus>> _files;

        public UploadFilesDlg(IEnumerable<KeyValuePair<string, GinClientLibrary.GinRepository.FileStatus>> alteredFiles)
        {
            InitializeComponent();

            _files = alteredFiles;
        }

        private void UploadFilesDlg_Load(object sender, EventArgs e)
        {
            foreach (var file in _files)
            {
                lvwFiles.Items.Add(new ListViewItem(file.Key));
            }
        }
    }
}
