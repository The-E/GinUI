using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GinClientLibrary;
using MetroFramework.Forms;

namespace GinClientApp.Dialogs
{
    public partial class MetroUploadFilesDlg : MetroForm
    {
        private readonly IEnumerable<KeyValuePair<string, GinRepository.FileStatus>> _alteredFiles;

        public MetroUploadFilesDlg(IEnumerable<KeyValuePair<string, GinRepository.FileStatus>> alteredFiles)
        {
            InitializeComponent();
            _alteredFiles = alteredFiles;
        }

        private void MetroUploadFilesDlg_Load(object sender, EventArgs e)
        {
            foreach (var file in _alteredFiles)
                mLvwFiles.Items.Add(new ListViewItem(file.Key));
        }
    }
}