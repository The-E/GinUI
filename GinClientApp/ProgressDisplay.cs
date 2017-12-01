using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp
{
    public partial class ProgressDisplay : Form
    {
        Dictionary<string, FileTransferProgress> _progressbars = new Dictionary<string, FileTransferProgress>();

        public ProgressDisplay()
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
                    var _progBar = _progressbars[filename];
                    Controls.Remove(_progBar);
                    _progressbars.Remove(filename);
                    _progBar.Dispose();
                }

                if (_progressbars.Count == 0)
                {
                    Close();
                }
            }
        }

        public void SetProgressBarState(string filename, string state, int progress, string rate)
        {
            lock (this)
            {
                if (_progressbars.ContainsKey(filename))
                {
                    var _progBar = _progressbars[filename];

                    _progBar.Progress = progress;
                    _progBar.State = state;
                    _progBar.Speed = rate;

                    var totalProgress = 0;

                    foreach (var prog in _progressbars.Values)
                    {
                        totalProgress += prog.Progress;
                    }

                    totalProgress /= _progressbars.Values.Count;

                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue(totalProgress, 100);
                }
            }
        }
    }
}
