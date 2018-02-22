using System;
using System.Threading;
using System.Windows.Forms;

namespace GinClientApp
{
    internal static class Program
    {
        static Mutex mutex = new Mutex(true, "{AC8AB48D-C289-445D-B1EB-ABCFF24443ED}");

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true)) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GinApplicationContext());
        }
    }
}