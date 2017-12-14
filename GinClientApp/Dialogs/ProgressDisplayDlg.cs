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

        public ProgressDisplayDlg()
        {
            InitializeComponent();
        }

        private void ProgressDisplay_Load(object sender, EventArgs e)
        {
        }

        public void AddFileTransfer(string filename)
        {
            lock (this)
            {
                var progBar = new FileTransferProgress();
                progBar.Filename = Path.GetFileName(filename);
                progBar.Dock = DockStyle.Bottom;
                _progressbars.Add(filename, progBar);
                Controls.Add(progBar);
            }
        }

        public void RemoveFileTransfer(string filename)
        {
            lock (this)
            {
                if (_progressbars.ContainsKey(filename))
                {
                    var progBar = _progressbars[filename];
                    Controls.Remove(progBar);
                    _progressbars.Remove(filename);
                    progBar.Dispose();
                }

                if (_progressbars.Count == 0)
                    Close();
            }
        }

        public void SetProgressBarState(string filename, string state, int progress, string rate)
        {
            lock (this)
            {
                if (_progressbars.ContainsKey(filename))
                {
                    var progBar = _progressbars[filename];

                    progBar.Progress = progress;
                    progBar.State = state;
                    progBar.Speed = rate;

                    var totalProgress = 0;

                    foreach (var prog in _progressbars.Values)
                        totalProgress += prog.Progress;

                    totalProgress /= _progressbars.Values.Count;

                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue(totalProgress, 100);
                }
            }
        }
    }
}