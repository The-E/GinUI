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
    public partial class MetroUploadFilesDlg : MetroFramework.Forms.MetroForm
    {
        private IEnumerable<KeyValuePair<string, GinClientLibrary.GinRepository.FileStatus>> _alteredFiles;
        public MetroUploadFilesDlg(IEnumerable<KeyValuePair<string, GinClientLibrary.GinRepository.FileStatus>> alteredFiles)
        {
            InitializeComponent();
            _alteredFiles = alteredFiles;
        }

        private void MetroUploadFilesDlg_Load(object sender, EventArgs e)
        {
            foreach (var file in _alteredFiles)
            {
                mLvwFiles.Items.Add(new ListViewItem(file.Key));
            }
        }
    }
}
