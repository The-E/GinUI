using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using GinClientApp.Custom_Controls;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace GinClientApp.Dialogs
{
    public partial class ProgressDisplayDlg : Form
    {
        private readonly Dictionary<string, FileTransferProgress> _progressbars =
            new Dictionary<string, FileTransferProgress>();

        public int NestingLevel;

        public ProgressDisplayDlg()
        {
            InitializeComponent();
        }

        private void ProgressDisplay_Load(object sender, EventArgs e)
        {
        }


        private delegate void SetProgressBarStateDelegate(string filename, string state, int progress, string rate);
        
        public void SetProgressBarState(string filename, string state, int progress, string rate)
        {
            if (InvokeRequired)
            {
                Invoke(new SetProgressBarStateDelegate(SetProgressBarState), filename, state, progress, rate);
            }
            else
            {
                fileTransferProgress1.Filename = filename;
                fileTransferProgress1.State = state;
                fileTransferProgress1.Progress = progress;
                fileTransferProgress1.Speed = rate;

                TaskbarManager.Instance.SetProgressValue(progress, 100);
            }

        }
    }
}