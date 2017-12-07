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
    }
}
