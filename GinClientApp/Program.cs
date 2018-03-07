using System;
using System.Threading;
using System.Windows.Forms;
using GinService;

namespace GinClientApp
{
    internal static class Program
    {
        static readonly Mutex Mutex = new Mutex(true, "{AC8AB48D-C289-445D-B1EB-ABCFF24443ED}" + Environment.UserName);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if ( args[0] == "-uninstall")
                {
                    Installer.DoUninstall();
                    return;
                }
            }
            if (!Mutex.WaitOne(TimeSpan.Zero, true)) return;

            var path = AppDomain.CurrentDomain.BaseDirectory;

            var value = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            value += ";" + path + @"gin-cli\bin";
            value += ";" + path + @"gin-cli\git\usr\bin";
            value += ";" + path + @"gin-cli\git\bin";
            Environment.SetEnvironmentVariable("PATH", value, EnvironmentVariableTarget.Process);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GinApplicationContext());
        }
    }
}