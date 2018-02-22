using System;
using System.Threading;
using System.Windows.Forms;

namespace GinClientApp
{
    internal static class Program
    {
        static readonly Mutex Mutex = new Mutex(true, "{AC8AB48D-C289-445D-B1EB-ABCFF24443ED}" + Environment.UserName);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (!Mutex.WaitOne(TimeSpan.Zero, true)) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GinApplicationContext());
        }
    }
}