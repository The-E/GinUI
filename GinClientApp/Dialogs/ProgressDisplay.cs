using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace GinClientApp
{
    public partial class ProgressDisplay : Form
    {
        private readonly Dictionary<string, FileTransferProgress> _progressbars =
            new Dictionary<string, FileTransferProgress>();

        public ProgressDisplay()
        {
            InitializeComponent();
        }

        private void ProgressDisplay_Load(object sender, EventArgs e)
        {
        }

        delegate void AddFileTransferDelegate(string filename);
        public void AddFileTransfer(string filename)
        {
            if (InvokeRequired)
            {
                AddFileTransferDelegate d = AddFileTransfer;
                Invoke(d, filename);
            }
            else
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
        }

        delegate void RemoveFileTransferDelegate(string filename);
        public void RemoveFileTransfer(string filename)
        {
            if (InvokeRequired)
            {

                RemoveFileTransferDelegate d = RemoveFileTransfer;
                Invoke(d, filename);
            }
            else
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
        }

        delegate void SetProgressBarStateDelegate(string filename, string state, int progress, string rate);

        public void SetProgressBarState(string filename, string state, int progress, string rate)
        {
            if (InvokeRequired)
            {
                SetProgressBarStateDelegate d = SetProgressBarState;
                Invoke(d, filename, state, progress, rate);
            }
            else
            {
                lock (this)
                {
                    if (!_progressbars.ContainsKey(filename)) return;
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